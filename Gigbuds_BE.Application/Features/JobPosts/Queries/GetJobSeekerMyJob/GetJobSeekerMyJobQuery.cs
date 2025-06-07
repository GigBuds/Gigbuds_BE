using System;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.Specifications.JobPosts;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobSeekerMyJob;

public class GetJobSeekerMyJobQuery : IRequest<PagedResultDto<JobPostDto>>
{
    public JobSeekerMyJobRequestDto RequestDto { get; set; }

    public GetJobSeekerMyJobQuery(JobSeekerMyJobRequestDto requestDto)
    {
        RequestDto = requestDto;
    }
}
