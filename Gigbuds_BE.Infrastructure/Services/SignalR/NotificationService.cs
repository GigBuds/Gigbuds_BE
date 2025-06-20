using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Application.Specifications.Notifications;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Infrastructure.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    internal class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationForUser> _hubContext;
        private readonly ILogger<NotificationService> _logger;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly INotificationStorageService _notificationStorageService;
        private readonly IConnectionManager _connectionManager;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(
            IHubContext<NotificationHub,
            INotificationForUser> hubContext,
            ILogger<NotificationService> logger,
            IPushNotificationService pushNotificationService,
            INotificationStorageService notificationStorageService,
            IConnectionManager connectionManager,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _logger = logger;
            _pushNotificationService = pushNotificationService;
            _notificationStorageService = notificationStorageService;
            _connectionManager = connectionManager;
        }

        public async Task NotifyOneUser(MethodInfo method, List<string> deviceTokens, string userId, NotificationDto notification)
        {
            if (IsClientConnected(userId))
            {
                await _hubContext.Clients.User(userId).NotifyUser(method.Name, notification);
                _logger.LogInformation("Notification sent to connected user {UserId}", userId);
            }
            else
            {
                _logger.LogInformation("User {UserId} is not connected, sending push notification", userId);

                // Send push notification if user has device tokens
                if (deviceTokens.Count > 0)
                {
                    try
                    {
                        await _pushNotificationService.SendPushNotificationAsync(
                            deviceTokens,
                            "GigBuds",
                            notification.Content,
                            notification.AdditionalPayload);

                        _logger.LogInformation("Push notification sent to user {UserId}", userId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending push notification to user {UserId}", userId);
                    }
                }

                // Store notification for when user comes back online
                try
                {
                    // Set notification type and title based on method name
                    HubClientExtensions.SetNotificationTypeAndTitleForUser(method.Name, notification);
                    await _notificationStorageService.SaveNotificationAsync(userId, notification);

                    _logger.LogInformation("Notification stored for offline user {UserId}", userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving notification to storage for user {UserId}", userId);
                }
            }
        }
        public async Task NotifyOneJobSeeker(MethodInfo method, List<string> deviceTokens, string jobSeekerId, NotificationDto notification)
        {
            if (IsClientConnected(jobSeekerId))
            {
                await _hubContext.Clients.User(jobSeekerId).NotifyJobSeeker(method.Name, notification);
            }
            else
            {
                _logger.LogInformation("Client {JobSeekerId} is not connected, sending push notification", jobSeekerId);
                if (deviceTokens.Count > 0)
                {
                    try
                    {
                        await _pushNotificationService.SendPushNotificationAsync(deviceTokens, "GigBuds", notification.Content, notification.AdditionalPayload);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending push notification to user {JobSeekerId}", jobSeekerId);
                    }
                }

                try
                {
                    HubClientExtensions.SetNotificationTypeAndTitleForJobSeeker(
                        method.Name, notification);
                    await _notificationStorageService.SaveNotificationAsync(jobSeekerId, notification);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving notification to database for user {JobSeekerId}", jobSeekerId);
                }

                _logger.LogInformation("No device token found for user {JobSeekerId}, storing notification", jobSeekerId);
            }
        }

        public async Task NotifyOneEmployer(MethodInfo method, string employerId, NotificationDto notification)
        {
            var deviceTokens = await GetUserDeviceTokensAsync(employerId);

            if (IsClientConnected(employerId))
            {
                await _hubContext.Clients.User(employerId).NotifyEmployer(method.Name, notification);
            }
            else
            {
                _logger.LogInformation("Client {EmployerId} is not connected, sending push notification", employerId);
                if (deviceTokens.Count > 0)
                {
                    try
                    {
                        await _pushNotificationService.SendPushNotificationAsync(deviceTokens, "GigBuds", notification.Content, notification.AdditionalPayload);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending push notification to user {EmployerId}", employerId);
                    }
                }

                try
                {
                    HubClientExtensions.SetNotificationTypeAndTitleForEmployer(
                        method.Name, notification);
                    await _notificationStorageService.SaveNotificationAsync(employerId, notification);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving notification to database for user {EmployerId}", employerId);
                }
                _logger.LogInformation("No device token found for user {EmployerId}, storing notification", employerId);
            }
        }

        public async Task NotifyAllJobSeekers(MethodInfo method, NotificationDto notification)
        {
            await _hubContext.Clients.Group(UserRoles.JobSeeker).NotifyJobSeeker(method.Name, notification);
        }

        public async Task NotifyAllEmployers(MethodInfo method, NotificationDto notification)
        {
            await _hubContext.Clients.Group(UserRoles.Employer).NotifyEmployer(method.Name, notification);
        }

        public async Task NotifyAllUsers(MethodInfo method, NotificationDto notification)
        {
            await _hubContext.Clients.Group(UserRoles.JobSeeker).NotifyJobSeeker(method.Name, notification);
            await _hubContext.Clients.Group(UserRoles.Employer).NotifyEmployer(method.Name, notification);
        }


        public bool IsClientConnected(string userId)
        {
            return _connectionManager.GetConnectionId(userId) != null;
        }

        private async Task<List<string>> GetUserDeviceTokensAsync(string userId)
        {
            var userDevices = await _unitOfWork.Repository<DevicePushNotifications>()
    .GetAllWithSpecificationAsync(new GetDevicesByUserSpecification(int.Parse(userId)));
            return userDevices.Select(ud => ud.DeviceToken).ToList();
        }

    }
}
