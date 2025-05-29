using System;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Application.DTOs.Paging;
using Gigbuds_BE.Application.Specifications.JobPosts;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetSearchJobPosts;

public class GetSearchJobPostQuery : IRequest<PagedResultDto<SearchJobPostDto>>
{
    public JobPostSearchParams JobPostSearchParams { get; set; }
    public GetSearchJobPostQuery(JobPostSearchParams jobPostSearchParams)
    {
        JobPostSearchParams = jobPostSearchParams;
    }
}
