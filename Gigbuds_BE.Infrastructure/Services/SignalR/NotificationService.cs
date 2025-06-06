using System.Reflection;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Infrastructure.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Gigbuds_BE.Infrastructure.Services.SignalR
{
    internal class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationForUser> _hubContext;

        public NotificationService(IHubContext<NotificationHub, INotificationForUser> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyOneJobSeeker(MethodInfo method, object payload, string jobSeekerId)
        {
            await _hubContext.Clients.Group(jobSeekerId).NotifyJobSeeker(method.Name, payload);
        }

        public async Task NotifyOneEmployer(MethodInfo method, object payload, string employerId)
        {
            await _hubContext.Clients.User(employerId).NotifyEmployer(method.Name, payload);
        }

        public async Task NotifyAllJobSeekers(MethodInfo method, object payload)
        {
            await _hubContext.Clients.Group(UserRoles.JobSeeker).NotifyJobSeeker(method.Name, payload);
        }

        public async Task NotifyAllEmployers(MethodInfo method, object payload)
        {
            await _hubContext.Clients.Group(UserRoles.Employer).NotifyEmployer(method.Name, payload);
        }

        public async Task NotifyAllUsers(MethodInfo method, object payload)
        {
            await _hubContext.Clients.Group(UserRoles.JobSeeker).NotifyJobSeeker(method.Name, payload);
            await _hubContext.Clients.Group(UserRoles.Employer).NotifyEmployer(method.Name, payload);
        }
    }
}
