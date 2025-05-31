namespace Gigbuds_BE.Application.DTOs.Files;

public class FileUploadResult
{
    public bool Success { get; set; }
    public string? FileUrl { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public long FileSize { get; set; }
    public string? ContentType { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

public class FileUploadRequest
{
    public required string FileName { get; set; }
    public required Stream FileStream { get; set; }
    public required string ContentType { get; set; }
    public string? Folder { get; set; } = "uploads";
    public bool IsImage { get; set; }
}

public enum FileType
{
    Image,
    Document,
    Avatar,
    CompanyLogo,
    JobAttachment
} 