using System.Text;
using Gigbuds_BE.Application.Configurations;
using Gigbuds_BE.Application.DTOs.Files;
using Gigbuds_BE.Application.Interfaces.Services;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace Gigbuds_BE.Infrastructure.Services;

public class FirebaseStorageService : IFileStorageService
{
    private readonly FirebaseSettings _settings;
    private readonly ILogger<FirebaseStorageService> _logger;
    private readonly StorageClient _storageClient;

    public FirebaseStorageService(IOptions<FirebaseSettings> settings, ILogger<FirebaseStorageService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        
        var tempFilePath = Path.GetTempFileName();
        // File.WriteAllText(tempFilePath, _settings.ServiceAccountKeyPath);
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempFilePath);
        _storageClient = StorageClient.Create();
    }

    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder, FileType fileType)
    {
        try
        {
            if (!ValidateFile(file, fileType))
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = "Invalid file type or size"
                };
            }

            var fileName = GenerateUniqueFileName(file.FileName);
            var filePath = $"{folder}/{fileName}";

            using var stream = file.OpenReadStream();
            var request = new FileUploadRequest
            {
                FileName = fileName,
                FileStream = stream,
                ContentType = file.ContentType,
                Folder = folder,
                IsImage = IsImageFile(file.ContentType)
            };

            return await UploadFileAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
            return new FileUploadResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<FileUploadResult> UploadFileAsync(FileUploadRequest request)
    {
        try
        {
            var filePath = $"{request.Folder}/{request.FileName}";
            
            var objectOptions = new Google.Apis.Storage.v1.Data.Object
            {
                Bucket = _settings.StorageBucket,
                Name = filePath,
                ContentType = request.ContentType
            };

            // Upload to Firebase Storage
            var uploadedObject = await _storageClient.UploadObjectAsync(
                objectOptions,
                request.FileStream);

            // Generate public URL
            var fileUrl = $"{_settings.BaseUrl}/{_settings.StorageBucket}/o/{Uri.EscapeDataString(filePath)}?alt=media";

            _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);

            return new FileUploadResult
            {
                Success = true,
                FileUrl = fileUrl,
                FileName = request.FileName,
                FilePath = filePath,
                FileSize = request.FileStream.Length,
                ContentType = request.ContentType
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file stream {FileName}", request.FileName);
            return new FileUploadResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<FileUploadResult> UploadImageAsync(IFormFile file, string folder, int? maxWidth = null, int? maxHeight = null)
    {
        try
        {
            if (!ValidateFile(file, FileType.Image))
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = "Invalid image file"
                };
            }

            var fileName = GenerateUniqueFileName(file.FileName);
            var filePath = $"{folder}/{fileName}";

            using var originalStream = file.OpenReadStream();
            
            // If resizing is requested, process the image
            if (maxWidth.HasValue || maxHeight.HasValue)
            {
                using var image = await Image.LoadAsync(originalStream);
                
                if (maxWidth.HasValue || maxHeight.HasValue)
                {
                    var resizeOptions = new ResizeOptions
                    {
                        Size = new Size(maxWidth ?? image.Width, maxHeight ?? image.Height),
                        Mode = ResizeMode.Max
                    };
                    
                    image.Mutate(x => x.Resize(resizeOptions));
                }

                using var processedStream = new MemoryStream();
                
                // Save in appropriate format
                var extension = Path.GetExtension(fileName).ToLowerInvariant();
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                        await image.SaveAsJpegAsync(processedStream, new JpegEncoder { Quality = 85 });
                        break;
                    case ".png":
                        await image.SaveAsPngAsync(processedStream);
                        break;
                    case ".webp":
                        await image.SaveAsWebpAsync(processedStream);
                        break;
                    default:
                        await image.SaveAsJpegAsync(processedStream, new JpegEncoder { Quality = 85 });
                        break;
                }

                processedStream.Position = 0;

                var request = new FileUploadRequest
                {
                    FileName = fileName,
                    FileStream = processedStream,
                    ContentType = file.ContentType,
                    Folder = folder,
                    IsImage = true
                };

                return await UploadFileAsync(request);
            }
            else
            {
                // Upload original image without processing
                var request = new FileUploadRequest
                {
                    FileName = fileName,
                    FileStream = originalStream,
                    ContentType = file.ContentType,
                    Folder = folder,
                    IsImage = true
                };

                return await UploadFileAsync(request);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image {FileName}", file.FileName);
            return new FileUploadResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            await _storageClient.DeleteObjectAsync(_settings.StorageBucket, filePath);
            _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            return false;
        }
    }

    public async Task<string?> GetFileUrlAsync(string filePath)
    {
        try
        {
            var obj = await _storageClient.GetObjectAsync(_settings.StorageBucket, filePath);
            if (obj != null)
            {
                return $"{_settings.BaseUrl}/{_settings.StorageBucket}/o/{Uri.EscapeDataString(filePath)}?alt=media";
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file URL for {FilePath}", filePath);
            return null;
        }
    }

    public bool ValidateFile(IFormFile file, FileType fileType)
    {
        if (file == null || file.Length == 0)
        {
            return false;
        }

        // Check file size
        var maxSizeBytes = _settings.MaxFileSizeMB * 1024 * 1024;
        if (file.Length > maxSizeBytes)
        {
            return false;
        }

        // Check file extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return fileType switch
        {
            FileType.Image or FileType.Avatar or FileType.CompanyLogo => 
                _settings.AllowedImageExtensions.Contains(extension),
            FileType.Document or FileType.JobAttachment => 
                _settings.AllowedDocumentExtensions.Contains(extension) || 
                _settings.AllowedImageExtensions.Contains(extension),
            _ => false
        };
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var guid = Guid.NewGuid().ToString("N")[..8];
        
        return $"{nameWithoutExtension}_{timestamp}_{guid}{extension}";
    }

    private static bool IsImageFile(string contentType)
    {
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
} 