using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.Notifications.Commands.RegisterNotification
{
    internal class RegisterPushNotificationCommandHandler : IRequestHandler<RegisterPushNotificationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterPushNotificationCommandHandler> _logger;
        public RegisterPushNotificationCommandHandler(IUnitOfWork unitOfWork, ILogger<RegisterPushNotificationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(RegisterPushNotificationCommand request, CancellationToken cancellationToken)
        {
            var newDevicePushNotificationInfo = new DevicePushNotifications
            {
                DeviceToken = request.DeviceToken ?? null,
                DeviceId = request.DeviceId,
                DeviceType = request.DeviceType,
                DeviceName = request.DeviceName,
                DeviceModel = request.DeviceModel,
                DeviceManufacturer = request.DeviceManufacturer,
                AccountId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _unitOfWork.Repository<DevicePushNotifications>().Insert(newDevicePushNotificationInfo);
            
            try {
                await _unitOfWork.CompleteAsync();
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering push notification for user {UserId}", request.UserId);
                throw;
            }
        }
    }
}
