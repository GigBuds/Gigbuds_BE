using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobApplications;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetAllJobPosts
{
    public class GetAllJobPostsQueryHandler : IRequestHandler<GetAllJobPostsQuery, PagedResultDto<JobPostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllJobPostsQueryHandler> _logger;

        public GetAllJobPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllJobPostsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResultDto<JobPostDto>> Handle(GetAllJobPostsQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetAllJobPostsSpecification(request.Params);
            var jobPostsCount = await _unitOfWork.Repository<JobPost>().CountAsync(spec);
            var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationProjectedAsync<JobPostDto>(spec, _mapper.ConfigurationProvider);

            return new PagedResultDto<JobPostDto>(jobPostsCount, jobPosts);
        }
    }
}