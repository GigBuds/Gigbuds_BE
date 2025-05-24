using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule
{
    public class CreateJobPostScheduleCommand
    {
        [JsonIgnore]
        public int JobPostId { get; set; }
        public required int ShiftCount { get; set; }
        public required int MinimumShift { get; set; }
        [JsonPropertyName("JobShifts")]
        public required IReadOnlyList<JobShift> JobShifts { get; set; }
    }
    public record JobShift(int DayOfWeek, TimeOnly StartTime, TimeOnly EndTime);
}
