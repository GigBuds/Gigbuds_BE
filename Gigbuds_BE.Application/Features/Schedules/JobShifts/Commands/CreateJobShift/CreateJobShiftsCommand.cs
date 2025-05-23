
using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;

namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift
{
    public class CreateJobShiftsCommand
    {
        public required IReadOnlyList<JobShift> JobShifts { get; set; } = [];
    }
}
