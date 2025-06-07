using System.Reflection;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task NotifyOneJobSeeker(MethodInfo method, string jobSeekerId, object payload, object? additionalPayload = null);
        public Task NotifyOneEmployer(MethodInfo method,  string employerId, object payload, object? additionalPayload = null);
        public Task NotifyAllJobSeekers(MethodInfo method, object payload, object? additionalPayload = null);
        public Task NotifyAllEmployers(MethodInfo method, object payload, object? additionalPayload = null);
        public Task NotifyAllUsers(MethodInfo method, object payload, object? additionalPayload = null);
    }
}
