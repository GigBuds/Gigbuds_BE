using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.Configurations;

public class SpeedSmsSettings
{
    public const string SectionName = "SpeedSMS";

    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Type { get; set; } = 5; // Default to TYPE_GATEWAY

    public string DeviceId { get; set; } = string.Empty;
} 