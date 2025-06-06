namespace Gigbuds_BE.Application.DTOs
{
    public class JobSeekerShiftsDto
    {
        public required int JobSeekerId { get; set; }
        public required int DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}
