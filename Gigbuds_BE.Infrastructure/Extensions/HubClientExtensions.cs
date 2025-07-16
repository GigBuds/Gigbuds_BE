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
            SetNotificationTypeAndTitleForEmployer(methodName, notification);

            switch (methodName)
            {
                case nameof(INotificationForEmployers.NotifyNewJobApplicationReceived):
                    await clients.NotifyNewJobApplicationReceived(notification);
                    break;
                case nameof(INotificationForEmployers.NotifyNewFeedbackReceived): // TODO: set new feedback received after employer click 'view'
                    await clients.NotifyNewFeedbackReceived(notification);
                    break;
                case nameof(INotificationForEmployers.NotifyNewFollower):
                    await clients.NotifyNewFollower(notification);
                    break;
                case nameof(INotificationForEmployers.NotifyJobPostExpired): // TODO: set job post expired after employer click 'view'
                    await clients.NotifyJobPostExpired(notification);
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
                    await clients.NotifyNewPostFromFollowedEmployer(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackReceived): // TODO: set job feedback received after employer click 'send'
                    await clients.NotifyJobFeedbackReceived(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackSent): // TODO: set job feedback sent after job seeker click 'send'
                    await clients.NotifyJobFeedbackSent(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationAccepted):
                    await clients.NotifyJobApplicationAccepted(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRejected):
                    await clients.NotifyJobApplicationRejected(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRemovedFromApproved):
                    await clients.NotifyJobApplicationRemovedFromApproved(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobCompleted): 
                    await clients.NotifyJobCompleted(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyNewJobPostMatching):
                    await clients.NotifyNewJobPostMatching(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyProfileViewedByEmployer): // TODO: set profile viewed by employer after employer click 'view'
                    await clients.NotifyProfileViewedByEmployer(notification);
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
                default:
                    throw new NotImplementedException($"Notification method {methodName} not implemented for job seeker.");
            }
        }

        public static void SetNotificationTypeAndTitleForEmployer(string methodName, NotificationDto notification)
        {
            switch (methodName)
            {
                case nameof(INotificationForEmployers.NotifyNewFollower):
                    notification.Type = NotificationType.profile.ToString();
                    notification.Title = "Bạn có người theo dõi mới";
                    break;
                case nameof(INotificationForEmployers.NotifyNewJobApplicationReceived):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển mới";
                    break;
                case nameof(INotificationForEmployers.NotifyNewFeedbackReceived):
                    notification.Type = NotificationType.feedback.ToString();
                    notification.Title = "Nhận phản hồi mới về công việc";
                    break;
                case nameof(INotificationForEmployers.NotifyJobPostExpired):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Bài đăng đã hết hạn";
                    break;
                default:
                    throw new NotImplementedException($"Notification method {methodName} not implemented for employer.");
            }
        }

        internal static async Task NotifyUser(
            this INotificationForUser clients,
            string methodName,
            NotificationDto notification
        )
        {
            SetNotificationTypeAndTitleForUser(methodName, notification);

            switch (methodName)
            {
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    await clients.NotifyMembershipExpired(notification);
                    break;
                case nameof(INotificationForUser.NotifyNewMessageReceived):
                    await clients.NotifyNewMessageReceived(notification);
                    break;
                default:
                    throw new NotImplementedException($"Notification method {methodName} not implemented for user.");
            }
        }

        public static void SetNotificationTypeAndTitleForUser(string methodName, NotificationDto notification)
        {
            switch (methodName)
            {
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    notification.Type = NotificationType.membership.ToString();
                    notification.Title = "Gói thành viên đã hết hạn";
                    break;
                default:
                    break;
            }
        }
    }
}
