using System.Text.Json;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class RedisNotificationStorageService : INotificationStorageService
    {
        private readonly IDatabase _db;
        private readonly NotificationSettings _settings;
        private readonly ILogger<RedisNotificationStorageService> _logger;

        public RedisNotificationStorageService(IConnectionMultiplexer redis, IOptions<NotificationSettings> settings, ILogger<RedisNotificationStorageService> logger)
        {
            _db = redis.GetDatabase(settings.Value.Storage.RedisDatabase);
            _settings = settings.Value;
            _logger = logger;
        }

        public Task ClearAllNotificationsAsync(string deviceId)
        {
            return _db.KeyDeleteAsync(deviceId);
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync(string deviceId)
        {
            var notifications = await _db.SetMembersAsync(deviceId);
            return notifications.Select(n => JsonSerializer.Deserialize<NotificationDto>(n!)!).ToList();
        }

        public async Task SaveNotificationAsync(List<string> userDevicesIds, NotificationDto notificationDto)
        {
            var tasks = new List<Task>();
            foreach (var deviceId in userDevicesIds)
            {
                tasks.Add(_db.SetAddAsync(deviceId, JsonSerializer.Serialize(notificationDto)));
            }
            await Task.WhenAll(tasks);
        }

        private string GetKey(int userId)
        {
            return $"{_settings.Storage.RedisKeyPrefix}:{userId}";
        }

    }
}
