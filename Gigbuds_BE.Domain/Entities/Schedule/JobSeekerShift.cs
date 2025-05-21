namespace Gigbuds_BE.Domain.Entities.Schedule;

public class JobSeekerShift : BaseEntity
{
    public int JobSeekerScheduleId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public virtual JobSeekerSchedule JobSeekerSchedule { get; set; }
}
