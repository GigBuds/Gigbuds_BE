using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Domain.Entities.Notifications;

namespace Gigbuds_BE.Infrastructure.Extensions
{
    public static class HubClientExtensions
    {
        internal static async Task NotifyEmployer(
            this INotificationForUser clients,
            string methodName,
            NotificationDto notification
        )
        {
            SetNotificationTypeAndTitleForJobSeeker(methodName, notification);

            switch (methodName)
            {
                case nameof(INotificationForEmployers.NotifyNewJobApplicationReceived):
                    await clients.NotifyNewJobApplicationReceived();
                    break;
                case nameof(INotificationForEmployers.NotifyNewFeedbackReceived):
                    await clients.NotifyNewFeedbackReceived();
                    break;
                case nameof(INotificationForEmployers.NotifyNewFollower):
                    await clients.NotifyNewFollower(notification);
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
            NotificationDto notification
        )
        {
            SetNotificationTypeAndTitleForJobSeeker(methodName, notification);

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
                case nameof(INotificationForJobSeekers.NotifyJobCompleted): // TODO: set job complete after employer click 'finish'
                    await clients.NotifyJobCompleted();
                    break;
                case nameof(INotificationForJobSeekers.NotifyNewJobPostMatching):
                    await clients.NotifyNewJobPostMatching(notification);
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

        public static void SetNotificationTypeAndTitleForJobSeeker(string methodName, NotificationDto notification)
        {
            switch (methodName)
            {
                case nameof(INotificationForJobSeekers.NotifyNewPostFromFollowedEmployer):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Bài đăng mới từ nhà tuyển dụng đang theo dõi";
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackReceived):
                    notification.Type = NotificationType.feedback.ToString();
                    notification.Title = "Nhận phản hồi mới về công việc";
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackSent):
                    notification.Type = NotificationType.feedback.ToString();
                    notification.Title = "Đã gửi phản hồi về công việc";
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationAccepted):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển được chấp nhận";
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRejected):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển bị từ chối";
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRemovedFromApproved):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển bị hủy khỏi danh sách chấp nhận";
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobCompleted):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Công việc đã hoàn thành";
                    break;
                case nameof(INotificationForJobSeekers.NotifyNewJobPostMatching):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Công việc phù hợp mới";
                    break;
                case nameof(INotificationForJobSeekers.NotifyProfileViewedByEmployer):
                    notification.Type = NotificationType.profile.ToString();
                    notification.Title = "Hồ sơ được nhà tuyển dụng xem";
                    break;
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    notification.Type = NotificationType.membership.ToString();
                    notification.Title = "Gói thành viên đã hết hạn";
                    break;
            }
        }
    }
}
