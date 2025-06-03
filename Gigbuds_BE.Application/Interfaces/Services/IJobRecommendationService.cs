using Gigbuds_BE.Application.DTOs.JobRecommendations;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IJobRecommendationService
{
    Task<List<JobRecommendationDto>> GetRecommendedJobsWithScheduleAndDistanceAsync(
        int jobSeekerId, 
        string currentLocation, 
        int maxResults = 50);
} 