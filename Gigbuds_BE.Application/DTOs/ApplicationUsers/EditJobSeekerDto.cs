using Gigbuds_BE.Application.DTOs.SkillTags;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class EditJobSeekerDto
    {
        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
        public DateOnly Dob { get; set; }
        public bool IsMale { get; set; }

        // Professional Information
        public List<SkillTagDto> SkillTags { get; set; } = [];
        public List<EducationalLevelDto> EducationalLevels { get; set; } = [];
        public List<AccountExperienceTagDto> AccountExperienceTags { get; set; } = [];
    }
} 