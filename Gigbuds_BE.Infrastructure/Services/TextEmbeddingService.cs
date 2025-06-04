using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Embeddings;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class TextEmbeddingService : ITextEmbeddingService
    {
        private readonly ILogger<TextEmbeddingService> _logger;
        private readonly EmbeddingClient _embeddingClient;
        private readonly IConfiguration _configuration;


        public TextEmbeddingService(ILogger<TextEmbeddingService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _embeddingClient = new EmbeddingClient(configuration["Embedding:Model"], configuration["Embedding:ApiKey"]);
        }

        public async Task<ReadOnlyMemory<float>> GenerateEmbeddingsAsync(string description)
        {
            try
            {
                EmbeddingGenerationOptions options = new()
                {
                    Dimensions = _configuration.GetValue<int>("Embedding:Dimensions"),
                };

                OpenAIEmbedding embedding = await _embeddingClient.GenerateEmbeddingAsync(description, options);
                return embedding.ToFloats();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating embeddings for description: {Description}", description);
                throw;
            }
        }
    }
}
