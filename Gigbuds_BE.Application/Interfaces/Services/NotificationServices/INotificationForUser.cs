using Gigbuds_BE.Application.DTOs.Notifications;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationForUser : INotificationForJobSeekers, INotificationForEmployers
    {
        public Task NotifyMembershipExpired(NotificationDto notification);
    }
}
