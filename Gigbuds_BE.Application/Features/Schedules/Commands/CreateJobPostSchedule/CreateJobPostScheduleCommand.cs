namespace Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule
{
    public class CreateJobPostScheduleCommand
    {
        public required int ShiftCount { get; set; }
        public required int MinimumShift { get; set; }
        public required IReadOnlyList<JobShift> JobShifts { get; set; }
    }
    public record JobShift(int DayOfWeek, TimeOnly StartTime, TimeOnly EndTime);
}
