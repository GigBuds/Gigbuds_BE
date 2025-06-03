namespace Gigbuds_BE.Application.DTOs.JobRecommendations;

public class GetJobRecommendationsQueryParams
{
    public string CurrentLocation { get; set; } = string.Empty;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool IncludeScheduleMatching { get; set; } = true;
    public bool IncludeDistanceCalculation { get; set; } = true;
} 