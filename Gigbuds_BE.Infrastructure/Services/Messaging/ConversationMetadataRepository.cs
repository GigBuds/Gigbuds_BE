using Gigbuds_BE.Application.DTOs.Messages;
using Google.Apis.Util;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Redis.OM;
using Redis.OM.Searching;

namespace Gigbuds_BE.Infrastructure.Services.Messaging
{
    internal class ConversationMetadataRepository
    {
        private readonly ILogger<ConversationMetadataRepository> _logger;
        private readonly RedisCollection<ConversationMetaDataDto> _conversationMetaDataCollection;

        public ConversationMetadataRepository(RedisConnectionProvider provider, ILogger<ConversationMetadataRepository> logger)
        {
            _logger = logger;
            _conversationMetaDataCollection = (RedisCollection<ConversationMetaDataDto>)provider.RedisCollection<ConversationMetaDataDto>();
        }

        public async Task<string> GetLatestMessage(int conversationId)
        {
            _logger.LogInformation("Getting latest message for ConversationId: {ConversationId}", conversationId);
            try
            {
                var conversation = await _conversationMetaDataCollection.Where(c => c.Id == conversationId.ToString()).FirstOrDefaultAsync();
                if (conversation != null)
                {
                    return conversation.LastMessageId;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest message for ConversationId: {ConversationId}", conversationId);
                return string.Empty;
            }
        }
        public async Task<bool> UpsertConversationMetadataAsync(List<ConversationMetaDataDto> conversationMetaData)
        {
            _logger.LogInformation("Upserting conversation metadata for {Count} conversations", conversationMetaData.Count);
            try
            {
                var tasks = new List<Task<bool>>();
                foreach (var conversation in conversationMetaData)
                {
                    tasks.Add(UpsertConversationMetadataAsync(conversation));
                }
                await Task.WhenAll(tasks);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upserting conversation metadata");
                return false;
            }
        }

        public async Task<bool> UpsertConversationMetadataAsync(ConversationMetaDataDto conversationMetaData)
        {
            _logger.LogInformation("Upserting conversation metadata for ConversationId: {ConversationId}", conversationMetaData.Id);
            try
            {
                var existingConversation = await _conversationMetaDataCollection.Where(c => c.Id == conversationMetaData.Id).FirstOrDefaultAsync();
                if (existingConversation != null)
                {
                    conversationMetaData.MemberIds = [.. conversationMetaData.Members.Select(m => m.UserId.ToString())];
                    await _conversationMetaDataCollection.UpdateAsync(conversationMetaData);
                    _logger.LogInformation("Updated existing conversation metadata for ConversationId: {ConversationId}", conversationMetaData.Id);
                    return true;
                }
                else
                {
                    conversationMetaData.MemberIds = [.. conversationMetaData.Members.Select(m => m.UserId.ToString())];
                    await _conversationMetaDataCollection.InsertAsync(conversationMetaData);
                    _logger.LogInformation("Inserted new conversation metadata for ConversationId: {ConversationId}", conversationMetaData.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upserting conversation metadata {ConversationId}", conversationMetaData.Id);
                return false;
            }
        }

        public async Task<ConversationMetaDataDto?> GetConversationMetadataByIdAsync(int conversationId)
        {
            _logger.LogInformation("Getting conversation metadata by id {ConversationId}", conversationId);
            try
            {
                var conversationIdString = conversationId.ToString();
                var conversation = await _conversationMetaDataCollection
                    .Where(c => c.Id == conversationIdString)
                    .FirstOrDefaultAsync();

                if (conversation != null)
                {
                    _logger.LogInformation("Found conversation metadata for ConversationId: {ConversationId}", conversationId);
                }
                else
                {
                    _logger.LogWarning("No conversation metadata found for ConversationId: {ConversationId}", conversationId);
                }

                return conversation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversation metadata by id {ConversationId}", conversationId);
                return null;
            }
        }

        public async Task<List<ConversationMetaDataDto>> GetConversationMetadatasByUserIdAsync(int userId, int pageIndex, int pageSize, string? searchTerm = null)
        {
            _logger.LogInformation("Getting conversation metadatas for UserId: {UserId}, PageIndex: {PageIndex}, PageSize: {PageSize}, SearchTerm: {SearchTerm}", userId, pageIndex, pageSize, searchTerm);
            try
            {
                var userIdString = userId.ToString();
                IRedisCollection<ConversationMetaDataDto> query = userId == -1 ?
                    _conversationMetaDataCollection :
                    _conversationMetaDataCollection.Where(c => c.MemberIds.Contains(userIdString));

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(c =>
                        (c.NameOne != null && c.NameOne.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        || (c.NameTwo != null && c.NameTwo.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
                }

                IList<ConversationMetaDataDto> conversation = await query
                    .OrderByDescending(c => c.Timestamp)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} conversation metadatas for UserId: {UserId}", conversation.Count, userId);
                return conversation.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversation metadata from user {UserId}", userId);
                return [];
            }
        }

        public async Task<bool> DeleteConversationMetadata(int conversationId)
        {
            _logger.LogInformation("Deleting conversation metadata for ConversationId: {ConversationId}", conversationId);
            try
            {
                var conversationIdString = conversationId.ToString();
                var conversation = await _conversationMetaDataCollection
                    .Where(c => c.Id == conversationIdString).FirstOrDefaultAsync();
                if (conversation != null)
                {
                    await _conversationMetaDataCollection.DeleteAsync(conversation);
                    _logger.LogInformation("Deleted conversation metadata for ConversationId: {ConversationId}", conversationId);
                    return true;
                }
                _logger.LogWarning("No conversation metadata found to delete for ConversationId: {ConversationId}", conversationId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting conversation metadata {ConversationId}", conversationId);
                return false;
            }
        }
    }
}
