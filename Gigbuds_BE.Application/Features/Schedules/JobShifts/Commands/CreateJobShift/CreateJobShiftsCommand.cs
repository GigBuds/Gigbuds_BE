using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;
using System.Text.Json.Serialization;
using MediatR;
namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift
{
    public class CreateJobShiftsCommand : INotification
    {
        [JsonIgnore]
        public required int JobPostId { get; set; }
        public required IReadOnlyList<JobShift> JobShifts { get; set; } = [];
    }
}