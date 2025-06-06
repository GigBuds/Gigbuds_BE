using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.SkillTags;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Commands.UpdateJobSeeker
{
    public class UpdateJobSeekerCommand : IRequest
    {
        public required int JobSeekerId { get; set; }
        public required string Location { get; set; }
        public List<SkillTagDto>? SkillTags { get; set; }
        public List<AccountExperienceTagDto>? AccountExperienceTags { get; set; }
        public List<EducationalLevelDto>? EducationalLevels { get; set; }
        public List<JobSeekerShiftsDto>? JobSeekerShifts { get; set; }
    }
}
