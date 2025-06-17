using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotifications
{
    internal class GetStoredNotificationsQueryHandler : IRequestHandler<GetStoredNotificationQuery, List<NotificationDto>>
    {
        private readonly INotificationStorageService _notificationStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public GetStoredNotificationsQueryHandler(INotificationStorageService notificationStorageService, IUnitOfWork unitOfWork)
        {
            _notificationStorageService = notificationStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NotificationDto>> Handle(GetStoredNotificationQuery request, CancellationToken cancellationToken)
        {
            var device = await _unitOfWork.Repository<DevicePushNotifications>().GetBySpecificationAsync(new GetDeviceByDeviceIdSpecification(request.DeviceId));
            var storedNotifications = await _notificationStorageService.GetNotificationsAsync(device!.AccountId.ToString());
            if (storedNotifications.Count == 0)
            {
                return [];
            }
            return storedNotifications.OrderByDescending(n => n.Timestamp).ToList();
        }
    }
}
