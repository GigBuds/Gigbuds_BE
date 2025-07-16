using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Specifications.Messaging;

namespace Gigbuds_BE.Application.Interfaces.Services.MessagingServices
{
    public interface IMessagingCacheService
    {
        public Task<List<ConversationMetaDataDto>> GetConversationsAsync(ConversationQueryParams queryParams);
        public Task<ConversationMetaDataDto?> GetConversationByIdAsync(int conversationId);
        public Task<List<ChatHistoryDto>> GetMessagesAsync(MessagesQueryParams queryParams);
        public Task<bool> UpsertConversationAsync(ConversationMetaDataDto conversation);
        public Task<bool> UpsertMessagesAsync(ChatHistoryDto messages);
        public Task<List<ConversationMetaDataDto>> RetrieveConversationsFromServerAsync(ConversationQueryParams queryParams);
        public Task<List<ChatHistoryDto>> RetrieveMessagesFromServerAsync(MessagesQueryParams queryParams);
        public Task<bool> EditMessageAsync(string messageId, int conversationId, string newContent);
        public Task<bool> DeleteMessageAsync(string messageId, int conversationId);
    }
}
