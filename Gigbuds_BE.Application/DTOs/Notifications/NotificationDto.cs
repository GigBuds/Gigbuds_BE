using Gigbuds_BE.Domain.Entities.Notifications;

namespace Gigbuds_BE.Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; } = false; // whether the notification has been read by the user
        public Dictionary<string, string>? AdditionalPayload { get; set; } = null;

        public string Type { get; set; }
        public string? Title { get; set; }

        public NotificationDto(int id, string content, Dictionary<string, string>? additionalPayload, DateTime timestamp)
        {
            Id = id;
            Content = content;
            AdditionalPayload = additionalPayload;
            Timestamp = timestamp;
        }
    }
}
