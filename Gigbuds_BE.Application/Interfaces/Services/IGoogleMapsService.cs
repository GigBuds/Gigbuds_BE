using Gigbuds_BE.Application.DTOs.Maps;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IGoogleMapsService
{
    Task<List<DistanceResult>> CalculateDistancesToMultipleDestinationsAsync(string origin, IEnumerable<string> destinations);
} 