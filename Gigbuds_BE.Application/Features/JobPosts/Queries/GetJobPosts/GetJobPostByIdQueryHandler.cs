using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobPosts;

public class GetJobPostByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetJobPostByIdQuery, JobPostDto>
{
    public async Task<JobPostDto> Handle(GetJobPostByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetJobPostByIdSpecification(request.Id);
        var jobPost = await unitOfWork.Repository<JobPost>().GetBySpecificationProjectedAsync<JobPostDto>(spec, mapper.ConfigurationProvider);
        if (jobPost == null)
        {
            throw new NotFoundException(nameof(JobPost), request.Id);
        }
        return jobPost;
    }
}
