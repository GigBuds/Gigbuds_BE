namespace Gigbuds_BE.Application.Interfaces.Services.NotificationServices
{
    public interface INotificationForEmployers 
    {
        /// <summary>
        /// Sends a notification to employers about new job post suggestions
        /// </summary>
        public Task NotifyNewJobPostSuggestion();

        /// <summary>
        /// Sends a notification to employers when they receive a new job application
        /// </summary>
        public Task NotifyNewJobApplicationReceived();

        /// <summary>
        /// Sends a notification to employers when they receive new feedback
        /// </summary>
        public Task NotifyNewFeedbackReceived();

        /// <summary>
        /// Sends a notification to employers when they gain a new follower
        /// </summary>
        public Task NotifyNewFollower();

        /// <summary>
        /// Sends a notification to employers when their job post has expired
        /// </summary>
        public Task NotifyJobPostExpired();
    }
}
