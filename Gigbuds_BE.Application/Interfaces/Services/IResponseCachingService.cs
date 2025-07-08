namespace Gigbuds_BE.Application.Interfaces.Services;

/// <summary>
/// Defines the contract for a response caching service.
/// This service handles storing and retrieving responses from a distributed cache like Redis.
/// </summary>
public interface IResponseCachingService
{
    /// <summary>
    /// Caches a response with a specific key and a time-to-live (TTL).
    /// </summary>
    /// <param name="cacheKey">The unique key to identify the cached item.</param>
    /// <param name="response">The response object to cache. This object will be serialized.</param>
    /// <param name="timeToLive">The duration for which the cache entry will be valid.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

    /// <summary>
    /// Retrieves a cached response by its key.
    /// </summary>
    /// <param name="cacheKey">The key of the cached item to retrieve.</param>
    /// <returns>The cached response as a string (e.g., JSON), or null if the key is not found.</returns>
    Task<string?> GetCachedResponseAsync(string cacheKey);

    /// <summary>
    /// Removes a cached response by its key. Used for cache invalidation.
    /// </summary>
    /// <param name="cacheKey">The key of the cached item to remove.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveCacheResponseAsync(string cacheKey);
}