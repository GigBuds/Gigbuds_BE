using System.Collections.Concurrent;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{

    /// <summary>
    /// Data-type neutral connection manager interface.
    /// Implementations can use any underlying data structure (Lists, Sets, Hashes, etc.)
    /// </summary>
    public interface IConnectionManager
    {
        /// <summary>
        /// Adds one or more connection values to the specified key.
        /// Implementation can choose data structure (List for order, Set for uniqueness, etc.)
        /// </summary>
        Task AddConnectionAsync(string keyId, params string[] valueIds);

        /// <summary>
        /// Removes all connections associated with the specified key.
        /// </summary>
        Task RemoveConnectionAsync(string keyId);

        /// <summary>
        /// Removes specific connection values from the specified key.
        /// </summary>
        Task RemoveConnectionAsync(string keyId, params string[] valueIds);

        /// <summary>
        /// Gets a single connection for the specified key.
        /// Implementation decides which one to return (first, random, etc.)
        /// </summary>
        Task<string?> GetConnectionAsync(string keyId);

        /// <summary>
        /// Gets all connections for the specified key.
        /// Returns empty list if key doesn't exist.
        /// </summary>
        Task<List<string>> GetConnectionsAsync(string keyId);

        /// <summary>
        /// Gets all connections for all keys.
        /// Key = keyId, Value = list of connections for that key.
        /// </summary>
        Task<Dictionary<string, List<string>>> GetAllConnectionsAsync();

        /// <summary>
        /// Checks if a specific connection exists for the given key.
        /// </summary>
        Task<bool> ConnectionExistsAsync(string keyId, string valueId);

        /// <summary>
        /// Gets the count of connections for the specified key.
        /// </summary>
        Task<int> GetConnectionCountAsync(string keyId);
    }
}