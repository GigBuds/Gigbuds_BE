using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.Templates;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification
{
    internal class CreateNewNotificationCommandHandler : IRequestHandler<CreateNewNotificationCommand, NotificationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateNewNotificationCommandHandler> _logger;
        public CreateNewNotificationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateNewNotificationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<NotificationDto> Handle(CreateNewNotificationCommand request, CancellationToken cancellationToken)
        {
            var template = await _unitOfWork.Repository<Template>().GetBySpecificationAsync(new GetByTemplateTypeSpecification(request.ContentType));
            var newNotification = new Notification
            {
                Message = request.Message,
                TemplateId = template!.Id,
                AccountId = request.UserId,
                JobPostId = request.JobPostId,
                CreatedAt = DateTime.UtcNow,
            };

            _unitOfWork.Repository<Notification>().Insert(newNotification);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new notification for user {UserId}", request.UserId);
                throw;
            }

            try
            {
                var notificationDto = new NotificationDto(
                    id: newNotification.Id,
                    content: request.Message,
                    timestamp: request.CreatedAt,
                    additionalPayload: request.AdditionalPayload);
                return notificationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving notification for user {UserId} in storage", request.UserId);
                throw;
            }
        }
    }
}
