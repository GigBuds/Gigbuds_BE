using Gigbuds_BE.Application.DTOs.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotifications
{
    public class GetStoredNotificationQuery : IRequest<List<NotificationDto>>
    {
        public required string DeviceId { get; set; }
    }
}
