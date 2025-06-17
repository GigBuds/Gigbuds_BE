using Gigbuds_BE.Application.DTOs.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotificationsWeb
{
    public class GetStoredNotificationWebQuery : IRequest<List<NotificationDto>>
    {
        public required int UserId { get; set; }
    }
}
