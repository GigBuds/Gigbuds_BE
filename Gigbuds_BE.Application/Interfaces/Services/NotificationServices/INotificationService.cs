using System.Reflection;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationService
    {
        public Task NotifyOneJobSeeker(MethodInfo method, object payload, string jobSeekerId);
        public Task NotifyOneEmployer(MethodInfo method, object payload, string employerId);
        public Task NotifyAllJobSeekers(MethodInfo method, object payload);
        public Task NotifyAllEmployers(MethodInfo method, object payload);
        public Task NotifyAllUsers(MethodInfo method, object payload);
    }
}
