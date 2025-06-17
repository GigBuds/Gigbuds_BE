
namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface IPushNotificationService
    {
        /// <summary>
        /// Sends a push notification via Firebase Cloud Messaging (FCM).
        /// </summary>
        /// <param name="deviceToken">The FCM device token.</param>
        /// <param name="title">Notification title.</param>
        /// <param name="body">Notification body.</param>
        /// <param name="data">Optional data payload.</param>
        /// <returns>True if the notification is sent successfully, false otherwise.</returns>
        Task<bool> SendPushNotificationAsync(List<string> userDeviceTokens, string title, string body, Dictionary<string, string>? data = null);
    }
}
