using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Domain.Entities.Jobs;

namespace Gigbuds_BE.Application.DTOs.JobRecommendations;

public class JobRecommendationDto
{
    // Job Post Information
    public int JobPostId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
    public string JobRequirement { get; set; } = string.Empty;
    public string ExperienceRequirement { get; set; } = string.Empty;
    public int Salary { get; set; }
    public SalaryUnit SalaryUnit { get; set; }
    public string JobLocation { get; set; } = string.Empty;
    public DateTime ExpireTime { get; set; }
    public string Benefit { get; set; } = string.Empty;
    public int VacancyCount { get; set; }
    public bool IsOutstandingPost { get; set; }
    public bool IsMale { get; set; }
    public int? AgeRequirement { get; set; }
    public string DistrictCode { get; set; } = string.Empty;
    public string ProvinceCode { get; set; } = string.Empty;

    // Company Information
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyLogo { get; set; } = string.Empty;

    // Job Schedule
    public JobScheduleDto? JobSchedule { get; set; }

    // Recommendation Scoring
    public int TotalScore { get; set; }
    public int ScheduleScore { get; set; }
    public int DistanceScore { get; set; }
    public string ScheduleMatchReason { get; set; } = string.Empty;
    public string DistanceMatchReason { get; set; } = string.Empty;

    // Distance Information
    public double DistanceKilometers { get; set; }
    public string FormattedDistance { get; set; } = string.Empty;
    public string EstimatedDuration { get; set; } = string.Empty;
}

public class JobRecommendationRequest
{
    public int JobSeekerId { get; set; }
    public string CurrentLocation { get; set; } = string.Empty;
    public int MaxResults { get; set; } = 50;
    public bool IncludeScheduleMatching { get; set; } = true;
    public bool IncludeDistanceCalculation { get; set; } = true;
} 