using System.Text;
using System.Text.Json;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.DTOs.Maps;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gigbuds_BE.Infrastructure.Services;

public class GoogleMapsService : IGoogleMapsService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleMapsSettings _settings;
    private readonly ILogger<GoogleMapsService> _logger;

    public GoogleMapsService(
        HttpClient httpClient,
        IOptions<GoogleMapsSettings> settings,
        ILogger<GoogleMapsService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.RequestTimeoutSeconds);
    }

    public async Task<List<DistanceResult>> CalculateDistancesToMultipleDestinationsAsync(
        string origin, 
        IEnumerable<string> destinations)
    {
        var destinationList = destinations.ToList();
        var results = new List<DistanceResult>();

        // Process in batches to respect API limits
        const int batchSize = 25; // Google Maps API limit
        
        for (int i = 0; i < destinationList.Count; i += batchSize)
        {
            var batch = destinationList.Skip(i).Take(batchSize).ToList();
            
            var request = new RouteMatrixRequest
            {
                Origins = new List<RouteMatrixOrigin>
                {
                    new RouteMatrixOrigin 
                    { 
                        Waypoint = new Waypoint { Address = origin }
                    }
                },
                Destinations = batch.Select(d => new RouteMatrixDestination 
                { 
                    Waypoint = new Waypoint { Address = d }
                }).ToList(),
                TravelMode = _settings.TravelMode,
                RoutingPreference = _settings.RoutingPreference,
                LanguageCode = _settings.LanguageCode,
                RegionCode = _settings.RegionCode,
                Units = _settings.Units
            };

            try
            {
                // For multiple destinations, we need to make the API call differently
                // Since the API returns an array of results
                var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var fieldMask = _settings.FieldMask;
                var requestUrl = $"{_settings.RouteMatrixEndpoint}?key={_settings.ApiKey}";
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", fieldMask);

                var response = await _httpClient.PostAsync(requestUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var matrixResults = JsonSerializer.Deserialize<RouteMatrixResponse[]>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (matrixResults != null)
                    {
                        foreach (var matrixResult in matrixResults)
                        {
                            var destinationIndex = matrixResult.DestinationIndex ?? 0;
                            var destination = destinationIndex < batch.Count ? batch[destinationIndex] : "Unknown";

                            results.Add(new DistanceResult
                            {
                                Origin = origin,
                                Destination = destination,
                                DistanceMeters = matrixResult.DistanceMeters ?? 0,
                                Duration = matrixResult.Duration,
                                LocalizedDistance = matrixResult.LocalizedValues?.Distance?.Text,
                                LocalizedDuration = matrixResult.LocalizedValues?.Duration?.Text,
                                IsSuccess = matrixResult.Status?.Code == 0,
                                ErrorMessage = matrixResult.Status?.Code == 0 ? null : matrixResult.Status?.Message
                            });
                        }
                    }
                }
                else
                {
                    _logger.LogError("Batch request failed with status {StatusCode}: {ResponseContent}", 
                        response.StatusCode, responseContent);
                        
                    // Add failed results for this batch
                    foreach (var destination in batch)
                    {
                        results.Add(new DistanceResult
                        {
                            Origin = origin,
                            Destination = destination,
                            IsSuccess = false,
                            ErrorMessage = $"API request failed: {response.StatusCode}"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing batch starting at index {BatchStartIndex}", i);
                
                // Add failed results for this batch
                foreach (var destination in batch)
                {
                    results.Add(new DistanceResult
                    {
                        Origin = origin,
                        Destination = destination,
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    });
                }
            }

            // Add a small delay between batches to be respectful to the API
            if (i + batchSize < destinationList.Count)
            {
                await Task.Delay(100);
            }
        }

        return results;
    }
} 