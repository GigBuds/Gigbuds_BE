
using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Features.Notifications.Commands.CreateNewNotification;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Infrastructure.Services.Messaging
{
    internal class MessagingService : IMessagingService
    {
        private readonly INotificationService _notificationService;
        private readonly IMessagingCacheService _messagingCacheService;
        private readonly IMediator _mediator;
        private readonly ILogger<MessagingService> _logger;
        private readonly IConnectionManager _connectionManager;
        private readonly IUnitOfWork _unitOfWork;

        public MessagingService(
            IMessagingCacheService messagingCacheService,
            IMediator mediator,
            ILogger<MessagingService> logger,
            IConnectionManager connectionManager,
            IUnitOfWork unitOfWork,
            INotificationService notificationService)
        {
            _messagingCacheService = messagingCacheService;
            _mediator = mediator;
            _logger = logger;
            _connectionManager = connectionManager;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        public Task AddUserToConversationAsync(int conversationId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task BroadcastToConversationAsync(int conversationId, string methodName, object data)
        {
            throw new NotImplementedException();
        }

        public Task CreateConversationAsync(ConversationDto conversation)
        {
            throw new NotImplementedException();
        }

        public Task HandleUserConnectedAsync(string connectionId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task HandleUserDisconnectedAsync(string connectionId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task JoinConversationGroupAsync(string connectionId, int conversationId)
        {
            throw new NotImplementedException();
        }

        public Task LeaveConversationGroupAsync(string connectionId, int conversationId)
        {
            throw new NotImplementedException();
        }

        public Task MarkMessagesAsReadAsync(int conversationId, int userId, List<int> messageIds)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromConversationAsync(int conversationId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task SendConversationEventAsync(int conversationId, string eventType, object eventData)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageDeliveredAsync(int conversationId, int messageId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageReadAsync(int conversationId, int messageId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendMessageToConversationAsync(int conversationId, int receiverId, ChatHistoryDto message, IMessagingClient clientProxy)
        {
            _logger.LogInformation("Sending message to conversation {ConversationId} to user {ReceiverId}", conversationId, receiverId);
            var isConnected = await IsClientConnectedAsync(message.SenderId);
            if (!isConnected)
            {
                try
                {
                    _logger.LogWarning("User {UserId} is not connected, sending notification to user", message.SenderId);

                    var createNotificationCommand = new CreateNewNotificationCommand
                    {
                        UserId = message.SenderId,
                        Message = message.Content,
                        ContentType = ContentType.NewMessageReceived,
                        CreatedAt = DateTime.UtcNow,
                        AdditionalPayload = new Dictionary<string, string>
                        {
                            { "conversationId", conversationId.ToString() },
                            { "messageId", message.MessageId.ToString() }
                        }
                    };

                    var notificationDto = await _mediator.Send(createNotificationCommand);

                    var userDevices = await _unitOfWork.Repository<DevicePushNotifications>()
                            .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(receiverId));

                    await _notificationService.NotifyOneUser(
                        typeof(INotificationForUser).GetMethod(nameof(INotificationForUser.NotifyNewMessageReceived))!,
                        userDevices.Select(a => a.DeviceToken!).ToList(),
                        receiverId.ToString(),
                        notificationDto
                    );
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending notification to user {UserId}", message.SenderId);
                    return false;
                }
            }

            var conversation = await _messagingCacheService.GetConversationByIdAsync(conversationId);
            if (conversation == null)
            {
                _logger.LogWarning("Conversation not found for ConversationId: {ConversationId}", conversationId);
                return false;
            }

            await clientProxy.ReceiveMessageAsync(conversation, message);
            return true;
        }

        public Task SendTypingIndicatorAsync(int conversationId, int userId, bool isTyping)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserOnlineStatusAsync(int userId, bool isOnline)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> IsClientConnectedAsync(int userId)
        {
            var isConnected = await _connectionManager.GetConnectionAsync(userId.ToString());
            return isConnected != null;
        }
    }
}
