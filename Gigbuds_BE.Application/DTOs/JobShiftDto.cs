namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class JobShiftDto
    {
        public required int DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}