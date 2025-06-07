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
        Task<List<(int, string)>> SearchBySemanticsAsync(
            string collectionName,
            ReadOnlyMemory<float> queryVector,
            QueryFilter? queryFilter = null,
            List<string>? payloadInclude = null,
            List<string>? payloadExclude = null,
            int resultLimits = 9999,
            int resultOffset = 0);
    }
    /// <summary>
    /// Represents a single condition for filtering query results in vector search.
    /// </summary>
    public record class QueryCondition
    {
        /// <summary>
        /// The name of the field to apply the condition on.
        /// </summary>
        public string FieldName { get; init; } = string.Empty;

        
        /// <summary>
        /// The bool value to match against the field (for equality or direct match).
        /// </summary>
        public bool? MatchBoolValue { get; init; } = null!;
        /// <summary>
        /// The string value to match against the field (for equality or direct match).
        /// </summary>
        public string? MatchStringValue { get; init; } = null!;

        /// <summary>
        /// A range (exclusive) for the field, specified as a tuple (min, max).
        /// </summary>
        public (int, int)? RangeWithoutEqual { get; init; }

        /// <summary>
        /// Specifies that the field value should be less than this value.
        /// </summary>
        public int? LessThan { get; init; }

        /// <summary>
        /// A range (inclusive) for the field, specified as a tuple (min, max).
        /// </summary>
        public (int, int)? RangeWithEqual { get; init; }

        /// <summary>
        /// Specifies that the field value should be greater than this value.
        /// </summary>
        public int? GreaterThan { get; init; }
    }

    /// <summary>
    /// Represents a set of query conditions for advanced filtering in vector search.
    /// </summary>
    public record class QueryFilter
    {
        /// <summary>
        /// List of conditions that must be satisfied (logical AND).
        /// </summary>
        public List<QueryCondition>? Must { get; init; }

        /// <summary>
        /// List of conditions that must not be satisfied (logical NOT).
        /// </summary>
        public List<QueryCondition>? MustNot { get; init; }

        /// <summary>
        /// List of conditions that should be satisfied (logical OR).
        /// </summary>
        public List<QueryCondition>? Should { get; init; }

        /// <summary>
        /// List of conditions where a minimum number should be satisfied.
        /// </summary>
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
