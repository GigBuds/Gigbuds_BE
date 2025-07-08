using Redis.OM.Modeling;

namespace Gigbuds_BE.Application.DTOs.Messages
{
    public class MessageDto
    {
        public required string Content { get; set; }
        public DateTime? SendDate { get; set; }
        public int MessageId { get; set; }
        public int ConversationId { get; set; } = -1;
        public required int SenderId { get; set; }
        public required string SenderName { get; set; }
        public required string SenderAvatar { get; set; }
    }


    [Document(StorageType = StorageType.Json, Prefixes = new[] { "ChatHistory" })]
    public class ChatHistoryDto
    {
        [RedisIdField]
        [Indexed]
        public required string MessageId { get; set; }

        [Indexed]
        public int ConversationId { get; set; }

        [Indexed]
        public int SenderId { get; set; }

        public required string SenderName { get; set; }

        public required string SenderAvatar { get; set; }

        public string[] ReadByNames { get; set; } = [];

        [Indexed(Sortable = true)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.Sending;

        [Searchable]
        public required string Content { get; set; }
    }
    public enum DeliveryStatus
    {
        Sending,
        Delivered,
        Read,
        Failed
    }
}