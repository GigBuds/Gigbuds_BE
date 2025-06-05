using System.Text.Json.Serialization;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.SkillTags;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Commands.EditJobSeeker
{
    public class EditJobSeekerCommand : IRequest
    {
        [JsonIgnore]
        public int JobSeekerId { get; set; }
        
        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public bool IsMale { get; set; }

        // Professional Information
        public List<int> SkillTags { get; set; } = [];
        public List<EducationalLevelDto> EducationalLevels { get; set; } = [];
        public List<AccountExperienceTagDto> AccountExperienceTags { get; set; } = [];
    }
} 