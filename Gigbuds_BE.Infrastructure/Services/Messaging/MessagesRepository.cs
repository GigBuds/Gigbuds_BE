using Castle.Core.Logging;
using Gigbuds_BE.Application.DTOs.Messages;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Redis.OM;
using Redis.OM.Searching;

namespace Gigbuds_BE.Infrastructure.Services.Messaging
{
    internal class MessagesRepository
    {
        private readonly ILogger<MessagesRepository> _logger;
        private readonly RedisCollection<ChatHistoryDto> _chatHistoryCollection;

        public MessagesRepository(RedisConnectionProvider provider, ILogger<MessagesRepository> logger)
        {
            _chatHistoryCollection = (RedisCollection<ChatHistoryDto>)provider.RedisCollection<ChatHistoryDto>();
            _logger = logger;
        }

        public async Task<bool> UpsertMessagesAsync(List<ChatHistoryDto> messages)
        {
            try
            {
                var tasks = new List<Task<(bool success, bool isNew)>>();
                foreach (var message in messages)
                {
                    tasks.Add(UpsertMessageAsync(message));
                }
                await Task.WhenAll(tasks);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upserting messages");
                return false;
            }
        }
        public async Task<(bool success, bool isNew)> UpsertMessageAsync(ChatHistoryDto message)
        {
            try
            {
                var existingMessage = await _chatHistoryCollection.Where(m => m.MessageId == message.MessageId).FirstOrDefaultAsync();
                if (existingMessage != null)
                {
                    message.SenderAvatar ??= "https://pngtree.com/so/photo-placeholder";
                    await _chatHistoryCollection.UpdateAsync(message);
                    return (true, false);
                }
                else
                {
                    message.SenderAvatar ??= "https://pngtree.com/so/photo-placeholder";
                    await _chatHistoryCollection.InsertAsync(message);
                    return (true, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding message to chat history {MessageId}", message.MessageId);
                return (false, false);
            }
        }

        public async Task<List<ChatHistoryDto>> GetMessagesByConversationIdAsync(int conversationId, int pageIndex, int pageSize = 5, string? searchTerm = null)
        {
            try
            {
                IQueryable<ChatHistoryDto> query = _chatHistoryCollection
                    .Where(m => m.ConversationId == conversationId);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(m => m.Content.Contains(searchTerm));
                }

                var messages = await query
                    .OrderByDescending(c => c.Timestamp)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToListAsync();

                return messages.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for conversation {ConversationId}", conversationId);
                return [];
            }
        }

        public async Task<List<ChatHistoryDto>> GetMessagesAsync(string searchText, int pageIndex, int pageSize = 5)
        {
            try
            {
                var messages = await _chatHistoryCollection.Where(m => m.Content.Contains(searchText))
                        .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                return messages.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for search text {SearchText}", searchText);
                return [];
            }
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            try
            {
                var idString = messageId.ToString();
                var message = await _chatHistoryCollection.Where(m => m.MessageId == idString).FirstOrDefaultAsync();
                if (message != null)
                {
                    await _chatHistoryCollection.DeleteAsync(message);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message {MessageId}", messageId);
                return false;
            }
        }

        public async Task<bool> DeleteMessagesByConversationIdAsync(int conversationId)
        {
            try
            {
                var messages = await _chatHistoryCollection.Where(m => m.ConversationId == conversationId).ToListAsync();
                var tasks = new List<Task>();
                foreach (var message in messages)
                {
                    tasks.Add(_chatHistoryCollection.DeleteAsync(message));
                }
                await Task.WhenAll(tasks);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting messages for conversation {ConversationId}", conversationId);
                return false;
            }
        }
    }
}
