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

        public Task ClearAllNotificationsAsync(string userId)
        {
            return _db.KeyDeleteAsync(userId);
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync(string userId)
        {
            var notifications = await _db.SetMembersAsync(userId);
            return notifications.Select(n => JsonSerializer.Deserialize<NotificationDto>(n!)!).ToList();
        }

        public async Task SaveNotificationAsync(string userId, NotificationDto notificationDto)
        {
            await _db.SetAddAsync(userId, JsonSerializer.Serialize(notificationDto));
        }
    }
}
