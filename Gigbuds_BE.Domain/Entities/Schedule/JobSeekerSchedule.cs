using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Domain.Entities.Schedule;

public class JobSeekerSchedule : BaseEntity
{
    public int AccountId { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<JobSeekerShift> JobShifts { get; set; }
}

