using Gigbuds_BE.Application.DTOs.Notifications;

namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationForJobSeekers
    {
        /// <summary>
        /// Notifies job seekers about new posts from employers they follow
        /// </summary>
        public Task NotifyNewPostFromFollowedEmployer();

        /// <summary>
        /// Notifies job seekers when they receive feedback on their job performance
        /// </summary>
        public Task NotifyJobFeedbackReceived();

        /// <summary>
        /// Notifies job seekers when their feedback has been sent to an employer
        /// </summary>
        public Task NotifyJobFeedbackSent();

        /// <summary>
        /// Notifies job seekers when their job application has been accepted
        /// </summary>
        public Task NotifyJobApplicationAccepted();

        /// <summary>
        /// Notifies job seekers when their job application has been rejected
        /// </summary>
        public Task NotifyJobApplicationRejected();

        /// <summary>
        /// Notifies job seekers when they are removed from approved applications
        /// </summary>
        public Task NotifyJobApplicationRemovedFromApproved();

        /// <summary>
        /// Notifies job seekers when their job has been marked as completed
        /// </summary>
        public Task NotifyJobCompleted();

        /// <summary>
        /// Notifies job seekers about new job posts that match their criteria
        /// </summary>
        /// <param name="notification">The notification message to be sent</param>
        public Task NotifyNewJobPostMatching(NotificationDto notification);

        /// <summary>
        /// Notifies job seekers when their profile has been viewed by an employer
        /// </summary>
        public Task NotifyProfileViewedByEmployer();
    }
}
