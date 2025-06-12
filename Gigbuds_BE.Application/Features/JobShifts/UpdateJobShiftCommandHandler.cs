using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobShifts;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobShifts;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Schedule;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobShifts;

public class UpdateJobShiftCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateJobShiftCommand, IReadOnlyList<JobShiftResponseDto>>
{
    public async Task<IReadOnlyList<JobShiftResponseDto>> Handle(UpdateJobShiftCommand request, CancellationToken cancellationToken)
    {
        var spec = new JobSeekerShiftsSpecification(request.JobSeekerScheduleId);

        var jobShifts = await unitOfWork.Repository<JobSeekerShift>().GetAllWithSpecificationAsync(spec);
        return jobShifts.Select(j => new JobShiftResponseDto
        {
            JobShiftId = j.Id,
            StartTime = j.StartTime,
            EndTime = j.EndTime,
            DayOfWeek = j.DayOfWeek
        }).ToList();
    }
}
