using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.SkillTags;
using MediatR;

namespace Gigbuds_BE.Application.Features.Embedding.JobSeekerEmbedding
{
    public class JobSeekerEmbeddingRequest : IRequest
    {
        public int Id { get; init; }
        public DateOnly Dob { get; init; }
        public bool IsMale { get; init; }
        public bool IsEnabled { get; init; }
        public string Location { get; init; } = string.Empty;
        public List<SkillTagDto>? SkillTags { get; init; } = [];
        public List<EducationalLevelDto>? EducationalLevels { get; init; } = [];
        public List<AccountExperienceTagDto>? AccountExperienceTags { get; init; } = [];
    }
}
