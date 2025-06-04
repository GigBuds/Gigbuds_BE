using Gigbuds_BE.Application.DTOs;
using MediatR;

namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Queries.GetJobSeekerShifts
{
    internal class GetJobSeekerShiftsQuery : IRequest<IReadOnlyList<JobSeekerShiftsDto>>
    {
        public int AccountId { get; set; }
    }
}
