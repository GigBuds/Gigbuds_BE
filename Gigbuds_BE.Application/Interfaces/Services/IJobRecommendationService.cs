using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Application.DTOs.JobRecommendations;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Schedule;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IJobRecommendationService
{
    Task<List<JobRecommendationDto>> GetRecommendedJobsWithScheduleAndDistanceAsync(
        int jobSeekerId, 
        string currentLocation, 
        int maxResults = 50);
    Task<JobPostScoreDto> CalculateScheduleScore(
        ICollection<JobSeekerShift>? jobSeekerShifts,
        ICollection<JobShift>? jobShifts, int minimumShift);
} 