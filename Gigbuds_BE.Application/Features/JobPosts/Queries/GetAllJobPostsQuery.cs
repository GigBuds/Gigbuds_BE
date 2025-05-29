using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries
{
    public class GetAllJobPostsQuery(GetAllJobPostsQueryParams queryParams) : IRequest<PagedResultDto<JobPostDto>>
    {
        public GetAllJobPostsQueryParams Params { get; private set; } = queryParams;
    }
}