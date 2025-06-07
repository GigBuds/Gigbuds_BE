using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;

namespace Gigbuds_BE.Infrastructure.Extensions
{
    public static class HubClientExtensions
    {
        public static async Task NotifyEmployer(
            this INotificationForUser clients,
            string methodName,
            object payload,
            object? additionalPayload = null
        )
        {
            switch (methodName)
            {
                case nameof(INotificationForEmployers.NotifyNewJobPostSuggestion):
                    await clients.NotifyNewJobPostSuggestion();
                    break;
                case nameof(INotificationForEmployers.NotifyNewJobApplicationReceived):
                    await clients.NotifyNewJobApplicationReceived();
                    break;
                case nameof(INotificationForEmployers.NotifyNewFeedbackReceived):
                    await clients.NotifyNewFeedbackReceived();
                    break;
                case nameof(INotificationForEmployers.NotifyNewFollower):
                    await clients.NotifyNewFollower();
                    break;
                case nameof(INotificationForEmployers.NotifyJobPostExpired):
                    await clients.NotifyJobPostExpired();
                    break;
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    await clients.NotifyMembershipExpired();
                    break;
                default:
                    throw new NotImplementedException($"Notification method {methodName} not implemented for employer.");
            }
        }

        public static async Task NotifyJobSeeker(
            this INotificationForUser clients,
            string methodName,
            object payload,
            object? additionalPayload = null
        )
        {
            switch (methodName)
            {
                case nameof(INotificationForJobSeekers.NotifyNewPostFromFollowedEmployer):
                    await clients.NotifyNewPostFromFollowedEmployer();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackReceived):
                    await clients.NotifyJobFeedbackReceived();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackSent):
                    await clients.NotifyJobFeedbackSent();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationAccepted):
                    await clients.NotifyJobApplicationAccepted();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRejected):
                    await clients.NotifyJobApplicationRejected();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRemovedFromApproved):
                    await clients.NotifyJobApplicationRemovedFromApproved();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobCompleted):
                    await clients.NotifyJobCompleted();
                    break;
                case nameof(INotificationForJobSeekers.NotifyNewJobPostMatching):
                    await clients.NotifyNewJobPostMatching(payload as string, additionalPayload);
                    break;
                case nameof(INotificationForJobSeekers.NotifyProfileViewedByEmployer):
                    await clients.NotifyProfileViewedByEmployer();
                    break;
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    await clients.NotifyMembershipExpired();
                    break;
                default:
                    throw new NotImplementedException($"Notification method {methodName} not implemented for job seeker.");
            }
        }
    }
}
