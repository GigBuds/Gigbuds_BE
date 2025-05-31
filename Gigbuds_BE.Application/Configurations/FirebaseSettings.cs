using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gigbuds_BE.Application.Configurations;

public class FirebaseSettings
{
    public const string SectionName = "Firebase";

    [Required]
    public string ProjectId { get; set; } = string.Empty;

    [Required]
    public string StorageBucket { get; set; } = string.Empty;

    [Required]
    [JsonIgnore]
    public string ServiceAccountJson { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = "https://firebasestorage.googleapis.com/v0/b";

    public int MaxFileSizeMB { get; set; } = 10;

    public string[] AllowedImageExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public string[] AllowedDocumentExtensions { get; set; } = { ".pdf", ".doc", ".docx", ".txt" };
} 