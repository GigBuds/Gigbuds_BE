using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class VectorStorageService : IVectorStorageService
    {
        private readonly QdrantClient qdrantClient;
        private readonly ILogger<VectorStorageService> _logger;
        private readonly IConfiguration configuration;

        public VectorStorageService(ILogger<VectorStorageService> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
            qdrantClient = new QdrantClient(configuration["Qdrant:Host"]!, configuration.GetValue<int>("Qdrant:Port"), https: true);
        }

        public async Task<List<(string, string)>> SearchBySemanticsAsync(
            string collectionName,
            ReadOnlyMemory<float> queryVector,
            QueryFilter? queryFilter = null,
            List<string>? payloadInclude = null,
            List<string>? payloadExclude = null,
            int resultLimits = 0,
            int resultOffset = 0)
        {
            // TODO: apply builder pattern for must, should, mustNot, minShould
            Filter? filter = null;
            if (queryFilter != null
                && queryFilter.Must != null
                && queryFilter.Must.Count != 0)
            {
                filter = new Filter
                {
                    Must = {
                        queryFilter.Must.Select(condition => new Condition
                        {
                            Field = new FieldCondition
                            {
                                Key = condition.FieldName,
                                Match = new Match
                                {
                                    Keyword = condition.Value.ToString()
                                }
                            }
                        })
                    }
                };
            }

            var payloadSelector = new WithPayloadSelector
            {
                Enable = true,
                Include = payloadInclude != null ? new PayloadIncludeSelector { Fields = { payloadInclude } } : null,
                Exclude = payloadExclude != null ? new PayloadExcludeSelector { Fields = { payloadExclude } } : null
            };

            IReadOnlyList<ScoredPoint> results = await qdrantClient.QueryAsync(
            collectionName,
            queryVector.ToArray(),
            filter: filter,
            limit: (ulong)resultLimits,
            offset: (ulong)resultOffset,
            searchParams: new SearchParams
            {
                Exact = false,
            },
            payloadSelector: payloadSelector);

            return results.Select(point =>
            {
                // TODO: Implement return payload fields
                _logger.LogInformation("Found point with ID: {Id}, Score: {Score}, Payload: {Payload}", point.Id, point.Score, point.Payload);
                point.Payload.TryGetValue(configuration["Qdrant:DefaultPointId"], out var idValue);
                point.Payload.TryGetValue("location", out var locationValue);

                return (idValue.StringValue, locationValue.StringValue);
            }).ToList();
        }

        public async Task UpsertPointAsync(string collectionName, IList<VectorWithPayload> vectorWithPayloads)
        {
            await CreateCollectionIfNotExists(collectionName);

            List<PointStruct> pointStructs = Enumerable.Range(0, vectorWithPayloads.Count - 1).Select(index =>
            {
                var newId = BitConverter.GetBytes(vectorWithPayloads[index].Id);
                var pointStruct = new PointStruct
                {
                    Id = PointId.Parser.ParseFrom(newId),
                    Vectors = vectorWithPayloads[index].Vector.ToArray(),
                };
                pointStruct.Payload.Add(UsePayloadAdapter(vectorWithPayloads[index].Payload));

                return pointStruct;
            }
            ).ToList();

            await qdrantClient.UpsertAsync(collectionName, pointStructs);

            _logger.LogInformation("Upserted {Count} points to collection '{CollectionName}'.", pointStructs.Count, collectionName);
        }
        private async Task CreateCollectionIfNotExists(string collectionName)
        {
            if (!qdrantClient.CollectionExistsAsync(collectionName).Result)
            {
                await qdrantClient.CreateCollectionAsync(collectionName, new VectorParams
                {
                    Size = configuration.GetValue<ulong>("Qdrant:VectorSize"),
                    Distance = Distance.Cosine
                });
                _logger.LogWarning("Collection '{CollectionName}' created successfully.", collectionName);
            }
            else
            {
                _logger.LogWarning("Collection '{CollectionName}' already exists.", collectionName);
            }
        }

        private static Dictionary<string, Value> UsePayloadAdapter(IDictionary<string, object> payload)
        {
            var payloadDict = new Dictionary<string, Value>();
            foreach (var kvp in payload)
            {
                var value = kvp.Value switch
                {
                    string str => new Value { StringValue = str },
                    int num => new Value { IntegerValue = num },
                    float f => new Value { DoubleValue = f },
                    double d => new Value { DoubleValue = d },
                    bool b => new Value { BoolValue = b },
                    Struct s => new Value { StructValue = s },
                    _ => new Value { StringValue = kvp.Value?.ToString() ?? "" }
                };
                payloadDict[kvp.Key] = value;
            }
            return payloadDict;
        }

        //private static IReadOnlyList<PrefetchQuery> UsePrefetchQueryAdapter(Dictionary<string, object> prefetchDict)
        //{
        //    return prefetchDict.Select(kvp => new PrefetchQuery
        //    {
        //         = kvp.Key,
        //        Ids = new[] { PointId.Parser.ParseFrom(Encoding.UTF8.GetBytes(kvp.Value.ToString()!)) }
        //    }).ToList();
        //}
    }
}
