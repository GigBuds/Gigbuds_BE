namespace Gigbuds_BE.Domain.Entities.Schedule;

public class JobSeekerShift : BaseEntity
{
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
