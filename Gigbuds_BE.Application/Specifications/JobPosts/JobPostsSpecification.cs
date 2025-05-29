using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Application.Features.JobPosts.Queries;

namespace Gigbuds_BE.Application.Specifications.JobPosts
{
    public class JobPostByIdSpecification : BaseSpecification<JobPost>
    {
        public JobPostByIdSpecification(int jobPostId) : base(j => j.Id == jobPostId)
        {
        }
    }

    public class GetAllJobPostsSpecification : BaseSpecification<JobPost>
    {
        public GetAllJobPostsSpecification(GetAllJobPostsQueryParams queryParams)
            : base(j => j.IsEnabled
                && j.JobPostStatus == JobPostStatus.Open
                && (string.IsNullOrEmpty(queryParams.SearchTerm)
                    || j.JobTitle.ToLower().Contains(queryParams.SearchTerm.ToLower())
                    || j.JobDescription.ToLower().Contains(queryParams.SearchTerm.ToLower())
                    || j.JobRequirement.ToLower().Contains(queryParams.SearchTerm.ToLower())
                    || j.JobLocation.ToLower().Contains(queryParams.SearchTerm.ToLower())))
        {
            AddPaging(queryParams.PageSize * (queryParams.PageIndex - 1), queryParams.PageSize);
        }
    }
}