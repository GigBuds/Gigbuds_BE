using Gigbuds_BE.Application.Specifications;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries
{
    public class GetAllJobPostsQueryParams : BasePagingParams
    {
        public string? SearchTerm { get; set; }
        public int? EmployerId { get; set; }
    }
}