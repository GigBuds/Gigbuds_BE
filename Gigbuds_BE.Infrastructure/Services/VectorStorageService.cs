using Gigbuds_BE.Application.Interfaces.Services;
using Google.Protobuf.Collections;
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
            this._logger = logger;
            this.configuration = configuration;
            qdrantClient = new QdrantClient(configuration["Qdrant:Host"]!, configuration.GetValue<int>("Qdrant:Port"), https: true);
        }

        public async Task<List<string>> SearchBySemanticsAsync(
            string collectionName,
            float[] queryVector,
            List<string> payloadInclude,
            List<string> payloadExclude,
            int resultLimits = 0,
            int resultOffset = 0)
        {
            var payloadIncludeSelector = new PayloadIncludeSelector
            {
                Fields = { payloadInclude }
            };
            var payloadExcludeSelector = new PayloadExcludeSelector
            {
                Fields = { payloadExclude }
            };

            IReadOnlyList<ScoredPoint> results = await qdrantClient.SearchAsync(
            collectionName,
            queryVector,
            limit: (ulong)resultLimits,
            offset: (ulong)resultOffset,
            searchParams: new SearchParams
            {
                Exact = false,
            },
            payloadSelector: new WithPayloadSelector
            {
                Enable = true,
                Include = payloadIncludeSelector,
                Exclude = payloadExcludeSelector
            });

            return results.Select(point =>
            {
                _logger.LogInformation("Found point with ID: {Id}, Score: {Score}, Payload: {Payload}", point.Id, point.Score, point.Payload);
                point.Payload.TryGetValue("db-id", out var idValue);

                return idValue.StringValue;
            }).ToList();
        }

        public async Task UpsertPointAsync(string collectionName, IList<VectorWithPayload> vectorWithPayloads)
        {
            await CreateCollectionIfNotExists(collectionName);

            var newId = Guid.NewGuid();
            List<PointStruct> pointStructs = Enumerable.Range(0, vectorWithPayloads.Count - 1).Select(index => {
                var pointStruct = new PointStruct
                {
                    Id = PointId.Parser.ParseFrom(newId.ToByteArray()),
                    Vectors = vectorWithPayloads[index].Vector.ToArray(),
                };
                pointStruct.Payload.Add(UsePayloadAdapter(vectorWithPayloads[index].Payload));

                return pointStruct;
            }
            ).ToList();

            await qdrantClient.UpsertAsync(collectionName, pointStructs);

            _logger.LogInformation("Upserted {Count} points to collection '{CollectionName}' with base ID '{BaseId}'.", pointStructs.Count, collectionName, newId);
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
    }
}
