using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.JobRecommendations;
using Gigbuds_BE.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobRecommendations;

public class GetJobRecommendationsQueryHandler : IRequestHandler<GetJobRecommendationsQuery, PagedResultDto<JobRecommendationDto>>
{
    private readonly IJobRecommendationService _jobRecommendationService;
    private readonly ILogger<GetJobRecommendationsQueryHandler> _logger;

    public GetJobRecommendationsQueryHandler(
        IJobRecommendationService jobRecommendationService,
        ILogger<GetJobRecommendationsQueryHandler> logger)
    {
        _jobRecommendationService = jobRecommendationService;
        _logger = logger;
    }

    public async Task<PagedResultDto<JobRecommendationDto>> Handle(
        GetJobRecommendationsQuery request,
        CancellationToken cancellationToken)
    {

        if (string.IsNullOrWhiteSpace(request.CurrentLocation))
        {
            _logger.LogWarning("Current location is required for job recommendations");
            throw new ArgumentException("Current location is required for job recommendations");
        }

        // Get all recommendations first (we'll implement proper pagination in the service later)
        var allRecommendations = await _jobRecommendationService.GetRecommendedJobsWithScheduleAndDistanceAsync(
            request.JobSeekerId,
            request.CurrentLocation,
            1000); // Get a large number to ensure we get all possible recommendations

        var totalCount = allRecommendations.Count;
        var skip = (request.PageIndex - 1) * request.PageSize;
        var pagedRecommendations = allRecommendations
            .Skip(skip)
            .Take(request.PageSize)
            .ToList();

        _logger.LogInformation("Successfully retrieved {Count} job recommendations (page {PageIndex} of {TotalCount} total) for JobSeeker {JobSeekerId}",
            pagedRecommendations.Count, request.PageIndex, totalCount, request.JobSeekerId);

        return new PagedResultDto<JobRecommendationDto>(totalCount, pagedRecommendations);
    }
}