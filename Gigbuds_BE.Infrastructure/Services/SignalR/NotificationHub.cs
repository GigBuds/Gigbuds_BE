using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Features.Notifications.Queries.GetStoredNotifications;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    public class NotificationHub : Hub<INotificationForUser>
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IMediator _mediator;
        public NotificationHub(IConnectionManager connectionManager, IMediator mediator)
        {
            _connectionManager = connectionManager;
            _mediator = mediator;
        }

        // ==============================
        // === Methods
        // ==============================
        public override async Task OnConnectedAsync()
        {
            _connectionManager.AddConnection(Context.UserIdentifier, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _connectionManager.RemoveConnection(Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<NotificationDto>> GetStoredNotifications(string deviceId)
        {
            var result = await _mediator.Send(new GetStoredNotificationQuery { DeviceId = deviceId });
            return result;
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
