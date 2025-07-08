using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Chats;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.CreateConversation
{
    internal class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateConversationCommandHandler> logger;
        public CreateConversationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateConversationCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<int> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var newConversation = new Conversation
            {
                ConversationMembers = [],
                Messages = [],
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

            return newConversation.Id;
        }
    }
}
