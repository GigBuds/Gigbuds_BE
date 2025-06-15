using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotifications
{
    internal class GetStoredNotificationsQueryHandler : IRequestHandler<GetStoredNotificationQuery, List<NotificationDto>>
    {
        private readonly INotificationStorageService _notificationStorageService;

        public GetStoredNotificationsQueryHandler(INotificationStorageService notificationStorageService)
        {
            _notificationStorageService = notificationStorageService;
        }

        public async Task<List<NotificationDto>> Handle(GetStoredNotificationQuery request, CancellationToken cancellationToken)
        {
            var storedNotifications = await _notificationStorageService.GetNotificationsAsync(request.DeviceId);
            if (storedNotifications.Count == 0)
            {
                return [];
            }
            return storedNotifications.OrderByDescending(n => n.Timestamp).ToList();
        }
    }
}
