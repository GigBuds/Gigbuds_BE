using System;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.DTOs.JobPosts;

public class SearchJobPostDto
{
    public int AccountId { get; set; }
    public string CompanyLogo { get; set; }
    public string CompanyName { get; set; }
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public string JobRequirement { get; set; }
    public string ExperienceRequirement { get; set; }
    public int Salary { get; set; }
    public SalaryUnit SalaryUnit { get; set; }
    public string JobLocation { get; set; }
    public DateTime ExpireTime { get; set; }
    public string Benefit { get; set; }
    public JobPostStatus JobPostStatus { get; set; }
    public int VacancyCount { get; set; }
    public bool IsOutstandingPost { get; set; }
    public bool IsMale { get; set; }
    public int? AgeRequirement { get; set; }
    public int JobPositionId { get; set; }
    public string DistrictCode { get; set; }
    public string ProvinceCode { get; set; }
    public int PriorityLevel { get; set; }
}
