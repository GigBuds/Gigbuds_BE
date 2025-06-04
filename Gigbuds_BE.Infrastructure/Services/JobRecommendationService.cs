using AutoMapper;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.JobPosts;
using Gigbuds_BE.Application.DTOs.JobRecommendations;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Application.Specifications.JobSeekerSchedules;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Schedule;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Infrastructure.Services;

public class JobRecommendationService : IJobRecommendationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IMapper _mapper;
    private readonly ILogger<JobRecommendationService> _logger;

    public JobRecommendationService(
        IUnitOfWork unitOfWork,
        IGoogleMapsService googleMapsService,
        IMapper mapper,
        ILogger<JobRecommendationService> logger)
    {
        _unitOfWork = unitOfWork;
        _googleMapsService = googleMapsService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<JobRecommendationDto>> GetRecommendedJobsWithScheduleAndDistanceAsync(
        int jobSeekerId, 
        string currentLocation, 
        int maxResults = 50)
    {
        try
        {

            // 1. Get JobSeeker's schedule
            var jobSeekerSchedule = await GetJobSeekerScheduleAsync(jobSeekerId);
            if (jobSeekerSchedule == null)
            {
                _logger.LogWarning("No schedule found for JobSeeker {JobSeekerId}", jobSeekerId);
                return new List<JobRecommendationDto>();
            }

            // 2. Get all open job posts with their schedules
            var jobPosts = await GetActiveJobPostsAsync();
            if (!jobPosts.Any())
            {
                _logger.LogWarning("No active job posts found");
                return new List<JobRecommendationDto>();
            }

            _logger.LogInformation("Found {JobPostCount} active job posts to analyze", jobPosts.Count);

            // 3. Calculate schedule scores for all job posts
            var jobRecommendations = await Task.WhenAll(jobPosts.Select(async jp => await CreateJobRecommendationDto(jp, jobSeekerSchedule)));

            // 4. Calculate distance scores using Google Maps
            await CalculateDistanceScoresAsync(jobRecommendations.ToList(), currentLocation);

            // 5. Calculate total scores and sort
            foreach (var recommendation in jobRecommendations)
            {
                recommendation.TotalScore = recommendation.ScheduleScore + recommendation.DistanceScore;
            }

            // 6. Sort by total score (highest first) and take the requested number
            var result = jobRecommendations
                .OrderByDescending(r => r.TotalScore)
                .ThenByDescending(r => r.DistanceScore)
                .ThenByDescending(r => r.ScheduleScore)
                .Take(maxResults)
                .ToList();

            _logger.LogInformation("Returning {ResultCount} job recommendations for JobSeeker {JobSeekerId}", 
                result.Count, jobSeekerId);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating job recommendations for JobSeeker {JobSeekerId}", jobSeekerId);
            throw;
        }
    }

    private async Task<JobSeekerSchedule?> GetJobSeekerScheduleAsync(int jobSeekerId)
    {
        var schedule = await _unitOfWork.Repository<JobSeekerSchedule>()
            .GetBySpecificationAsync(new JobSeekerScheduleSpecification(jobSeekerId));
        
        return schedule;
    }

    private async Task<List<JobPost>> GetActiveJobPostsAsync()
    {
        var jobPosts = await _unitOfWork.Repository<JobPost>()
            .GetAllWithSpecificationAsync(new ActiveJobPostsSpecification());
        
        return jobPosts.ToList();
    }

    private async Task<JobRecommendationDto> CreateJobRecommendationDto(JobPost jobPost, JobSeekerSchedule jobSeekerSchedule)
    {
        var recommendation = new JobRecommendationDto
        {
            JobPostId = jobPost.Id,
            JobTitle = jobPost.JobTitle,
            JobDescription = jobPost.JobDescription,
            JobRequirement = jobPost.JobRequirement,
            ExperienceRequirement = jobPost.ExperienceRequirement,
            Salary = jobPost.Salary,
            SalaryUnit = jobPost.SalaryUnit,
            JobLocation = jobPost.JobLocation,
            ExpireTime = jobPost.ExpireTime,
            Benefit = jobPost.Benefit,
            VacancyCount = jobPost.VacancyCount,
            IsOutstandingPost = jobPost.IsOutstandingPost,
            IsMale = jobPost.IsMale,
            AgeRequirement = jobPost.AgeRequirement,
            DistrictCode = jobPost.DistrictCode,
            ProvinceCode = jobPost.ProvinceCode,
            CompanyName = jobPost.Account?.EmployerProfile?.CompanyName ?? "Unknown Company",
            CompanyLogo = jobPost.Account?.EmployerProfile?.CompanyLogo ?? "",
            JobSchedule = jobPost.JobPostSchedule != null ? _mapper.Map<JobScheduleDto>(jobPost.JobPostSchedule) : null
        };

        // Calculate schedule score
        var scheduleScore = await CalculateScheduleScore(jobSeekerSchedule.JobShifts, jobPost.JobPostSchedule?.JobShifts);
        recommendation.ScheduleScore = scheduleScore.Score;
        recommendation.ScheduleMatchReason = scheduleScore.Reason;

        return recommendation;
    }

    public async Task<JobPostScoreDto> CalculateScheduleScore(
        ICollection<JobSeekerShift>? jobSeekerShifts,
        ICollection<JobShift>? jobShifts)
    {
        if (jobSeekerShifts == null || !jobSeekerShifts.Any() || 
            jobShifts == null || !jobShifts.Any())
        {
            return new JobPostScoreDto {
                Score = 0,
                Reason = "No schedule information available"
            };
        }

        var matchingShifts = 0;
        var totalJobShifts = jobShifts.Count;

        foreach (var jobShift in jobShifts)
        {
            // Check if any job seeker shift matches this job shift
            var hasMatch = jobSeekerShifts.Any(seekerShift =>
                seekerShift.DayOfWeek == jobShift.DayOfWeek &&
                DoesTimeOverlap(seekerShift.StartTime, seekerShift.EndTime, 
                              jobShift.StartTime, jobShift.EndTime));

            if (hasMatch)
            {
                matchingShifts++;
            }
        }

        // Scoring logic:
        // - All shifts match: 3 points
        // - Half or more shifts match: 1 point
        // - Less than half: 0 points
        var matchPercentage = (double)matchingShifts / totalJobShifts;

        if (matchPercentage >= 1.0)
        {
            return new JobPostScoreDto {
                Score = 3,
                Reason = $"Perfect schedule match - all {totalJobShifts} shifts available"
            };
        }
        else if (matchPercentage >= 0.5)
        {
            return new JobPostScoreDto {
                Score = 1,
                Reason = $"Partial schedule match - {matchingShifts} of {totalJobShifts} shifts available"
            };
        }
        else
        {
            return new JobPostScoreDto {
                Score = 0,
                Reason = $"Poor schedule match - only {matchingShifts} of {totalJobShifts} shifts available"
            };
        }
    }

    private static bool DoesTimeOverlap(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
    {
        return start1 < end2 && start2 < end1;
    }

    private async Task CalculateDistanceScoresAsync(
        List<JobRecommendationDto> recommendations, 
        string currentLocation)
    {
        try
        {
            // Extract unique job locations
            var jobLocations = recommendations
                .Where(r => !string.IsNullOrWhiteSpace(r.JobLocation))
                .Select(r => r.JobLocation)
                .Distinct()
                .ToList();

            if (!jobLocations.Any())
            {
                _logger.LogWarning("No job locations found for distance calculation");
                return;
            }

            _logger.LogInformation("Calculating distances to {LocationCount} unique job locations", jobLocations.Count);

            // Calculate distances using Google Maps
            var distanceResults = await _googleMapsService
                .CalculateDistancesToMultipleDestinationsAsync(currentLocation, jobLocations);

            // Create a lookup dictionary for quick access
            var distanceLookup = distanceResults
                .Where(d => d.IsSuccess)
                .ToDictionary(d => d.Destination!, d => d);

            // Apply distance scores to recommendations
            foreach (var recommendation in recommendations)
            {
                if (distanceLookup.TryGetValue(recommendation.JobLocation, out var distanceResult))
                {
                    recommendation.DistanceKilometers = distanceResult.DistanceKilometers;
                    recommendation.FormattedDistance = distanceResult.LocalizedDistance ?? $"{distanceResult.DistanceKilometers:F1} km";
                    recommendation.EstimatedDuration = distanceResult.LocalizedDuration ?? distanceResult.Duration ?? "Unknown";

                    var distanceScore = CalculateDistanceScore(distanceResult.DistanceKilometers);
                    recommendation.DistanceScore = distanceScore.Score;
                    recommendation.DistanceMatchReason = distanceScore.Reason;
                }
                else
                {
                    recommendation.DistanceScore = 0;
                    recommendation.DistanceMatchReason = "Distance calculation failed";
                    recommendation.FormattedDistance = "Unknown";
                    recommendation.EstimatedDuration = "Unknown";
                }
            }

            _logger.LogInformation("Distance calculation completed for {ProcessedCount} recommendations", 
                recommendations.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating distance scores");
            
            // Set default values for all recommendations if distance calculation fails
            foreach (var recommendation in recommendations)
            {
                recommendation.DistanceScore = 0;
                recommendation.DistanceMatchReason = "Distance calculation unavailable";
                recommendation.FormattedDistance = "Unknown";
                recommendation.EstimatedDuration = "Unknown";
            }
        }
    }

    private static (int Score, string Reason) CalculateDistanceScore(double distanceKm)
    {
        // Distance scoring logic:
        // 0-2km: 4 points
        // 2-5km: 3 points
        // 5-10km: 2 points
        // >10km: 1 point

        return distanceKm switch
        {
            <= 2.0 => (4, $"Very close - {distanceKm:F1}km away"),
            <= 5.0 => (3, $"Close - {distanceKm:F1}km away"),
            <= 10.0 => (2, $"Moderate distance - {distanceKm:F1}km away"),
            _ => (1, $"Far - {distanceKm:F1}km away")
        };
    }
}using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using Gigbuds_BE.Application.DTOs.JobRecommendations;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Application.Specifications.JobPosts;
using Gigbuds_BE.Application.Specifications.JobSeekerSchedules;
using Gigbuds_BE.Domain.Entities.Jobs;
using Gigbuds_BE.Domain.Entities.Schedule;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Infrastructure.Services;

public class JobRecommendationService : IJobRecommendationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IMapper _mapper;
    private readonly ILogger<JobRecommendationService> _logger;

    public JobRecommendationService(
        IUnitOfWork unitOfWork,
        IGoogleMapsService googleMapsService,
        IMapper mapper,
        ILogger<JobRecommendationService> logger)
    {
        _unitOfWork = unitOfWork;
        _googleMapsService = googleMapsService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<JobRecommendationDto>> GetRecommendedJobsWithScheduleAndDistanceAsync(
        int jobSeekerId, 
        string currentLocation, 
        int maxResults = 50)
    {
        try
        {

            // 1. Get JobSeeker's schedule
            var jobSeekerSchedule = await GetJobSeekerScheduleAsync(jobSeekerId);
            if (jobSeekerSchedule == null)
            {
                _logger.LogWarning("No schedule found for JobSeeker {JobSeekerId}", jobSeekerId);
                return new List<JobRecommendationDto>();
            }

            // 2. Get all open job posts with their schedules
            var jobPosts = await GetActiveJobPostsAsync();
            if (!jobPosts.Any())
            {
                _logger.LogWarning("No active job posts found");
                return new List<JobRecommendationDto>();
            }

            _logger.LogInformation("Found {JobPostCount} active job posts to analyze", jobPosts.Count);

            // 3. Calculate schedule scores for all job posts
            var jobRecommendations = jobPosts.Select(jp => CreateJobRecommendationDto(jp, jobSeekerSchedule))
                .ToList();

            // 4. Calculate distance scores using Google Maps
            await CalculateDistanceScoresAsync(jobRecommendations, currentLocation);

            // 5. Calculate total scores and sort
            foreach (var recommendation in jobRecommendations)
            {
                recommendation.TotalScore = recommendation.ScheduleScore + recommendation.DistanceScore;
            }

            // 6. Sort by total score (highest first) and take the requested number
            var result = jobRecommendations
                .OrderByDescending(r => r.TotalScore)
                .ThenByDescending(r => r.DistanceScore)
                .ThenByDescending(r => r.ScheduleScore)
                .Take(maxResults)
                .ToList();

            _logger.LogInformation("Returning {ResultCount} job recommendations for JobSeeker {JobSeekerId}", 
                result.Count, jobSeekerId);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating job recommendations for JobSeeker {JobSeekerId}", jobSeekerId);
            throw;
        }
    }

    private async Task<JobSeekerSchedule?> GetJobSeekerScheduleAsync(int jobSeekerId)
    {
        var schedule = await _unitOfWork.Repository<JobSeekerSchedule>()
            .GetBySpecificationAsync(new JobSeekerScheduleSpecification(jobSeekerId));
        
        return schedule;
    }

    private async Task<List<JobPost>> GetActiveJobPostsAsync()
    {
        var jobPosts = await _unitOfWork.Repository<JobPost>()
            .GetAllWithSpecificationAsync(new ActiveJobPostsSpecification());
        
        return jobPosts.ToList();
    }

    private JobRecommendationDto CreateJobRecommendationDto(JobPost jobPost, JobSeekerSchedule jobSeekerSchedule)
    {
        var recommendation = new JobRecommendationDto
        {
            JobPostId = jobPost.Id,
            JobTitle = jobPost.JobTitle,
            JobDescription = jobPost.JobDescription,
            JobRequirement = jobPost.JobRequirement,
            ExperienceRequirement = jobPost.ExperienceRequirement,
            Salary = jobPost.Salary,
            SalaryUnit = jobPost.SalaryUnit,
            JobLocation = jobPost.JobLocation,
            ExpireTime = jobPost.ExpireTime,
            Benefit = jobPost.Benefit,
            VacancyCount = jobPost.VacancyCount,
            IsOutstandingPost = jobPost.IsOutstandingPost,
            IsMale = jobPost.IsMale,
            AgeRequirement = jobPost.AgeRequirement,
            DistrictCode = jobPost.DistrictCode,
            ProvinceCode = jobPost.ProvinceCode,
            CompanyName = jobPost.Account?.EmployerProfile?.CompanyName ?? "Unknown Company",
            CompanyLogo = jobPost.Account?.EmployerProfile?.CompanyLogo ?? "",
            JobSchedule = jobPost.JobPostSchedule != null ? _mapper.Map<JobScheduleDto>(jobPost.JobPostSchedule) : null
        };

        // Calculate schedule score
        var scheduleScore = CalculateScheduleScore(jobSeekerSchedule.JobShifts, jobPost.JobPostSchedule?.JobShifts);
        recommendation.ScheduleScore = scheduleScore.Score;
        recommendation.ScheduleMatchReason = scheduleScore.Reason;

        return recommendation;
    }

    private (int Score, string Reason) CalculateScheduleScore(
        ICollection<JobSeekerShift>? jobSeekerShifts,
        ICollection<JobShift>? jobShifts)
    {
        if (jobSeekerShifts == null || !jobSeekerShifts.Any() || 
            jobShifts == null || !jobShifts.Any())
        {
            return (0, "No schedule information available");
        }

        var matchingShifts = 0;
        var totalJobShifts = jobShifts.Count;

        foreach (var jobShift in jobShifts)
        {
            // Check if any job seeker shift matches this job shift
            var hasMatch = jobSeekerShifts.Any(seekerShift =>
                seekerShift.DayOfWeek == jobShift.DayOfWeek &&
                DoesTimeOverlap(seekerShift.StartTime, seekerShift.EndTime, 
                              jobShift.StartTime, jobShift.EndTime));

            if (hasMatch)
            {
                matchingShifts++;
            }
        }

        // Scoring logic:
        // - All shifts match: 3 points
        // - Half or more shifts match: 1 point
        // - Less than half: 0 points
        var matchPercentage = (double)matchingShifts / totalJobShifts;

        if (matchPercentage >= 1.0)
        {
            return (3, $"Perfect schedule match - all {totalJobShifts} shifts available");
        }
        else if (matchPercentage >= 0.5)
        {
            return (1, $"Partial schedule match - {matchingShifts} of {totalJobShifts} shifts available");
        }
        else
        {
            return (0, $"Poor schedule match - only {matchingShifts} of {totalJobShifts} shifts available");
        }
    }

    private static bool DoesTimeOverlap(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
    {
        return start1 < end2 && start2 < end1;
    }

    private async Task CalculateDistanceScoresAsync(
        List<JobRecommendationDto> recommendations, 
        string currentLocation)
    {
        try
        {
            // Extract unique job locations
            var jobLocations = recommendations
                .Where(r => !string.IsNullOrWhiteSpace(r.JobLocation))
                .Select(r => r.JobLocation)
                .Distinct()
                .ToList();

            if (!jobLocations.Any())
            {
                _logger.LogWarning("No job locations found for distance calculation");
                return;
            }

            _logger.LogInformation("Calculating distances to {LocationCount} unique job locations", jobLocations.Count);

            // Calculate distances using Google Maps
            var distanceResults = await _googleMapsService
                .CalculateDistancesToMultipleDestinationsAsync(currentLocation, jobLocations);

            // Create a lookup dictionary for quick access
            var distanceLookup = distanceResults
                .Where(d => d.IsSuccess)
                .ToDictionary(d => d.Destination!, d => d);

            // Apply distance scores to recommendations
            foreach (var recommendation in recommendations)
            {
                if (distanceLookup.TryGetValue(recommendation.JobLocation, out var distanceResult))
                {
                    recommendation.DistanceKilometers = distanceResult.DistanceKilometers;
                    recommendation.FormattedDistance = distanceResult.LocalizedDistance ?? $"{distanceResult.DistanceKilometers:F1} km";
                    recommendation.EstimatedDuration = distanceResult.LocalizedDuration ?? distanceResult.Duration ?? "Unknown";

                    var distanceScore = CalculateDistanceScore(distanceResult.DistanceKilometers);
                    recommendation.DistanceScore = distanceScore.Score;
                    recommendation.DistanceMatchReason = distanceScore.Reason;
                }
                else
                {
                    recommendation.DistanceScore = 0;
                    recommendation.DistanceMatchReason = "Distance calculation failed";
                    recommendation.FormattedDistance = "Unknown";
                    recommendation.EstimatedDuration = "Unknown";
                }
            }

            _logger.LogInformation("Distance calculation completed for {ProcessedCount} recommendations", 
                recommendations.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating distance scores");
            
            // Set default values for all recommendations if distance calculation fails
            foreach (var recommendation in recommendations)
            {
                recommendation.DistanceScore = 0;
                recommendation.DistanceMatchReason = "Distance calculation unavailable";
                recommendation.FormattedDistance = "Unknown";
                recommendation.EstimatedDuration = "Unknown";
            }
        }
    }

    private static (int Score, string Reason) CalculateDistanceScore(double distanceKm)
    {
        // Distance scoring logic:
        // 0-2km: 4 points
        // 2-5km: 3 points
        // 5-10km: 2 points
        // >10km: 1 point

        return distanceKm switch
        {
            <= 2.0 => (4, $"Very close - {distanceKm:F1}km away"),
            <= 5.0 => (3, $"Close - {distanceKm:F1}km away"),
            <= 10.0 => (2, $"Moderate distance - {distanceKm:F1}km away"),
            _ => (1, $"Far - {distanceKm:F1}km away")
        };
    }
}