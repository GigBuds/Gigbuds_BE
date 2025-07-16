
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Domain.Entities.Chats;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.UpdateMessage
{
    internal class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateMessageCommandHandler> _logger;
        public UpdateMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateMessageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update message with ID {MessageId}", request.UpdatedEntity.MessageId);

            var specification = new GetMessageByIdSpecification(int.Parse(request.UpdatedEntity.MessageId));
            var result = await _unitOfWork.Repository<Message>().GetBySpecificationAsync(specification, false);
            if (result == null)
            {
                _logger.LogWarning("Message with ID {MessageId} not found for update", request.UpdatedEntity.MessageId);
                throw new NotFoundException(nameof(Message), request.UpdatedEntity.MessageId);
            }
            result.Content = request.UpdatedEntity.Content;
            _unitOfWork.Repository<Message>().Update(result);

            try
            {
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Successfully updated message with ID {MessageId}", request.UpdatedEntity.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating message with ID {MessageId}", request.UpdatedEntity.MessageId);
                throw new Exception(ex.Message);
            }
        }
    }
}
