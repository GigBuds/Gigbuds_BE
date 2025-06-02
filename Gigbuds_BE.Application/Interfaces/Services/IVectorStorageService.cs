namespace Gigbuds_BE.Application.Interfaces.Services
{
    public interface IVectorStorageService
    {
        /// <summary>
        /// Upserts a single vector point with associated payload into the specified collection.
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
        /// <param name="limit">The maximum number of results to return. Default is 0 (no limit).</param>
        /// <param name="offset">The number of results to skip before returning results. Default is 0.</param>
        /// <returns>A list of string identifiers for the found points.</returns>
        Task<List<string>> SearchBySemanticsAsync(
            string collectionName,
            float[] queryVector,
            List<string> payloadInclude,
            List<string> payloadExclude,
            int limit = 0,
            int offset = 0);
    }
    /// <summary>
    /// Represents a vector along with its associated payload metadata.
    /// </summary>
    public record class VectorWithPayload
    {
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
