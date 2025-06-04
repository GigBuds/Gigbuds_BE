using Gigbuds_BE.Application.DTOs.JobRecommendations;
using Gigbuds_BE.Application.Features.Schedules.Commands.CreateJobPostSchedule;
using Gigbuds_BE.Domain.Entities.Schedule;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IJobRecommendationService
{
    Task<List<JobRecommendationDto>> GetRecommendedJobsWithScheduleAndDistanceAsync(
        int jobSeekerId, 
        string currentLocation, 
        int maxResults = 50);
    Task<(int score, string reason)> CalculateScheduleScore(
        ICollection<JobSeekerShift>? jobSeekerShifts,
        ICollection<JobShift>? jobShifts);
} 