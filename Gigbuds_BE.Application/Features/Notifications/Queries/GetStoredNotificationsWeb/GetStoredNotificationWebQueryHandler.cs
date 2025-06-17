
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotificationsWeb
{
    internal class GetStoredNotificationWebQueryHandler : IRequestHandler<GetStoredNotificationWebQuery, List<NotificationDto>>
    {
        private readonly INotificationStorageService _notificationStorageService;

        public GetStoredNotificationWebQueryHandler(INotificationStorageService notificationStorageService)
        {
            _notificationStorageService = notificationStorageService;
        }

        public Task<List<NotificationDto>> Handle(GetStoredNotificationWebQuery request, CancellationToken cancellationToken)
        {
            return _notificationStorageService.GetNotificationsAsync(request.UserId.ToString());
        }
    }
}