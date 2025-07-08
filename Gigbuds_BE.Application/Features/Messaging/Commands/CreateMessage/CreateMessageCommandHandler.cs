using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Domain.Entities.Chats;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.CreateMessage
{
    internal class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateMessageCommandHandler> logger;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateMessageCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<int> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            // TODO: uncomment 
            var specification = new GetConversationByIdSpecification(request.ConversationId);
            var updatedConversation = await unitOfWork.Repository<Conversation>().GetBySpecificationAsync(specification);
            var newMessage = new Message
            {
                ConversationId = request.ConversationId,
                Content = request.Content,
                AccountId = request.SenderId,
                SendDate = request.SentDate,
            };

            newMessage = unitOfWork.Repository<Message>().Insert(newMessage);

            updatedConversation!.UpdatedAt = DateTime.UtcNow;
            unitOfWork.Repository<Conversation>().Update(updatedConversation);

            try
            {
                await unitOfWork.CompleteAsync();
                logger.LogInformation("Message {MessageId} in conversation {ConversationId} created successfully",
                        newMessage.Id, newMessage.ConversationId);

                return newMessage.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating message");
                throw;
            }
        }
    }
}
