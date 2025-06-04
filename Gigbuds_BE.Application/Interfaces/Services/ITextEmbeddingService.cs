namespace Gigbuds_BE.Application.Interfaces.Services
{
    public interface ITextEmbeddingService
    {
        /// <summary>
        /// Generates a text embedding vector for the provided description using an embedding model.
        /// </summary>
        /// <param name="description">The input text to generate embeddings for.</param>
        /// <returns>A task that represents the asynchronous operation, containing the embedding as a read-only memory of floats.</returns>
        Task<ReadOnlyMemory<float>> GenerateEmbeddingsAsync(string description);
    }
}
