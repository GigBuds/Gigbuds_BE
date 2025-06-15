using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification
{
    public class CreateNewNotificationCommand : IRequest<NotificationDto>
    {
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public ContentType ContentType { get; set; }
        public int? JobPostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string>? AdditionalPayload { get; set; } = null;// This can be used for any additional data needed for the notification
    }
}
