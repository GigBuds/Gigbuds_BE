using Gigbuds_BE.Application.DTOs;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetAllJobPosts
{
    public class GetAllJobPostsQuery(GetAllJobPostsQueryParams queryParams) : IRequest<PagedResultDto<JobPostDto>>
    {
        public GetAllJobPostsQueryParams Params { get; private set; } = queryParams;
    }
}