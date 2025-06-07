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
            AddInclude("AccountMemberships.Membership");
        }
    }

    public class EmployerByIdSpecification : BaseSpecification<ApplicationUser>
    {
        public EmployerByIdSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.EmployerProfile);
        }
    }

    public class JobSeekerByAccountIdSpecification : BaseSpecification<ApplicationUser>
    {
        public JobSeekerByAccountIdSpecification(int accountId) : base(x => x.Id == accountId)
        {
            AddInclude(x => x.JobSeekerSchedule.JobShifts);
        }
    }
}
