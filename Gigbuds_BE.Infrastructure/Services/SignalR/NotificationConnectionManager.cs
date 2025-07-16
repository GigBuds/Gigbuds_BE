using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    internal class NotificationConnectionManager : IConnectionManager
    {
        private readonly ConcurrentDictionary<string, string> _connections;

        public NotificationConnectionManager()
        {
            _connections = new ConcurrentDictionary<string, string>();
        }

        public Task AddConnectionAsync(string keyId, params string[] valueIds)
        {
            if (string.IsNullOrEmpty(keyId) || valueIds.Length == 0)
            {
                throw new ArgumentException("KeyId and ValueIds cannot be null or empty.");
            }

            _connections.AddOrUpdate(keyId,
                valueIds[0],
                (key, existingValue) =>
                {
                    return valueIds[0];
                });

            return Task.CompletedTask;
        }

        public Task RemoveConnectionAsync(string keyId)
        {
            ArgumentNullException.ThrowIfNull(keyId);

            _connections.TryRemove(keyId, out _);
            return Task.CompletedTask;
        }

        public Task RemoveConnectionAsync(string keyId, params string[] valueIds)
        {
            if (string.IsNullOrEmpty(keyId) || valueIds.Length == 0)
            {
                throw new ArgumentException($"KeyId and ValueIds cannot be null or empty. {keyId}");
            }

            _connections.TryRemove(keyId, out _);

            return Task.CompletedTask;
        }

        public Task<string?> GetConnectionAsync(string keyId)
        {
            ArgumentNullException.ThrowIfNull(keyId);

            return Task.FromResult(_connections.TryGetValue(keyId, out var connections) ? connections : null);
        }

        public Task<List<string>> GetConnectionsAsync(string keyId)
        {
            ArgumentNullException.ThrowIfNull(keyId);

            return Task.FromResult(_connections.TryGetValue(keyId, out var connections) ? new List<string> { connections } : new List<string>());
        }

        public Task<Dictionary<string, List<string>>> GetAllConnectionsAsync()
        {
            return Task.FromResult(_connections.ToDictionary(kvp => kvp.Key, kvp => new List<string> { kvp.Value }));
        }

        public Task<bool> ConnectionExistsAsync(string keyId, string valueId)
        {
            ArgumentNullException.ThrowIfNull(keyId);
            ArgumentNullException.ThrowIfNull(valueId);

            return Task.FromResult(_connections.TryGetValue(keyId, out var connections) && connections == valueId);
        }

        public Task<int> GetConnectionCountAsync(string keyId)
        {
            if (string.IsNullOrEmpty(keyId))
            {
                throw new ArgumentException("KeyId cannot be null or empty.");
            }
            _connections.TryGetValue(keyId, out var connections);
            return Task.FromResult(connections != null ? 1 : 0);
        }
    }
}
