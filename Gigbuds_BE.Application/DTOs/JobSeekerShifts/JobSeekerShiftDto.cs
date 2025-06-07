using System;
using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.DTOs.JobSeekerShifts;

public class JobSeekerShiftDto
{
    [JsonIgnore]
    public int JobSeekerId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
