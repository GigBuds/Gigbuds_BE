using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobSeekerMyJob;

public class GetJobSeekerMyJobQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetJobSeekerMyJobQuery, PagedResultDto<JobPostDto>>
{
    public async Task<PagedResultDto<JobPostDto>> Handle(GetJobSeekerMyJobQuery request, CancellationToken cancellationToken)
    {
    var query = new GetJobSeekerMyJobSpecification(request.RequestDto);
        var jobPosts = await unitOfWork.Repository<JobApplication>().GetAllWithSpecificationProjectedAsync<JobPostDto>(query, mapper.ConfigurationProvider);

        return new PagedResultDto<JobPostDto>(jobPosts.Count, jobPosts);
    }
}
