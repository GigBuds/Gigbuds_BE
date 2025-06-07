namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface IConnectionManager
    {
        void AddConnection(string userId, string connectionId);
        void RemoveConnection(string userId);
        string? GetConnectionId(string userId);
        IReadOnlyDictionary<string, string> GetAllConnections();
    }
}