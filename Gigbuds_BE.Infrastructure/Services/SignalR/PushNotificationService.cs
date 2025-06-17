using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Expo.Server.Client;
using Expo.Server.Models;
using Gigbuds_BE.Infrastructure.Services.Firebase;
using Microsoft.Extensions.Logging;


namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    internal class PushNotificationService : IPushNotificationService
    {
        private readonly ILogger<PushNotificationService> _logger;
        private readonly IFirebaseService _firebaseService;

        public PushNotificationService(ILogger<PushNotificationService> logger, IFirebaseService firebaseService)
        {
            _logger = logger;
            _firebaseService = firebaseService;
        }

        public async Task<bool> SendPushNotificationAsync(List<string> userDeviceTokens, string title, string body, Dictionary<string, string>? data = null)
        {
            List<Task> tasks = new List<Task>();
            foreach (var deviceToken in userDeviceTokens)
            {

                try
                {
                    var message = new Message
                    {
                        Token = deviceToken,
                        Notification = new Notification
                        {
                            Title = title,
                            Body = body
                        },
                        Data = data ?? new Dictionary<string, string>() // custom payload
                    };

                    tasks.Add(FirebaseMessaging.GetMessaging(_firebaseService.GetApp()).SendAsync(message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[FCM] Failed to send push notification to token: {DeviceToken}", deviceToken);
                }
            }

            await Task.WhenAll(tasks);

            return true;
        }
    }
}
