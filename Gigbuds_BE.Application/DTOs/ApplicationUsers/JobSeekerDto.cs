using Gigbuds_BE.Application.DTOs.SkillTags;

namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class JobSeekerDto
    {
        public int Id { get; set; }
        public DateOnly Dob { get; set; }
        public bool IsMale { get; set; }
        public bool IsEnabled { get; set; }
        public List<SkillTagDto> SkillTags { get; set; } = [];
        public List<EducationalLevelDto> EducationalLevels { get; set; } = [];
        public List<AccountExperienceTagDto> accountExperienceTags { get; set; } = [];
        public List<JobSeekerShiftsDto> JobSeekerShifts { get; set; } = [];

    }
}