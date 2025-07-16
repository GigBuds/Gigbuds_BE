using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Messaging;
using Gigbuds_BE.Domain.Entities.Chats;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Messaging.Commands.DeleteMessage
{
    internal class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteMessageCommandHandler> _logger;

        public DeleteMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteMessageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        async Task<int> IRequestHandler<DeleteMessageCommand, int>.Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete message with ID {MessageId}", request.MessageId);

            var specification = new GetMessageByIdSpecification(request.MessageId);
            var message = await _unitOfWork.Repository<Message>().GetBySpecificationAsync(specification, false);
            if (message == null)
            {
                _logger.LogWarning("Message with ID {MessageId} not found for deletion", request.MessageId);
                throw new NotFoundException(nameof(Message), request.MessageId);
            }
            message.IsEnabled = false;
            _unitOfWork.Repository<Message>().Update(message);
            try
            {
                return await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message with ID {MessageId}", request.MessageId);
                throw new Exception(ex.Message);
            }
        }
    }
}
