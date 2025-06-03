using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.Configurations;

public class GoogleMapsSettings
{
    public const string SectionName = "GoogleMaps";

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    public string BaseUrl { get; set; } = "https://routes.googleapis.com";

    [Required]
    public string RouteMatrixEndpoint { get; set; } = "/distanceMatrix/v2:computeRouteMatrix";

    [Range(10, 300)]
    public int RequestTimeoutSeconds { get; set; } = 30;

    [Range(1, 25)]
    public int MaxOrigins { get; set; } = 25;

    [Range(1, 25)]
    public int MaxDestinations { get; set; } = 25;

    public string FieldMask { get; set; } = "originIndex,destinationIndex,status,condition,distanceMeters,duration,localizedValues.distance,localizedValues.duration";

    public string TravelMode { get; set; } = "TWO_WHEELER";

    public string RoutingPreference { get; set; } = "TRAFFIC_UNAWARE";

    public string LanguageCode { get; set; } = "en";

    public string RegionCode { get; set; } = "VN";

    public string Units { get; set; } = "METRIC";
} 