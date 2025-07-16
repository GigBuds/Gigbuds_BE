namespace Gigbuds_BE.Application.DTOs.Messages
{
    public class GroupSystemMessageDto
    {
        public GroupSystemMessageType MessageType { get; set; }
        public string ConversationId { get; set; }
    }
    public enum GroupSystemMessageType
    {
        GroupCreated,
        GroupDeleted,
        PersonJoined,
        PersonLeft,
        UserAddedToGroup,
        UserRemovedFromGroup
    }

    public static class GroupSystemMessages
    {
        public static string GetSystemMessage(GroupSystemMessageType messageType, string? userName = null)
        {
            return messageType switch
            {
                GroupSystemMessageType.GroupCreated => "Nhóm đã được tạo",
                GroupSystemMessageType.GroupDeleted => "Nhóm đã bị xóa",
                GroupSystemMessageType.PersonJoined => $"{userName} đã tham gia nhóm",
                GroupSystemMessageType.PersonLeft => $"{userName} đã rời khỏi nhóm",
                GroupSystemMessageType.UserAddedToGroup => "Bạn đã được thêm vào nhóm",
                GroupSystemMessageType.UserRemovedFromGroup => "Bạn đã bị xóa khỏi nhóm",
                _ => throw new ArgumentException("Invalid message type", nameof(messageType))
            };
        }
    }
}
