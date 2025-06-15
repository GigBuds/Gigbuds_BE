using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Microsoft.AspNetCore.SignalR;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    public class NotificationHub : Hub<INotificationForUser>
    {
        private readonly IConnectionManager _connectionManager;
        public NotificationHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
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
