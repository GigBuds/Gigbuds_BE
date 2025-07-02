using System;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;
using AutoMapper;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobPostByEmployerId;

public class GetJobPostByEmployerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetJobPostByEmployerIdQuery, List<JobPostDto>>
{
public async Task<List<JobPostDto>> Handle(GetJobPostByEmployerIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetJobPostByEmployerIdSpecification(request.EmployerId);
        var jobPosts = await unitOfWork.Repository<JobPost>().GetAllWithSpecificationProjectedAsync<JobPostDto>(spec, mapper.ConfigurationProvider);
        return jobPosts.ToList();
    }
}
