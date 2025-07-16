using Gigbuds_BE.Domain.Entities.Feedbacks;
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

    public class GetEmployerProfileByIdSpecification : BaseSpecification<ApplicationUser>
    {
        public GetEmployerProfileByIdSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.EmployerProfile);
            AddInclude(x => x.EmployerFeedbacks.Where(f => f.FeedbackType == FeedbackType.JobSeekerToEmployer));
            AddInclude(x => x.Followers.Where(f => f.FollowedAccountId == id));
        }
    }

    public class GetAllAccountSpec : BaseSpecification<ApplicationUser>
    {
        public GetAllAccountSpec() : base(x => x.IsEnabled)
        {
            AddInclude(x => x.EmployerProfile);
        }
    }
}
