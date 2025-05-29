using System;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.Specifications.JobPosts;

public class JobPostSpecification : BaseSpecification<JobPost>
{
    public JobPostSpecification(JobPostSearchParams jobPostSearchParams)
        : base(x =>
        (string.IsNullOrEmpty(jobPostSearchParams.EmployerName) || x.Account.EmployerProfile.CompanyName.ToLower().Contains(jobPostSearchParams.EmployerName.ToLower())) &&
        (string.IsNullOrEmpty(jobPostSearchParams.JobName) || x.JobTitle.ToLower().Contains(jobPostSearchParams.JobName.ToLower())) &&
        (!jobPostSearchParams.SalaryFrom.HasValue || x.Salary >= jobPostSearchParams.SalaryFrom.Value) &&
        (!jobPostSearchParams.SalaryTo.HasValue || x.Salary <= jobPostSearchParams.SalaryTo.Value) &&
        (!jobPostSearchParams.JobPositionId.HasValue || x.JobPositionId == jobPostSearchParams.JobPositionId.Value) &&
        (!jobPostSearchParams.AgeFrom.HasValue || x.AgeRequirement >= jobPostSearchParams.AgeFrom.Value) &&
        (!jobPostSearchParams.AgeTo.HasValue || x.AgeRequirement <= jobPostSearchParams.AgeTo.Value) &&
        (!jobPostSearchParams.JobTimeFrom.HasValue || x.CreatedAt >= jobPostSearchParams.JobTimeFrom.Value) &&
        (!jobPostSearchParams.JobTimeTo.HasValue || x.ExpireTime <= jobPostSearchParams.JobTimeTo.Value) &&
        (!jobPostSearchParams.IsMale.HasValue || x.IsMale == jobPostSearchParams.IsMale.Value) &&
        (!jobPostSearchParams.SalaryUnit.HasValue || x.SalaryUnit == jobPostSearchParams.SalaryUnit.Value) &&
        (jobPostSearchParams.DistrictCodeList.Count() == 0 || jobPostSearchParams.DistrictCodeList.Contains(x.DistrictCode)))
    {
        AddInclude(x => x.Account);
        AddInclude(x => x.JobPosition);
        AddInclude(x => x.JobPostSchedule.JobShifts);
        AddOrderByDesc(x => x.PriorityLevel);
    }
}
