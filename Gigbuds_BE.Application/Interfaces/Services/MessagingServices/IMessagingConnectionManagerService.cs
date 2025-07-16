namespace Gigbuds_BE.Application.Interfaces.Services.MessagingServices
{
    public record Connection(string UserId, string ConnectionId, long LastActive);

    public interface IMessagingConnectionManagerService
    {
        Task<List<Connection>> GetAllConnectionAsync(bool includeSelf = false, int userId = 0);
        Task UpsertConnectionAsync(int userId, Connection connection);
        Task<Connection?> GetByKeyAsync(int userId);
        Task RemoveByKeyAsync(int userId);
        Task RemoveAllAsync();
    }
}
