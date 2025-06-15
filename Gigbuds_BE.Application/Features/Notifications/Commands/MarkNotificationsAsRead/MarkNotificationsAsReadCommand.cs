using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Commands.MarkNotificationsAsRead
{
    public class MarkNotificationsAsReadCommand : IRequest
    {
        public required string DeviceId { get; set; }
        public List<int> NotificationIds { get; set; } = [];
    }
}
