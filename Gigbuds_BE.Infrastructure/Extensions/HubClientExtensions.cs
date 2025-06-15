using Gigbuds_BE.Application.DTOs.Notifications;
using Gigbuds_BE.Application.Interfaces.Services.NotificationServices;
using Gigbuds_BE.Domain.Entities.Notifications;

namespace Gigbuds_BE.Infrastructure.Extensions
{
    public static class HubClientExtensions
    {
        public static async Task NotifyEmployer(
            this INotificationForUser clients,
            string methodName,
            NotificationDto notification
        )
        {
            switch (methodName)
            {
                case nameof(INotificationForEmployers.NotifyNewJobApplicationReceived):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Nhận đơn ứng tuyển mới";
                    await clients.NotifyNewJobApplicationReceived();
                    break;
                case nameof(INotificationForEmployers.NotifyNewFeedbackReceived):
                    notification.Type = NotificationType.feedback.ToString();
                    notification.Title = "Nhận phản hồi mới";
                    await clients.NotifyNewFeedbackReceived();
                    break;
                case nameof(INotificationForEmployers.NotifyNewFollower):
                    notification.Type = NotificationType.follower.ToString();
                    notification.Title = "Có người theo dõi mới";
                    await clients.NotifyNewFollower();
                    break;
                case nameof(INotificationForEmployers.NotifyJobPostExpired):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Bài đăng đã hết hạn";
                    await clients.NotifyJobPostExpired();
                    break;
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    notification.Type = NotificationType.membership.ToString();
                    notification.Title = "Gói thành viên đã hết hạn";
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
            switch (methodName)
            {
                case nameof(INotificationForJobSeekers.NotifyNewPostFromFollowedEmployer):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Bài đăng mới từ nhà tuyển dụng đang theo dõi";
                    await clients.NotifyNewPostFromFollowedEmployer();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackReceived):
                    notification.Type = NotificationType.feedback.ToString();
                    notification.Title = "Nhận phản hồi mới về công việc";
                    await clients.NotifyJobFeedbackReceived();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobFeedbackSent):
                    notification.Type = NotificationType.feedback.ToString();
                    notification.Title = "Đã gửi phản hồi về công việc";
                    await clients.NotifyJobFeedbackSent();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationAccepted):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển được chấp nhận";
                    await clients.NotifyJobApplicationAccepted();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRejected):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển bị từ chối";
                    await clients.NotifyJobApplicationRejected();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobApplicationRemovedFromApproved):
                    notification.Type = NotificationType.application.ToString();
                    notification.Title = "Đơn ứng tuyển bị hủy khỏi danh sách chấp nhận";
                    await clients.NotifyJobApplicationRemovedFromApproved();
                    break;
                case nameof(INotificationForJobSeekers.NotifyJobCompleted):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Công việc đã hoàn thành";
                    await clients.NotifyJobCompleted();
                    break;
                case nameof(INotificationForJobSeekers.NotifyNewJobPostMatching):
                    notification.Type = NotificationType.job.ToString();
                    notification.Title = "Công việc phù hợp mới";
                    await clients.NotifyNewJobPostMatching(notification);
                    break;
                case nameof(INotificationForJobSeekers.NotifyProfileViewedByEmployer):
                    notification.Type = NotificationType.profile.ToString();
                    notification.Title = "Hồ sơ được nhà tuyển dụng xem";
                    await clients.NotifyProfileViewedByEmployer();
                    break;
                case nameof(INotificationForUser.NotifyMembershipExpired):
                    notification.Type = NotificationType.membership.ToString();
                    notification.Title = "Gói thành viên đã hết hạn";
                    await clients.NotifyMembershipExpired();
                    break;
                default:
                    throw new NotImplementedException($"Notification method {methodName} not implemented for job seeker.");
            }
        }
    }
}
