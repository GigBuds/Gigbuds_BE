using System;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.Specifications.JobPosts;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobPostByEmployerId;

public class GetJobPostByEmployerIdQuery : IRequest<PagedResultDto<JobPostDto>>
{
    public int EmployerId { get; set; }
    public GetJobPostByEmployerQueryParams QueryParams { get; set; }
    public GetJobPostByEmployerIdQuery(int employerId, GetJobPostByEmployerQueryParams queryParams)
    {
        EmployerId = employerId;
        QueryParams = queryParams;
    }
}
