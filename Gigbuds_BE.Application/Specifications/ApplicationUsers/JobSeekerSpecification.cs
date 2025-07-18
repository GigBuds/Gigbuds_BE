using Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerById;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;
using System.Linq.Expressions;

namespace Gigbuds_BE.Application.Specifications.ApplicationUsers
{
    public class JobSeekerDetailSpecification : BaseSpecification<ApplicationUser>
    {
        public JobSeekerDetailSpecification(int id) : base(x => x.Id == id)
        {
            // Personal and account info
            AddInclude(x => x.SkillTags);
            AddInclude(x => x.EducationalLevels);
            AddInclude(x => x.AccountExperienceTags);

            // Schedule information
            AddInclude(x => x.JobSeekerSchedule);
            AddInclude(x => x.JobSeekerSchedule.JobShifts);

            // Feedback information with job history
            AddInclude(x => x.Feedbacks);
            AddInclude("Feedbacks.Employer.EmployerProfile");
            AddInclude("Feedbacks.JobHistory.JobPost");

            // Follower information
            AddInclude(x => x.Followers);
        }

    }

    public class GetJobSeekerByNameSpecification : BaseSpecification<ApplicationUser>
    {
        public GetJobSeekerByNameSpecification(JobSeekerGetByNameQueryParams param) : base(x
            => String.IsNullOrEmpty(param.Name) || 
                x.FirstName.ToLower().Contains(param.Name.ToLower()) ||
                x.LastName.ToLower().Contains(param.Name.ToLower()) ||
                x.UserName.ToLower().Contains(param.Name.ToLower()) 
                && x.IsEnabled)
        {
        }
    }
}