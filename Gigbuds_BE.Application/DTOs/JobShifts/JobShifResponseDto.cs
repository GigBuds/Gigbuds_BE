using System;

namespace Gigbuds_BE.Application.DTOs.JobShifts;

public class JobShiftResponseDto
{
    public int JobShiftId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int DayOfWeek { get; set; }
}
