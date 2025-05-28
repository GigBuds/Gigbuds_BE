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
        (!jobPostSearchParams.IsMale.HasValue || x.IsMale == jobPostSearchParams.IsMale.Value) &&
        (!jobPostSearchParams.SalaryUnit.HasValue || x.SalaryUnit == jobPostSearchParams.SalaryUnit.Value))
    {

    }
}
