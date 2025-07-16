namespace Gigbuds_BE.Application.DTOs.Messages
{
    public class MessageStatusDto(MessageStatus status)
    {
        public required int ConversationId { get; set; }
        public required int SenderId { get; set; }
        public required int ReceiverId { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public required string Status { get; set; } = status.ToString();
    }

    public enum MessageStatus
    {
        Sent,
        Delivered,
        Read
    }

}
