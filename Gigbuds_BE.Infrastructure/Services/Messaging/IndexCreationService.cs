using Gigbuds_BE.Application.DTOs.Messages;
using Microsoft.Extensions.Hosting;
using Redis.OM;

namespace Gigbuds_BE.Infrastructure.Services.Messaging
{
    /// <summary>
    /// Hosted service responsible for creating Redis search indexes for messaging-related DTOs.
    /// Ensures that ConversationMetaDataDto and ChatHistoryDto are indexed at application startup.
    /// </summary>
    public class IndexCreationService : IHostedService
    {
        private readonly RedisConnectionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexCreationService"/> class.
        /// </summary>
        /// <param name="provider">The Redis connection provider used to create indexes.</param>
        public IndexCreationService(RedisConnectionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Called when the application host is ready to start the service.
        /// Creates Redis search indexes for ConversationMetaDataDto and ChatHistoryDto.
        /// </summary>
        /// <param name="cancellationToken">A token to signal cancellation.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _provider.Connection.CreateIndexAsync(typeof(ConversationMetaDataDto));
            await _provider.Connection.CreateIndexAsync(typeof(ChatHistoryDto));
        }

        /// <summary>
        /// Called when the application host is performing a graceful shutdown.
        /// No action is required for this service on stop.
        /// </summary>
        /// <param name="cancellationToken">A token to signal cancellation.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
