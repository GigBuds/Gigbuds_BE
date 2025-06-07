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

        public async Task NotifyOneJobSeeker(MethodInfo method, string jobSeekerId, object payload, object? additionalPayload = null)
        {
            await _hubContext.Clients.User(jobSeekerId).NotifyJobSeeker(method.Name, payload, additionalPayload);
        }

        public async Task NotifyOneEmployer(MethodInfo method, string employerId, object payload, object? additionalPayload = null)
        {
            await _hubContext.Clients.User(employerId).NotifyEmployer(method.Name, payload, additionalPayload);
        }

        public async Task NotifyAllJobSeekers(MethodInfo method, object payload, object? additionalPayload = null)
        {
            await _hubContext.Clients.Group(UserRoles.JobSeeker).NotifyJobSeeker(method.Name, payload, additionalPayload);
        }

        public async Task NotifyAllEmployers(MethodInfo method, object payload, object? additionalPayload = null)
        {
            await _hubContext.Clients.Group(UserRoles.Employer).NotifyEmployer(method.Name, payload, additionalPayload);
        }

        public async Task NotifyAllUsers(MethodInfo method, object payload, object? additionalPayload = null)
        {
            await _hubContext.Clients.Group(UserRoles.JobSeeker).NotifyJobSeeker(method.Name, payload);
            await _hubContext.Clients.Group(UserRoles.Employer).NotifyEmployer(method.Name, payload);
        }
    }
}
