using Gigbuds_BE.Application.Specifications;

namespace Gigbuds_BE.Application.Specifications.JobPosts
{
    public class GetAllJobPostsQueryParams : BasePagingParams
    {
        public string? Search{ get; set; }
        public int? EmployerId { get; set; }
        public string? Status { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}