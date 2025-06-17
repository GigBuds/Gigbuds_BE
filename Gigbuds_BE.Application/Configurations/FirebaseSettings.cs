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
    public string ServiceAccountJson { get; set; } = string.Empty;

    // Optional: Keep for local development if you prefer file-based approach
    public string? ServiceAccountKeyPath { get; set; }

    public string? NotificationServiceAccountKeyPath { get; set;  }

    // Service Account JSON properties - add these
    // public ServiceAccount? ServiceAccount { get; set; }

    public string BaseUrl { get; set; } = "https://firebasestorage.googleapis.com/v0/b";

    public int MaxFileSizeMB { get; set; } = 10;

    public string[] AllowedImageExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public string[] AllowedDocumentExtensions { get; set; } = { ".pdf", ".doc", ".docx", ".txt" };

    public string[] AllowedJobAttachmentExtensions { get; set; } = { ".pdf", ".doc", ".docx" };
}

public class ServiceAccount
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; } = string.Empty;

    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId { get; set; } = string.Empty;

    [JsonPropertyName("private_key")]
    public string PrivateKey { get; set; } = string.Empty;

    [JsonPropertyName("client_email")]
    public string ClientEmail { get; set; } = string.Empty;

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("auth_uri")]
    public string AuthUri { get; set; } = string.Empty;

    [JsonPropertyName("token_uri")]
    public string TokenUri { get; set; } = string.Empty;

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProviderX509CertUrl { get; set; } = string.Empty;

    [JsonPropertyName("client_x509_cert_url")]
    public string ClientX509CertUrl { get; set; } = string.Empty;

    [JsonPropertyName("universe_domain")]
    public string UniverseDomain { get; set; } = string.Empty;
} 