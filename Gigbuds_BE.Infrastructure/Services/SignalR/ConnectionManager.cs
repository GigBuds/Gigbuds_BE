
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using System.Collections.Concurrent;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    internal class ConnectionManager : IConnectionManager
    {
        private ConcurrentDictionary<string, string> _connections;

        public ConnectionManager()
        {
            _connections = new ConcurrentDictionary<string, string>();
        }

        public void AddConnection(string userId, string connectionId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(connectionId))
            {
                throw new ArgumentException("UserId and ConnectionId cannot be null or empty.");
            }
            _connections[userId] = connectionId;
        }

        public IReadOnlyDictionary<string, string> GetAllConnections()
        {
            return _connections.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public string? GetConnectionId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.");
            }
            _connections.TryGetValue(userId, out var connectionId);
            return connectionId;
        }

        public void RemoveConnection(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.");
            }
            _connections.TryRemove(userId, out _);
        }
    }
}
