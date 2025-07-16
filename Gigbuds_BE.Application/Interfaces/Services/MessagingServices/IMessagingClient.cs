using Gigbuds_BE.Application.DTOs.Messages;

namespace Gigbuds_BE.Application.Interfaces.Services.MessagingServices
{
    /// <summary>
    /// SignalR client interface for messaging operations
    /// This interface defines the methods that can be called on the client side
    /// </summary>
    public interface IMessagingClient
    {
        Task MessageEditedAsync(string messageId, int conversationId, string newContent);
        Task MessageDeletedAsync(string messageId, int conversationId);
        /// <summary>
        /// Send a typing indicator to a conversation
        /// </summary>
        Task ReceiveTypingIndicatorAsync(bool isTyping, string typerName, int conversationId);

        /// <summary>
        /// Send a message read status to a conversation
        /// </summary>
        Task ReceiveMessageAsync(ConversationMetaDataDto conversation, ChatHistoryDto message);

        /// <summary
        /// Send a message read status to a conversation
        /// </summary>
        Task ReceiveMessageReadStatusAsync(MessageStatusDto message);

        /// <summary>
        /// When user leaves a conversation
        /// </summary>
        Task RemovedFromConversationAsync(GroupSystemMessageDto groupSystemMessageDto, int userId);

        /// <summary>
        /// When user joins a conversation
        /// </summary>
        Task JoinedConversationGroupAsync(GroupSystemMessageDto groupSystemMessageDto, int userId);


        // ------------------------------------
        // === Message Status ===
        /// <summary>
        /// Mark messages as read by a user in a conversation
        /// </summary>
        Task MarkMessagesAsReadAsync(int conversationId, int userId, List<int> messageIds);

        /// <summary>
        /// Send message delivery confirmation
        /// </summary>
        Task SendMessageDeliveredAsync(int conversationId, int messageId, int userId);

        // === Group Operations ===
        /// <summary>
        /// Broadcast a message to all members of a conversation
        /// </summary>
        Task BroadcastToConversationAsync(int conversationId, string methodName, object data);

        /// <summary>
        /// Send notification about conversation events (user joined, left, etc.)
        /// </summary>
        Task SendConversationEventAsync(int conversationId, string eventType, object eventData);

        // === Connection Management ===
        /// <summary>
        /// Handle user connection - join them to their active conversations
        /// </summary>
        Task HandleUserConnectedAsync(string connectionId, int userId);

        /// <summary>
        /// Handle user disconnection - clean up conversation groups
        /// </summary>
        Task HandleUserDisconnectedAsync(string connectionId, int userId);

        Task UserOnlineAsync(int userId);
        Task UserDisconnectedAsync(OnlineStatusDto onlineStatusDto);

    }
}