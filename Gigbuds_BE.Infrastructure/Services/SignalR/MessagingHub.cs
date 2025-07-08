using Gigbuds_BE.Application.DTOs.Messages;
using Gigbuds_BE.Application.Features.Messaging.Commands.CreateConversation;
using Gigbuds_BE.Application.Features.Messaging.Commands.CreateMessage;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    public class MessagingHub : Hub<IMessagingClient>
    {
        private readonly IMediator _mediator;
        private readonly IMessagingCacheService _messagingCacheService;
        private readonly IMessagingConnectionManagerService _connectionManager;
        private readonly ILogger<MessagingHub> _logger;
        private readonly IMessagingService _messagingService;
        public MessagingHub(IMessagingConnectionManagerService connectionManagers, IMediator mediator, IMessagingCacheService messagingCacheService, ILogger<MessagingHub> logger, IMessagingService messagingService)
        {
            _connectionManager = connectionManagers;
            _mediator = mediator;
            _messagingCacheService = messagingCacheService;
            _logger = logger;
            _messagingService = messagingService;
        }

        public override async Task OnConnectedAsync()
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);

            _logger.LogInformation("User {UserId} connected to messaging hub", Context.UserIdentifier);

            var now = DateTime.UtcNow;
            await _connectionManager.UpsertConnectionAsync(
                int.Parse(Context.UserIdentifier),
                new Connection(Context.UserIdentifier, Context.ConnectionId, -1)); // -1 means the user is online
            await Clients.All.UserOnlineAsync(int.Parse(Context.UserIdentifier));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);
            _logger.LogInformation("User {UserId} disconnected from messaging hub", Context.UserIdentifier);

            var now = DateTime.UtcNow;
            await _connectionManager.UpsertConnectionAsync(
                int.Parse(Context.UserIdentifier),
                new Connection(Context.UserIdentifier, Context.ConnectionId, ((DateTimeOffset)now).ToUnixTimeSeconds()));
            await Clients.All.UserDisconnectedAsync(new OnlineStatusDto
            {
                UserId = int.Parse(Context.UserIdentifier),
                LastActive = ((DateTimeOffset)now).ToUnixTimeSeconds()
            });
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<OnlineStatusDto>> GetOnlineUsers()
        {
            var onlineUsers = await _connectionManager.GetAllConnectionAsync(includeSelf: true, userId: int.Parse(Context.UserIdentifier));
            return [.. onlineUsers.Select(u => new OnlineStatusDto
            {
                UserId = int.Parse(u.UserId),
                LastActive = u.LastActive
            })];
        }

        // Add a user to a conversation
        public async Task AddToConversation(int conversationId, int userId)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);
            _logger.LogInformation("User {UserId} added to conversation {ConversationId}", Context.UserIdentifier, conversationId);
            var groupSystemMessageDto = new GroupSystemMessageDto
            {
                MessageType = GroupSystemMessageType.PersonJoined,
                ConversationId = conversationId.ToString()
            };

            await Clients.Group(conversationId.ToString())
                .JoinedConversationGroupAsync(groupSystemMessageDto, int.Parse(Context.UserIdentifier));
        }

        public async Task RemoveFromConversation(int conversationId)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);
            _logger.LogInformation("User {UserId} removed from conversation {ConversationId}", Context.UserIdentifier, conversationId);
            await _connectionManager.RemoveByKeyAsync(int.Parse(Context.UserIdentifier));

            var groupSystemMessageDto = new GroupSystemMessageDto
            {
                MessageType = GroupSystemMessageType.PersonLeft,
                ConversationId = conversationId.ToString()
            };

            await Clients.Group(conversationId.ToString())
                .RemovedFromConversationAsync(groupSystemMessageDto, int.Parse(Context.UserIdentifier));
        }

        public async Task CreateAndJoinConversation(CreateConversationCommand command)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);

            // Get the current date and time in UTC, make sure the persistent and cache storage are in sync
            DateTime now = DateTime.UtcNow;

            var createdConversationId = await _mediator.Send(command);
            foreach (var member in command.Members)
            {
                _logger.LogInformation("Adding user {UserId} to conversation {ConversationId}", member.Key, createdConversationId);
            }

            var newConversationMetadata = new ConversationMetaDataDto
            {
                Id = createdConversationId.ToString(),
                Members = command.Members.Select(m => new ConversationMemberDto { UserId = m.Key, UserName = m.Value }).ToArray(),
                CreatorId = command.Members.First().Key,
                NameOne = command.ConversationNameOne,
                NameTwo = command.ConversationNameTwo,
                AvatarOne = command.AvatarOne,
                AvatarTwo = command.AvatarTwo,
                LastMessageSenderName = command.ConversationNameOne,
                LastMessage = string.Empty,
                Timestamp = now,
            };
            await _messagingCacheService.UpsertConversationAsync(newConversationMetadata);
        }

        public async Task<ChatHistoryDto?> SendMessage(MessageDto message)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);

            // Get the current date and time in UTC, make sure the persistent and cache storage are in sync
            DateTime now = DateTime.UtcNow;

            message.MessageId = await _mediator.Send(new CreateMessageCommand
            {
                Content = message.Content,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                SentDate = now
            });

            var newMessage = new ChatHistoryDto
            {
                MessageId = message.MessageId.ToString(),
                Content = message.Content,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                SenderName = message.SenderName,
                SenderAvatar = message.SenderAvatar,
                Timestamp = now,
            };

            await _messagingCacheService.UpsertMessagesAsync(newMessage);

            var conversation = await _messagingCacheService.GetConversationByIdAsync(message.ConversationId);
            if (conversation == null)
            {
                _logger.LogWarning("Conversation not found for ConversationId: {ConversationId}", message.ConversationId);
                return null;
            }

            var tasks = new List<Task>();
            foreach (var member in conversation.Members)
            {
                if (member.UserId == message.SenderId)
                {
                    continue;
                }
                tasks.Add(Task.Run(async () =>
                {
                    await _messagingService.SendMessageToConversationAsync(
                        message.ConversationId,
                        member.UserId,
                        newMessage,
                        Clients.User(member.UserId.ToString())
                    );
                }));
            }
            await Task.WhenAll(tasks);

            return newMessage;
        }

        public async Task SendMessageRead(MessageStatusDto message)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);

            await Clients.Group(message.ConversationId.ToString())
                .ReceiveMessageReadStatusAsync(message);
        }

        public async Task SendTypingIndicator(int conversationId, bool isTyping, string typerName)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);
            if (isTyping)
            {
                _logger.LogInformation("User {UserId} is typing in conversation {ConversationId}", typerName, conversationId);
            }
            else
            {
                _logger.LogInformation("User {UserId} stopped typing in conversation {ConversationId}", typerName, conversationId);
            }
            await Clients.Group(conversationId.ToString())
                .ReceiveTypingIndicatorAsync(isTyping, typerName);
        }

        // When user leaves or switches to another conversation
        public async Task OnConversationCheckin(int conversationId)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);
            _logger.LogInformation("User {UserId} checked in to conversation {ConversationId}", Context.UserIdentifier, conversationId);
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
            _logger.LogInformation("User {UserId} added to SignalR group for conversation {ConversationId}", Context.UserIdentifier, conversationId);
        }

        // When user leaves or switches to another conversation
        public async Task OnConversationCheckout(int conversationId)
        {
            ArgumentException.ThrowIfNullOrEmpty(Context.UserIdentifier);
            _logger.LogInformation("User {UserId} checked out of conversation {ConversationId}", Context.UserIdentifier, conversationId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
            _logger.LogInformation("User {UserId} removed from SignalR group for conversation {ConversationId}", Context.UserIdentifier, conversationId);
        }
    }
}
