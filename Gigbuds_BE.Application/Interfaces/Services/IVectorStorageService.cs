namespace Gigbuds_BE.Application.Interfaces.Services
{
    public interface IVectorStorageService
    {
        /// <summary>
        /// Upserts a single or multiple vector points with associated payload into the specified collection. 
        /// Duplicate id will overwrite the old points.
        /// </summary>
        /// <param name="collectionName">The name of the collection to upsert into</param>
        /// <param name="vector">The vector data to be stored</param>
        /// <param name="payload">Additional metadata to be stored with the vector</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpsertPointAsync(
            string collectionName,
            IList<VectorWithPayload> vectorWithPayloads);

        /// <summary>
        /// Searches for vector points in the specified collection using semantic similarity.
        /// </summary>
        /// <param name="collectionName">The name of the collection to search in.</param>
        /// <param name="queryVector">The query vector to compare against stored vectors.</param>
        /// <param name="payloadInclude">List of payload fields to include in the search results.</param>
        /// <param name="payloadExclude">List of payload fields to exclude from the search results.</param>
        /// <param name="resultLimits">The maximum number of results to return. Default is 0 (no limit).</param>
        /// <param name="resultOffset">The number of results to skip before returning results. Default is 0.</param>
        /// <returns>A list of tuples containing the id and payload fields for the found points.</returns>
        Task<List<(string, string)>> SearchBySemanticsAsync(
            string collectionName,
            ReadOnlyMemory<float> queryVector,
            QueryFilter? queryFilter = null,
            List<string>? payloadInclude = null,
            List<string>? payloadExclude = null,
            int resultLimits = 0,
            int resultOffset = 0);
    }
    public record class QueryCondition
    {
        public string FieldName { get; init; } = string.Empty;
        public object Value { get; init; } = null!;
    }
    public record class QueryFilter
    {
        public List<QueryCondition>? Must { get; init; }
        public List<QueryCondition>? MustNot { get; init; }
        public List<QueryCondition>? Should { get; init; }
        public List<QueryCondition>? MinShould { get; init; }
    }
    /// <summary>
    /// Represents a vector along with its associated payload metadata.
    /// </summary>
    public record class VectorWithPayload
    {
        /// <summary>
        /// The id of the vector.
        /// </summary>
        public int Id { get; init; }
        /// <summary>
        /// The vector data to be stored or processed.
        /// </summary>
        public ReadOnlyMemory<float> Vector { get; init; }

        /// <summary>
        /// Additional metadata to be stored with the vector.
        /// </summary>
        public IDictionary<string, object> Payload { get; init; } = new Dictionary<string, object>();
    }

}
