using System;
using Gigbuds_BE.Application.DTOs.JobShifts;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobShifts;

public class UpdateJobShiftCommand : IRequest<IReadOnlyList<JobShiftResponseDto>>
{
    public int JobSeekerScheduleId { get; set; }
    public IReadOnlyList<JobShiftDto> JobShifts { get; set; }
}
