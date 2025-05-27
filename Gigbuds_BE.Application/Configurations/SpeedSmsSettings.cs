using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.Configurations;

public class SpeedSmsSettings
{
    public const string SectionName = "SpeedSMS";

    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Type { get; set; } = 5; // Default to TYPE_GATEWAY

    [Required]
    public string DeviceId { get; set; } = string.Empty;

    [Required]
    public string RootUrl { get; set; } = "https://api.speedsms.vn/index.php"; //Default value if app settings is not set
} 