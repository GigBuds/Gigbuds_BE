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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;

namespace Gigbuds_BE.Infrastructure.Services;

public class FirebaseStorageService : IFileStorageService
{
    private readonly FirebaseSettings _settings;
    private readonly ILogger<FirebaseStorageService> _logger;
    private readonly StorageClient _storageClient;

    public FirebaseStorageService(IOptions<FirebaseSettings> settings, ILogger<FirebaseStorageService> logger, IWebHostEnvironment environment)
    {
        _settings = settings.Value;
        _logger = logger;
        
        // Initialize Firebase Admin SDK
        // InitializeFirebaseAdmin(environment);

        if (environment.IsDevelopment())
        {
           //Development environment
           if (File.Exists(_settings.ServiceAccountKeyPath))
           {

               Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _settings.ServiceAccountKeyPath);

               _logger.LogInformation("Using Firebase credentials from file: {Path}", _settings.ServiceAccountKeyPath);
           }
           else
           {

               _logger.LogWarning("Firebase credentials file not found at: {Path}", _settings.ServiceAccountKeyPath);
        
           }
        }
        else
        {
           //Production environment
           var serviceAccountJson = _settings.ServiceAccountJson;
            
            if (!string.IsNullOrEmpty(serviceAccountJson))
            {
                // Create a temporary file with the JSON content
                var tempFilePath = Path.Combine(Path.GetTempPath(), $"firebase-service-account-{Guid.NewGuid()}.json");
                File.WriteAllText(tempFilePath, serviceAccountJson);
                
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempFilePath);
                _logger.LogInformation("Using Firebase credentials from temporary file created from appsettings");
            }
        }

        _storageClient = StorageClient.Create();
    }

    private void InitializeFirebaseAdmin(IWebHostEnvironment environment)
    {
        try
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                GoogleCredential credential;
                
                if (environment.IsDevelopment())
                {
                    //credential = GoogleCredential.FromFile(_settings.ServiceAccountKeyPath);
                    //_logger.LogInformation("Using Firebase credentials from file: {Path}", _settings.ServiceAccountKeyPath);
                    var serviceAccountJson = _settings.ServiceAccountJson;
                    credential = GoogleCredential.FromJson(serviceAccountJson);
                    _logger.LogInformation("Using Firebase credentials from appsettings.json");
                }
                else
                {
                    credential = GoogleCredential.FromFile(_settings.ServiceAccountKeyPath);
                    _logger.LogInformation("Using Firebase credentials from file: {Path}", _settings.ServiceAccountKeyPath);
                }

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential,
                    ProjectId = _settings.ProjectId
                });
                
                _logger.LogInformation("Firebase Admin SDK initialized successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Firebase Admin SDK");
            throw;
        }
    }

    public async Task<FileUploadResult> PrepareUploadFileAsync(IFormFile file, string folder, FileType fileType)
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
            
            // Generate a unique download token for Firebase-style URLs
            var downloadToken = Guid.NewGuid().ToString();
            
            var objectOptions = new Google.Apis.Storage.v1.Data.Object
            {
                Bucket = _settings.StorageBucket,
                Name = filePath,
                ContentType = request.ContentType,
                Metadata = new Dictionary<string, string>
                {
                    ["firebaseStorageDownloadTokens"] = downloadToken
                }
            };

            // Upload to Firebase Storage
            var uploadedObject = await _storageClient.UploadObjectAsync(
                objectOptions,
                request.FileStream);

            // Generate Firebase-style download URL with token
            string fileUrl = GenerateFirebaseDownloadUrl(filePath, downloadToken);

            _logger.LogInformation("File uploaded successfully: {FilePath} with download token", filePath);

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

    public async Task<FileUploadResult> PrepareUploadImageAsync(IFormFile file, string folder)
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
            return await GetDownloadUrlAsync(filePath);
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
            FileType.Document  => 
                _settings.AllowedDocumentExtensions.Contains(extension) || 
                _settings.AllowedImageExtensions.Contains(extension),
            FileType.JobAttachment => 
                _settings.AllowedJobAttachmentExtensions.Contains(extension),
            _ => false
        };
    }

    /// <summary>
    /// Generate Firebase-style download URL with access token
    /// </summary>
    /// <param name="filePath">The path of the file in storage</param>
    /// <param name="downloadToken">The download token for the file</param>
    /// <returns>Firebase-style download URL</returns>
    private string GenerateFirebaseDownloadUrl(string filePath, string downloadToken)
    {
        var encodedPath = Uri.EscapeDataString(filePath);
        return $"https://firebasestorage.googleapis.com/v0/b/{_settings.StorageBucket}/o/{encodedPath}?alt=media&token={downloadToken}";
    }

    /// <summary>
    /// Get download URL with token from existing file
    /// </summary>
    /// <param name="filePath">The path of the file in storage</param>
    /// <returns>Download URL with access token</returns>
    private async Task<string> GetDownloadUrlAsync(string filePath)
    {
        try
        {
            // Get the object to retrieve its metadata (including the download token)
            var obj = await _storageClient.GetObjectAsync(_settings.StorageBucket, filePath);
            
            if (obj != null && obj.Metadata != null && 
                obj.Metadata.TryGetValue("firebaseStorageDownloadTokens", out var downloadToken))
            {
                // Use the stored download token
                return GenerateFirebaseDownloadUrl(filePath, downloadToken);
            }
            
            // Fallback: Generate a new token and update the object for existing files
            var newToken = Guid.NewGuid().ToString();
            await UpdateObjectWithDownloadToken(filePath, newToken);
            return GenerateFirebaseDownloadUrl(filePath, newToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating download URL for {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Update existing object with download token
    /// </summary>
    /// <param name="filePath">The path of the file in storage</param>
    /// <param name="downloadToken">The download token to add</param>
    private async Task UpdateObjectWithDownloadToken(string filePath, string downloadToken)
    {
        try
        {
            var obj = await _storageClient.GetObjectAsync(_settings.StorageBucket, filePath);
            
            if (obj.Metadata == null)
                obj.Metadata = new Dictionary<string, string>();
                
            obj.Metadata["firebaseStorageDownloadTokens"] = downloadToken;
            
            await _storageClient.UpdateObjectAsync(obj);
            
            _logger.LogInformation("Updated download token for existing file: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating download token for {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Add download token to existing file (utility method)
    /// </summary>
    /// <param name="filePath">The path of the file in storage</param>
    /// <returns>New download URL with token</returns>
    public async Task<string> AddDownloadTokenToExistingFile(string filePath)
    {
        try
        {
            var downloadToken = Guid.NewGuid().ToString();
            await UpdateObjectWithDownloadToken(filePath, downloadToken);
            return GenerateFirebaseDownloadUrl(filePath, downloadToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding download token to existing file {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// Generate custom Firebase Auth token (if needed for client-side authentication)
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <param name="customClaims">Optional custom claims</param>
    /// <returns>Custom auth token</returns>
    public async Task<string> GenerateCustomTokenAsync(string uid, Dictionary<string, object>? customClaims = null)
    {
        try
        {
            var auth = FirebaseAuth.DefaultInstance;
            return await auth.CreateCustomTokenAsync(uid, customClaims);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating custom token for user {Uid}", uid);
            throw;
        }
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