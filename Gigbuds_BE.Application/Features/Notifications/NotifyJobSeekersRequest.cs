using MediatR;

namespace Gigbuds_BE.Application.Features.Notifications
{
    internal class NotifyJobSeekersRequest : INotification
    {
        // Data for filtering job seekers by location
        public required int JobPostId { get; init; }
        public required string JobPostLocation { get; init; }
        public required int MinAgeRequirement { get; init; }
        public required string JobTitle { get; init; }
        public required string JobDescription { get; init; }
        public required string JobRequirement { get; init; }
        public required string ExperienceRequirement { get; init; }
        public required bool IsMaleRequired { get; init; }

        // Data for notification template
        public required int EmployerId { get; init; }
        public required DateOnly JobDeadline { get; init; }
        public required string JobCompany { get; init; }
    }

}
