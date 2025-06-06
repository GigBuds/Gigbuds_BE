using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    public class NotificationHub : Hub<INotificationForUser>
    {
        // ==============================
        // === Methods
        // ==============================

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user == null) return;

            var userRole = user.FindFirst(ClaimTypes.Role)!.Value;
            if (userRole == null) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, userRole);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Context.User;
            if (user == null) return;

            var userRole = user.FindFirst(ClaimTypes.Role)!.Value;
            if (userRole == null) return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userRole);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
