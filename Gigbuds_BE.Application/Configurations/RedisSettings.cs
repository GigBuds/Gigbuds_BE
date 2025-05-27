using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.Configurations;

public class RedisSettings
{
    public const string SectionName = "Redis";

    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    [Range(1, 60)]
    public int VerificationCodeExpirationMinutes { get; set; } = 5;

    [Required]
    public string VerificationCodePrefix { get; set; } = string.Empty;
} 