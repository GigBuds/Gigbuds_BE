using System;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Application.DTOs.Paging;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Domain.Entities.Jobs;
using MediatR;
using AutoMapper;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetSearchJobPosts;

public class GetSearchJobPostQueryHandler : IRequestHandler<GetSearchJobPostQuery, PagedResultDto<SearchJobPostDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetSearchJobPostQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<PagedResultDto<SearchJobPostDto>> Handle(GetSearchJobPostQuery request, CancellationToken cancellationToken)
    {
        var spec = new JobPostSpecification(request.JobPostSearchParams);
        var jobPosts = await _unitOfWork.Repository<JobPost>().GetAllWithSpecificationProjectedAsync<SearchJobPostDto>(spec, _mapper.ConfigurationProvider);
        return new PagedResultDto<SearchJobPostDto>(jobPosts.Count, jobPosts);
    }
}
