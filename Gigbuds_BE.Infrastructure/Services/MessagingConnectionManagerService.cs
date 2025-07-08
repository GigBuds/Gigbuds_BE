using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.Interfaces.Services.MessagingServices;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class MessagingConnectionManagerService : IMessagingConnectionManagerService
    {
        private readonly IDatabase _database;
        private readonly IOptions<MessagingSettings> _settings;
        private readonly JsonSerializerOptions _jsonOptions;

        public MessagingConnectionManagerService(IConnectionMultiplexer redis, IOptions<MessagingSettings> settings)
        {
            _database = redis.GetDatabase(settings.Value.Storage.RedisDatabase);
            _settings = settings;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task UpsertConnectionAsync(int userId, Connection connection)
        {
            ArgumentNullException.ThrowIfNull(connection);

            var key = GenerateConnectionKey(userId);
            var jsonValue = JsonSerializer.Serialize(connection, _jsonOptions);

            await _database.StringSetAsync(key, jsonValue);
        }

        public async Task<List<Connection>> GetAllConnectionAsync(bool includeSelf = false, int userId = 0)
        {
            var connections = new List<Connection>();

            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys(
                _settings.Value.Storage.RedisDatabase,
                pattern: $"{_settings.Value.Storage.ConnectionKeyName}:*").ToList();

            foreach (var key in keys)
            {
                if (includeSelf && userId != 0 && key.ToString().Contains(userId.ToString()))
                {
                    continue;
                }
                var jsonValue = await _database.StringGetAsync(key);
                if (jsonValue.HasValue)
                {
                    try
                    {
                        var connection = JsonSerializer.Deserialize<Connection>(jsonValue!, _jsonOptions);
                        if (connection != null)
                        {
                            connections.Add(connection);
                        }
                    }
                    catch (JsonException)
                    {
                        // Skip invalid JSON entries
                        continue;
                    }
                }
            }

            return connections;

        }

        public async Task<Connection?> GetByKeyAsync(int userId)
        {
            var pattern = $"{_settings.Value.Storage.ConnectionKeyName}:{userId}:*";
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys(pattern: pattern).ToList();

            if (keys.Count == 0)
                return null;

            // Return the first matching connection for the user
            var jsonValue = await _database.StringGetAsync(keys[0]);

            if (!jsonValue.HasValue)
                return null;

            return JsonSerializer.Deserialize<Connection>(jsonValue!, _jsonOptions);
        }

        public async Task RemoveByKeyAsync(int userId)
        {
            var pattern = $"{_settings.Value.Storage.ConnectionKeyName}:{userId}:*";
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys(pattern: pattern).ToList();

            if (keys.Count > 0)
            {
                await _database.KeyDeleteAsync(keys.ToArray());
            }
        }

        public async Task RemoveAllAsync()
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys(pattern: $"{_settings.Value.Storage.ConnectionKeyName}:*").ToList();

            if (keys.Count > 0)
            {
                await _database.KeyDeleteAsync(keys.ToArray());
            }
        }

        private string GenerateConnectionKey(int userId)
        {
            return $"{_settings.Value.Storage.ConnectionKeyName}:{userId}";
        }
    }
}
