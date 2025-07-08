using Gigbuds_BE.Application.Interfaces.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Gigbuds_BE.Infrastructure.Services.Caching
{
    public class ResponseCachingService : IResponseCachingService
    {
        private readonly IDatabase _database;

        public ResponseCachingService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);

            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse.ToString();
        }

        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null)
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);

            await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task RemoveCacheResponseAsync(string cacheKey)
        {
            await _database.KeyDeleteAsync(cacheKey);
        }
    }
}
