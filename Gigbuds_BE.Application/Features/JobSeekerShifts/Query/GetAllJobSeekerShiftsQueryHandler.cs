using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobSeekerShifts;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobShifts;
using Gigbuds_BE.Domain.Entities.Schedule;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobSeekerShifts.Query;

public class GetAllJobSeekerShiftsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllJobSeekerShiftsQuery, List<JobSeekerShiftResponseDtoProjection>>
{
    public async Task<List<JobSeekerShiftResponseDtoProjection>> Handle(GetAllJobSeekerShiftsQuery request, CancellationToken cancellationToken)
    {
        var spec = new JobSeekerShiftsSpecification(request.AccountId);
        var jobSeekerShifts = await unitOfWork.Repository<JobSeekerShift>().GetAllWithSpecificationProjectedAsync<JobSeekerShiftResponseDtoProjection>(spec, mapper.ConfigurationProvider);
        return jobSeekerShifts.ToList();
    }
}
