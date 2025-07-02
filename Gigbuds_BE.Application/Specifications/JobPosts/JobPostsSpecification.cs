using Gigbuds_BE.Domain.Entities.Jobs;
using Quartz.Util;


namespace Gigbuds_BE.Application.Specifications.JobPosts
{
    public class JobPostByIdSpecification : BaseSpecification<JobPost>
    {
        public JobPostByIdSpecification(int jobPostId) : base(j => j.Id == jobPostId)
        {
            AddInclude(x => x.JobApplications);
        }
    }

    public class GetAllJobPostsSpecification : BaseSpecification<JobPost>
    {
        public GetAllJobPostsSpecification(GetAllJobPostsQueryParams queryParams)
            : base(j => j.IsEnabled
                && (string.IsNullOrEmpty(queryParams.Search)
                    || j.JobTitle.ToLower().Contains(queryParams.Search.ToLower())
                    || j.JobDescription.ToLower().Contains(queryParams.Search.ToLower())
                    || j.JobRequirement.ToLower().Contains(queryParams.Search.ToLower())
                    || j.JobLocation.ToLower().Contains(queryParams.Search.ToLower()))
                && (string.IsNullOrEmpty(queryParams.Status) || queryParams.Status.Equals("all") || Enum.Parse<JobPostStatus>(queryParams.Status).Equals(j.JobPostStatus))
                && (queryParams.EmployerId == null || j.AccountId == queryParams.EmployerId))
        {
            AddInclude(x => x.JobPosition);
            AddPaging(queryParams.PageSize * (queryParams.PageIndex - 1), queryParams.PageSize);
            AddInclude(x => x.JobApplications);
            switch (queryParams.SortBy)
            {
                case "createdAt":
                    if (queryParams.SortOrder.Equals("asc"))
                        AddOrderBy(a => a.CreatedAt);
                    else AddOrderByDesc(a => a.CreatedAt);
                        break;
            }
        }
    }

    public class GetSearchJobPostsSpecification : BaseSpecification<JobPost>
    {
        public GetSearchJobPostsSpecification(JobPostSearchParams jobPostSearchParams)
        : base(x =>
        (string.IsNullOrEmpty(jobPostSearchParams.CompanyName) || x.Account.EmployerProfile.CompanyName.ToLower().Contains(jobPostSearchParams.CompanyName.ToLower())) &&
        (string.IsNullOrEmpty(jobPostSearchParams.JobName) || x.JobTitle.ToLower().Contains(jobPostSearchParams.JobName.ToLower())) &&
        (!jobPostSearchParams.SalaryFrom.HasValue || x.Salary >= jobPostSearchParams.SalaryFrom.Value) &&
        (!jobPostSearchParams.SalaryTo.HasValue || x.Salary <= jobPostSearchParams.SalaryTo.Value) &&
        (!jobPostSearchParams.JobPositionId.HasValue || x.JobPositionId == jobPostSearchParams.JobPositionId.Value) &&
        (!jobPostSearchParams.JobTimeFrom.HasValue || x.CreatedAt >= jobPostSearchParams.JobTimeFrom.Value) &&
        (!jobPostSearchParams.JobTimeTo.HasValue || x.ExpireTime <= jobPostSearchParams.JobTimeTo.Value) &&
        (!jobPostSearchParams.SalaryUnit.HasValue || x.SalaryUnit == jobPostSearchParams.SalaryUnit.Value) &&
        (jobPostSearchParams.DistrictCodeList.Count() == 0 || jobPostSearchParams.DistrictCodeList.Contains(x.DistrictCode))
        && (x.JobPostStatus == JobPostStatus.Open))
        {
            AddInclude(x => x.Account);
            AddInclude(x => x.JobPosition);
            AddInclude(x => x.JobPostSchedule.JobShifts);
            AddOrderByDesc(x => x.PriorityLevel);
            AddPaging(jobPostSearchParams.PageSize * (jobPostSearchParams.PageIndex - 1), jobPostSearchParams.PageSize);
        }
    }

    public class GetJobPostByIdSpecification : BaseSpecification<JobPost>
    {
        public GetJobPostByIdSpecification(int jobPostId) : base(j => j.Id == jobPostId)
        {
            AddInclude(x => x.Account);
            AddInclude(x => x.JobPosition);
            AddInclude(x => x.JobPostSchedule.JobShifts);
            AddInclude(x => x.Account.EmployerProfile);
        }
    }

    public class ActiveJobPostsSpecification : BaseSpecification<JobPost>
    {
        public ActiveJobPostsSpecification() : base(j =>
            j.IsEnabled &&
            j.JobPostStatus == JobPostStatus.Open &&
            j.ExpireTime > DateTime.UtcNow)
        {
            AddInclude(j => j.Account.EmployerProfile);
            AddInclude(j => j.JobPostSchedule.JobShifts);
            AddInclude(j => j.JobPosition);
        }
    }

    public class GetJobPostByEmployerIdSpecification : BaseSpecification<JobPost>
    {
        public GetJobPostByEmployerIdSpecification(int employerId) : base(j => j.AccountId == employerId)
        {
            AddInclude(x => x.Account);
            AddInclude(x => x.JobPosition);
            AddInclude(x => x.JobPostSchedule.JobShifts);
        }
    }

    public class GetNumOfAvailablePostByEmployerIdSpecification : BaseSpecification<JobPost>
    {
        public GetNumOfAvailablePostByEmployerIdSpecification(int employerId) : base(j => j.AccountId == employerId)
        {
        }
    }

}