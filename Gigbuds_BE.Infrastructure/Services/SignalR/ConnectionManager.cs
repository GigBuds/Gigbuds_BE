using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using System.Collections.Concurrent;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<string, string> _connections = new();

        public void AddConnection(string userId, string connectionId)
            => _connections[userId] = connectionId;

        public void RemoveConnection(string userId)
            => _connections.TryRemove(userId, out _);

        public string? GetConnectionId(string userId)
            => _connections.TryGetValue(userId, out var connId) ? connId : null;

        public IReadOnlyDictionary<string, string> GetAllConnections()
            => _connections;
    }
}