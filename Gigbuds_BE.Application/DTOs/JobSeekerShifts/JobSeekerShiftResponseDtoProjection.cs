using System;

namespace Gigbuds_BE.Application.DTOs.JobSeekerShifts;

public class JobSeekerShiftResponseDtoProjection
{
    public int JobSeekerShiftId { get; set; }
    public int JobSeekerScheduleId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
