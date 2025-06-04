using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Application.Specifications.ApplicationUsers
{
    public class JobSeekerByIdSpecification : BaseSpecification<ApplicationUser>
    {
        public JobSeekerByIdSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.SkillTags);
            AddInclude(x => x.EducationalLevels);
            AddInclude(x => x.AccountMemberships);
            AddInclude(x => x.AccountMemberships.Select(am => am.Membership));
        }
    }

    public class EmployerByIdSpecification : BaseSpecification<ApplicationUser>
    {
        public EmployerByIdSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.EmployerProfile);
        }
    }
}
