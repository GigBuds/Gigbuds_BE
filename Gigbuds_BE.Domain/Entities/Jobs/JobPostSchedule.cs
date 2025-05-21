using System;
using System.Collections.Generic;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobPostSchedule : BaseEntity
{
    public int JobPostId { get; set; }
    public int ShiftCount { get; set; }
    public int MinimumShift { get; set; }
    // Navigation properties
    public virtual JobPost JobPost { get; set; }
    public virtual ICollection<JobShift> JobShifts { get; set; }
}
