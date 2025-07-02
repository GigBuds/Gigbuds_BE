using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.JobApplications;
using Gigbuds_BE.Application.DTOs.SkillTags;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Queries;

public class GetJobApplicationsByJobPostQueryHandler(IUnitOfWork unitOfWork, IMapper _mapper, IApplicationUserService<ApplicationUser> _applicationUserService) : IRequestHandler<GetJobApplicationsByJobPostQuery, List<JobApplicationForJobPostDto>>
{
    public async Task<List<JobApplicationForJobPostDto>> Handle(GetJobApplicationsByJobPostQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetJobApplicationsByJobPostSpecification(request.JobPostId);

        var jobApplications = await unitOfWork.Repository<JobApplication>().GetAllWithSpecificationProjectedAsync<JobApplicationForJobPostDto>(spec, _mapper.ConfigurationProvider);

        return jobApplications.ToList();
    }
}
