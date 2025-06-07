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
            qdrantClient = new QdrantClient(configuration["VectorDb:Host"]!, apiKey: configuration["VectorDb:ApiKey"], https: true);
        }

        public async Task<List<(int, string)>> SearchBySemanticsAsync(
            string collectionName,
            ReadOnlyMemory<float> queryVector,
            QueryFilter? queryFilter = null,
            List<string>? payloadInclude = null,
            List<string>? payloadExclude = null,
            int resultLimits = 9999,
            int resultOffset = 0)
        {
            Filter? filter = null;
            if (queryFilter?.Must != null && queryFilter.Must.Count != 0)
            {
                var mustConditions = queryFilter.Must.Select(condition =>
                {
                    var fieldCondition = new FieldCondition
                    {
                        Key = condition.FieldName
                    };
                    if (condition.GreaterThan != null)
                    {
                        fieldCondition.Range = new Qdrant.Client.Grpc.Range
                        {
                            Gt = condition.GreaterThan.Value
                        };
                    }

                    if (condition.LessThan != null)
                    {
                        fieldCondition.Range = new Qdrant.Client.Grpc.Range
                        {
                            Lt = condition.LessThan.Value
                        };
                    }

                    if (condition.MatchStringValue != null)
                    {
                        fieldCondition.Match = new Match
                        {
                            Text = condition.MatchStringValue!
                        };
                    }

                    if (condition.MatchBoolValue != null)
                    {
                        fieldCondition.Match = new Match
                        {
                            Boolean = (bool)condition.MatchBoolValue
                        };
                    }

                    return new Condition { Field = fieldCondition };
                }).ToList();

                filter = new Filter();
                filter.Must.AddRange(mustConditions);
            }
            WithPayloadSelector? payloadSelector;

            if (payloadInclude == null && payloadExclude == null)
            {
                payloadSelector = null;
            }
            else
            {
                payloadSelector = new WithPayloadSelector();
            }

            if (payloadInclude != null && payloadInclude.Count > 0)
            {
                payloadSelector.Include = new PayloadIncludeSelector
                {
                    Fields = { payloadInclude }
                };
            }
            if (payloadExclude != null && payloadExclude.Count > 0)
            {
                payloadSelector.Exclude = new PayloadExcludeSelector
                {
                    Fields = { payloadExclude }
                };
            }

            var results = await qdrantClient.SearchAsync(
                collectionName,
                queryVector.ToArray(),
                limit: (ulong)resultLimits,
                payloadSelector: payloadSelector,
                scoreThreshold: 0.6f);

            return results.Select(point =>
            {
                _logger.LogInformation("Found point with ID: {Id}, Score: {Score}, Payload: {Payload}", point.Id, point.Score, point.Payload);
                point.Payload.TryGetValue("db-id"!, out var idValue);
                point.Payload.TryGetValue("location", out var locationValue);

                return (((int)idValue?.IntegerValue!), locationValue?.StringValue ?? string.Empty);
            }).ToList();
        }

        public async Task UpsertPointAsync(string collectionName, IList<VectorWithPayload> vectorWithPayloads)
        {
            await CreateCollectionIfNotExists(collectionName);

            var pointStructs = vectorWithPayloads.Select((vector, index) =>
            {
                var pointStruct = new PointStruct
                {
                    Id = (ulong)vector.Id,
                    Vectors = vector.Vector.ToArray()
                };
                pointStruct.Payload.Add(UsePayloadAdapter(vector.Payload));

                return pointStruct;
            }).ToList();

            await qdrantClient.UpsertAsync(collectionName, pointStructs);
            _logger.LogInformation("Upserted {Count} points to collection '{CollectionName}'.", pointStructs.Count, collectionName);
        }

        private async Task CreateCollectionIfNotExists(string collectionName)
        {
            if (!await qdrantClient.CollectionExistsAsync(collectionName))
            {
                await qdrantClient.CreateCollectionAsync(collectionName, new VectorParams
                {
                    Size = configuration.GetValue<ulong>("VectorDb:VectorSize"),
                    Distance = Distance.Cosine
                });

                await AddIndexAsync(collectionName);

                _logger.LogWarning("Collection '{CollectionName}' created successfully.", collectionName);
            }
            else
            {
                _logger.LogWarning("Collection '{CollectionName}' already exists.", collectionName);
            }
        }

        private async Task AddIndexAsync(string collectionName)
        {
            await qdrantClient.CreatePayloadIndexAsync(
                collectionName,
                "age",
                PayloadSchemaType.Integer, new PayloadIndexParams { IntegerIndexParams = new IntegerIndexParams { Range = true } });
            await qdrantClient.CreatePayloadIndexAsync(
                collectionName,
                "isEnabled",
                PayloadSchemaType.Bool, new PayloadIndexParams { BoolIndexParams = new BoolIndexParams() });
            await qdrantClient.CreatePayloadIndexAsync(
                collectionName,
                "isMale",
                PayloadSchemaType.Bool, new PayloadIndexParams { BoolIndexParams = new BoolIndexParams() });
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
    }
}
