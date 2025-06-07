using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.Specifications.SkillTags
{
    public class SkillTagByIdSpecification : BaseSpecification<SkillTag>
    {
        public SkillTagByIdSpecification(int skillTagId) 
            : base(st => st.Id == skillTagId && st.IsEnabled)
        {
        }
    }

    public class GetAllSkillTagsSpecification : BaseSpecification<SkillTag>
    {
        public GetAllSkillTagsSpecification()
            : base(st => st.IsEnabled)
        {
        }
    }
} 