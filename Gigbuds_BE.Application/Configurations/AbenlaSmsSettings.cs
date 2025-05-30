using System.ComponentModel.DataAnnotations;

namespace Gigbuds_BE.Application.Configurations;

public class AbenlaSmsSettings
{
    public const string SectionName = "AbenlaSMS";

    [Required]
    public string LoginName { get; set; } = string.Empty;

    [Required]
    public string SendSmsPassword { get; set; } = string.Empty;

    [Required]
    public int ServiceTypeId { get; set; } = 535;

    [Required]
    public string BrandName { get; set; } = "Verify3";

    [Required]
    public string RootUrl { get; set; } = "https://api.abenla.com/api";

    public bool CallBack { get; set; } = false;
} 