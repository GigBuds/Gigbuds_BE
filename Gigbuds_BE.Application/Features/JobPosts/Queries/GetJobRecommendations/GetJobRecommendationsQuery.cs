using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.DTOs.JobRecommendations;
using MediatR;
using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.Features.JobPosts.Queries.GetJobRecommendations;

public class GetJobRecommendationsQuery : IRequest<PagedResultDto<JobRecommendationDto>>
{
    [JsonIgnore]
    public int JobSeekerId { get; set; }

    public string CurrentLocation { get; set; } = string.Empty;

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public bool IncludeScheduleMatching { get; set; } = true;

    public bool IncludeDistanceCalculation { get; set; } = true;
}