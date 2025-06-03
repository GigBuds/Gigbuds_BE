using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.DTOs.Maps;

public class RouteMatrixResponse
{
    [JsonPropertyName("originIndex")]
    public int? OriginIndex { get; set; }

    [JsonPropertyName("destinationIndex")]
    public int? DestinationIndex { get; set; }

    [JsonPropertyName("status")]
    public RouteMatrixStatus? Status { get; set; }

    [JsonPropertyName("condition")]
    public string? Condition { get; set; }

    [JsonPropertyName("distanceMeters")]
    public int? DistanceMeters { get; set; }

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }

    [JsonPropertyName("staticDuration")]
    public string? StaticDuration { get; set; }

    [JsonPropertyName("localizedValues")]
    public LocalizedValues? LocalizedValues { get; set; }
}

public class RouteMatrixStatus
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

public class LocalizedValues
{
    [JsonPropertyName("distance")]
    public LocalizedText? Distance { get; set; }

    [JsonPropertyName("duration")]
    public LocalizedText? Duration { get; set; }

    [JsonPropertyName("staticDuration")]
    public LocalizedText? StaticDuration { get; set; }
}

public class LocalizedText
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("languageCode")]
    public string? LanguageCode { get; set; }
}

public class DistanceResult
{
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public int DistanceMeters { get; set; }
    public double DistanceKilometers => DistanceMeters / 1000.0;
    public string? Duration { get; set; }
    public string? LocalizedDistance { get; set; }
    public string? LocalizedDuration { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
} 