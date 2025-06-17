using System.Reflection;
using Gigbuds_BE.Application.DTOs.Notifications;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task NotifyOneJobSeeker(MethodInfo method, string jobSeekerId, List<string> deviceTokens, NotificationDto notification);
        public Task NotifyOneEmployer(MethodInfo method, string employerId, List<string> deviceTokens, NotificationDto notification);
        public Task NotifyAllJobSeekers(MethodInfo method, NotificationDto notification);
        public Task NotifyAllEmployers(MethodInfo method, NotificationDto notification);
        public Task NotifyAllUsers(MethodInfo method, NotificationDto notification);
    }
}
