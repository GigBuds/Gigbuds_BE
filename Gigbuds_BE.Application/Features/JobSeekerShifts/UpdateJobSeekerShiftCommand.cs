
using Gigbuds_BE.Application.DTOs.JobSeekerShifts;
using Gigbuds_BE.Application.DTOs.JobShifts;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobSeekerShifts;

public class UpdateJobSeekerShiftCommand : IRequest<IReadOnlyList<JobSeekerShiftResponseDto>>
{
    public int JobSeekerId { get; set; }
    public IReadOnlyList<JobSeekerShiftDto> JobShifts { get; set; }
}
