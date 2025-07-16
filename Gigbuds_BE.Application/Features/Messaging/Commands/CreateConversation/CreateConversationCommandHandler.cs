using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Domain.Entities.Chats;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.CreateConversation
{
    internal class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationMetaDataDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateConversationCommandHandler> logger;
        private readonly IMessagingCacheService _messagingCacheService;
        public CreateConversationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateConversationCommandHandler> logger, IMessagingCacheService messagingCacheService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this._messagingCacheService = messagingCacheService;
        }
        public async Task<ConversationMetaDataDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var newConversation = new Conversation
            {
                ConversationMembers = [],
                NameOne = request.ConversationNameOne,
                NameTwo = request.ConversationNameTwo,
                AvatarOne = request.AvatarOne,
                AvatarTwo = request.AvatarTwo,
                Messages = [],
                CreatorId = request.CreatorId,
                CreatedAt = request.CreatedAt,
            };

            newConversation = unitOfWork.Repository<Conversation>().Insert(newConversation);

            try
            {
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating conversation");
                throw;
            }

            foreach (var member in request.Members)
            {
                var conversationMember = new ConversationMember
                {
                    ConversationId = newConversation.Id,
                    AccountId = member.Key,
                    IsAdmin = member.Key == int.Parse(request.CreatorId),
                    JoinedDate = request.CreatedAt,
                };
                unitOfWork.Repository<ConversationMember>().Insert(conversationMember);
            }

            try
            {
                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating conversation");
                throw;
            }

            var newConversationMetadata = new ConversationMetaDataDto
            {
                Id = newConversation.Id.ToString(),
                LastMessageId = string.Empty,
                AvatarOne = request.AvatarOne,
                AvatarTwo = request.AvatarTwo,
                LastMessage = string.Empty,
                LastMessageSenderName = string.Empty,
                NameOne = request.ConversationNameOne,
                NameTwo = request.ConversationNameTwo,
                CreatorId = int.Parse(request.CreatorId),
                Members = request.Members.Select(m =>
                {
                    return new ConversationMemberDto
                    {
                        UserName = m.Value,
                        UserId = m.Key,
                    };
                }).ToArray(),
                MemberIds = request.Members.Select(m => m.Key.ToString()).ToArray(),
                Timestamp = DateTime.UtcNow,
            };
            await _messagingCacheService.UpsertConversationAsync(newConversationMetadata);
            return newConversationMetadata;
        }
    }
}
