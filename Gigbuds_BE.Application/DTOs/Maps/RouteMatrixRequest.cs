using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.DTOs.Maps;

public class RouteMatrixRequest
{
    [JsonPropertyName("origins")]
    public List<RouteMatrixOrigin> Origins { get; set; } = new();

    [JsonPropertyName("destinations")]
    public List<RouteMatrixDestination> Destinations { get; set; } = new();

    [JsonPropertyName("travelMode")]
    public string TravelMode { get; set; } = string.Empty;

    [JsonPropertyName("routingPreference")]
    public string RoutingPreference { get; set; } = string.Empty;

    [JsonPropertyName("languageCode")]
    public string LanguageCode { get; set; } = string.Empty;

    [JsonPropertyName("regionCode")]
    public string RegionCode { get; set; } = string.Empty;

    [JsonPropertyName("units")]
    public string Units { get; set; } = string.Empty;
}

public class RouteMatrixOrigin
{
    [JsonPropertyName("waypoint")]
    public Waypoint Waypoint { get; set; } = new();
}

public class RouteMatrixDestination
{
    [JsonPropertyName("waypoint")]
    public Waypoint Waypoint { get; set; } = new();
}

public class Waypoint
{
    [JsonPropertyName("address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Address { get; set; }

    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WaypointLocation? Location { get; set; }
}

public class WaypointLocation
{
    [JsonPropertyName("latLng")]
    public LatLng LatLng { get; set; } = new();
}

public class LatLng
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
} 