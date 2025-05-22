using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Domain.Entities.Schedule;

public class JobSeekerSchedule : BaseEntity
{
    public virtual ApplicationUser Account { get; set; }
    public virtual ICollection<JobSeekerShift> JobShifts { get; set; }
}

