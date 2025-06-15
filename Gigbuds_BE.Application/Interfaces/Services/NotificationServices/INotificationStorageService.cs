
using Gigbuds_BE.Application.DTOs.Notifications;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationStorageService
    {
        /// <summary>
        /// Saves a notification for a specific user, appending it to the end of the list
        /// </summary>
        /// <param name="userDevicesIds">The IDs of the devices to save the notification for</param>
        /// <param name="notificationDto">The notification data to save</param>
        /// <returns>True if the notification was saved successfully, false otherwise</returns>
        Task SaveNotificationAsync(List<string> userDevicesIds, NotificationDto notificationDto);

        /// <summary>
        /// Retrieves all notifications for a specific user
        /// </summary>
        /// <param name="deviceId">The ID of the device to get notifications for</param>
        /// <returns>A list of notifications for the specified user</returns>
        Task<List<NotificationDto>> GetNotificationsAsync(string deviceId);

        /// <summary>
        /// Removes all notifications for a specific user
        /// </summary>
        /// <param name="deviceId">The ID of the device to clear notifications for</param>
        Task ClearAllNotificationsAsync(string deviceId);
    }
}
