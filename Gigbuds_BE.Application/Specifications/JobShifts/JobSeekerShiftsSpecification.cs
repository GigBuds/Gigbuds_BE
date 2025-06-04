using Gigbuds_BE.Domain.Entities.Schedule;

namespace Gigbuds_BE.Application.Specifications.JobShifts
{
    internal class JobSeekerShiftsSpecification : BaseSpecification<JobSeekerShift>
    {
        /// <summary>
        /// Initializes a new instance of the JobSeekerShiftsSpecification class.
        /// </summary>
        /// <param name="accountId">The ID of the account to filter job seeker shifts by.</param>
        internal JobSeekerShiftsSpecification(int accountId) : base(j => j.JobSeekerSchedule.Account.Id == accountId)
        {
            AddInclude(j => j.JobSeekerSchedule);
            AddInclude(j => j.JobSeekerSchedule.Account);
        }
    }
}
