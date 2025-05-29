namespace Gigbuds_BE.Application.DTOs.ApplicationUsers
{
    public class JobScheduleDto
    {
        public required int ShiftCount { get; set; }
        public required int MinimumShift { get; set; }
        public required IReadOnlyList<JobShiftDto> JobShifts { get; set; }
    }
}