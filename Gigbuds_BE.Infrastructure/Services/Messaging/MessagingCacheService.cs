using AutoMapper;
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Domain.Entities.Chats;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gigbuds_BE.Infrastructure.Services.Messaging
{
    internal class MessagingCacheService : IMessagingCacheService
    {
        private readonly MessagesRepository _messagesRepositories;
        private readonly ConversationMetadataRepository _conversationMetadataRepository;
        private readonly ILogger<MessagingCacheService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessagingCacheService(
            MessagesRepository messagesRepositories,
            ConversationMetadataRepository conversationMetadataRepository,
            ILogger<MessagingCacheService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _messagesRepositories = messagesRepositories;
            _conversationMetadataRepository = conversationMetadataRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ConversationMetaDataDto?> GetConversationByIdAsync(int conversationId)
        {
            _logger.LogInformation("Getting conversation for ConversationId: {ConversationId}", conversationId);
            return await _conversationMetadataRepository.GetConversationMetadataByIdAsync(conversationId);
        }

        public async Task<List<ConversationMetaDataDto>> GetConversationsAsync(ConversationQueryParams queryParams)
        {
            _logger.LogInformation("Getting conversations for AccountId: {AccountId}, Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}",
                queryParams.UserId, queryParams.PageIndex, queryParams.PageSize, queryParams.SearchTerm);

            var resultConversations = await _conversationMetadataRepository.GetConversationMetadatasByUserIdAsync(
                queryParams.UserId,
                queryParams.PageIndex,
                queryParams.PageSize,
                queryParams.SearchTerm);

            _logger.LogInformation("Retrieved {Count} conversations for AccountId: {AccountId}", resultConversations?.Count ?? 0, queryParams.UserId);

            return resultConversations ?? [];
        }

        public async Task<List<ChatHistoryDto>> GetMessagesAsync(MessagesQueryParams queryParams)
        {
            _logger.LogInformation("Getting messages for ConversationId: {ConversationId}, Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}",
                queryParams.ConversationId, queryParams.PageIndex, queryParams.PageSize, queryParams.SearchTerm);

            var resultMessages = await _messagesRepositories.GetMessagesByConversationIdAsync(
                queryParams.ConversationId,
                queryParams.PageIndex,
                queryParams.PageSize,
                queryParams.SearchTerm);

            _logger.LogInformation("Retrieved {Count} messages for ConversationId: {ConversationId}", resultMessages?.Count ?? 0, queryParams.ConversationId);

            return resultMessages ?? [];
        }

        public async Task<List<ConversationMetaDataDto>> RetrieveConversationsFromServerAsync(ConversationQueryParams queryParams)
        {
            var specification = new GetConversationsSpecification(queryParams);
            var conversations = (await _unitOfWork.Repository<Conversation>()
                .GetAllWithSpecificationProjectedAsync<ConversationMetaDataDto>(specification, _mapper.ConfigurationProvider)).ToList();

            await _conversationMetadataRepository.UpsertConversationMetadataAsync(conversations);
            return conversations;
        }

        public async Task<List<ChatHistoryDto>> RetrieveMessagesFromServerAsync(MessagesQueryParams queryParams)
        {
            var specification = new GetMessagesSpecification(queryParams);
            var messages = (await _unitOfWork.Repository<Message>()
                .GetAllWithSpecificationProjectedAsync<ChatHistoryDto>(specification, _mapper.ConfigurationProvider)).ToList();

            await _messagesRepositories.UpsertMessagesAsync(messages);
            return messages;
        }

        public async Task<bool> UpsertConversationAsync(ConversationMetaDataDto conversation)
        {
            _logger.LogInformation("Upserting conversation metadata for ConversationId: {ConversationId}", conversation.Id);

            var result = await _conversationMetadataRepository.UpsertConversationMetadataAsync(conversation);

            _logger.LogInformation("Upsert conversation metadata result for ConversationId: {ConversationId}: {Result}", conversation.Id, result);

            return result;
        }

        public async Task<bool> UpsertMessagesAsync(ChatHistoryDto messages)
        {
            _logger.LogInformation("Upserting message for ConversationId: {ConversationId}, SenderName: {SenderName}", messages.ConversationId, messages.SenderName);

            var (success, isNew) = await _messagesRepositories.UpsertMessageAsync(messages);

            if (isNew) // if new message, update the latest message in conversation metadata
            {
                _logger.LogInformation("Updating latest message in conversation metadata for ConversationId: {ConversationId}", messages.ConversationId);

                var conversation = await _conversationMetadataRepository.GetConversationMetadataByIdAsync(messages.ConversationId);
                if (conversation != null)
                {
                    conversation.LastMessage = messages.Content;
                    conversation.LastMessageSenderName = messages.SenderName;
                    conversation.Timestamp = messages.Timestamp;
                    conversation.LastMessageId = messages.MessageId;
                    await _conversationMetadataRepository.UpsertConversationMetadataAsync(conversation);

                    _logger.LogInformation("Updated conversation metadata for ConversationId: {ConversationId} with latest message", messages.ConversationId);
                }
                else
                {
                    _logger.LogWarning("Conversation metadata not found for ConversationId: {ConversationId} when updating latest message", messages.ConversationId);
                }
            }

            _logger.LogInformation("Upsert message result for ConversationId: {ConversationId}: Success={Success}, IsNew={IsNew}", messages.ConversationId, success, isNew);

            return success;
        }
        public async Task<bool> EditMessageAsync(string messageId, int conversationId, string newContent)
        {
            _logger.LogInformation("Editing message for MessageId: {MessageId}, ConversationId: {ConversationId}, NewContent: {NewContent}", messageId, conversationId, newContent);
            var message = await _messagesRepositories.GetMessageByIdAsync(messageId);
            if (message != null)
            {
                message.Content = newContent;
                await _messagesRepositories.UpsertMessageAsync(message);
                var conversation = await _conversationMetadataRepository.GetConversationMetadataByIdAsync(conversationId);
                if (conversation != null && conversation.LastMessageId == messageId)
                {
                    conversation.LastMessage = newContent;
                    await _conversationMetadataRepository.UpsertConversationMetadataAsync(conversation);
                }
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteMessageAsync(string messageId, int conversationId)
        {
            _logger.LogInformation("Deleting message for MessageId: {MessageId}, ConversationId: {ConversationId}", messageId, conversationId);
            var message = await _messagesRepositories.GetMessageByIdAsync(messageId);
            if (message != null)
            {
                await _messagesRepositories.DeleteMessageAsync(messageId);
                var conversation = await _conversationMetadataRepository.GetConversationMetadataByIdAsync(conversationId);
                if (conversation != null && conversation.LastMessageId == messageId)
                {
                    conversation.LastMessage = "Tin nhắn đã bị xóa";
                    await _conversationMetadataRepository.UpsertConversationMetadataAsync(conversation);
                }
                return true;
            }
            return false;
        }
    }
}
