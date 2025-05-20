using System;

namespace Gigbuds_BE.Domain.Entities.Jobs;

public class JobShift : BaseEntity
{
    public int JobPostScheduleId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    
    // Navigation properties
    public virtual JobPostSchedule JobPostSchedule { get; set; }
}
