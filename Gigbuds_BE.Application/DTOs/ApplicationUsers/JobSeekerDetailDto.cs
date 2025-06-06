using Gigbuds_BE.Application.DTOs.Feedbacks;
using Gigbuds_BE.Application.DTOs.SkillTags;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class JobSeekerDetailDto
    {
        // Personal Information
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly Dob { get; set; }
        public bool IsMale { get; set; }
        public bool IsEnabled { get; set; }
        public string CurrentLocation { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Professional Information
        public List<SkillTagDto> SkillTags { get; set; } = [];
        public List<EducationalLevelDto> EducationalLevels { get; set; } = [];
        public List<AccountExperienceTagDto> AccountExperienceTags { get; set; } = [];

        // Schedule Information
        public List<JobSeekerShiftsDtoMapping> JobSeekerShifts { get; set; } = [];

        // Feedback Information
        public List<FeedbackDto> Feedbacks { get; set; } = [];
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }

        // Follower Information
        public int FollowerCount { get; set; }
    }
} 