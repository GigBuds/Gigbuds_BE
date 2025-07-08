using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Domain.Entities.Chats;
using Microsoft.AspNetCore.SignalR;

namespace Gigbuds_BE.Application.Interfaces.Services.MessagingServices
{

    public interface IMessagingService
    {
        Task<bool> SendMessageToConversationAsync(int conversationId, int receiverId, ChatHistoryDto message, IMessagingClient clientProxy);

        /// <summary>
        /// Send a typing indicator to a conversation
        /// </summary>
        Task SendTypingIndicatorAsync(int conversationId, int userId, bool isTyping);

        // === Conversation Management ===
        /// <summary>
        /// Create a new conversation and add initial members
        /// </summary>
        Task CreateConversationAsync(ConversationDto conversation);

        /// <summary>
        /// Add a user to an existing conversation
        /// </summary>
        Task AddUserToConversationAsync(int conversationId, int userId);

        /// <summary>
        /// Remove a user from a conversation
        /// </summary>
        Task RemoveUserFromConversationAsync(int conversationId, int userId);

        /// <summary>
        /// Join a user to a SignalR group for the conversation
        /// </summary>
        Task JoinConversationGroupAsync(string connectionId, int conversationId);

        /// <summary>
        /// Remove a user from a SignalR group for the conversation
        /// </summary>
        Task LeaveConversationGroupAsync(string connectionId, int conversationId);

        // === Message Status ===
        /// <summary>
        /// Mark messages as read by a user in a conversation
        /// </summary>
        Task MarkMessagesAsReadAsync(int conversationId, int userId, List<int> messageIds);

        /// <summary>
        /// Send message delivery confirmation
        /// </summary>
        Task SendMessageDeliveredAsync(int conversationId, int messageId, int userId);

        /// <summary>
        /// Send message read confirmation
        /// </summary>
        Task SendMessageReadAsync(int conversationId, int messageId, int userId);

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

        /// <summary>
        /// Update user's online status in their conversations
        /// </summary>
        Task UpdateUserOnlineStatusAsync(int userId, bool isOnline);
    }
}
