using Gigbuds_BE.Application.DTOs.Files;
using Microsoft.AspNetCore.Http;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Prepares a file for upload to Firebase Storage
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="folder">The folder path in storage</param>
    /// <param name="fileType">The type of file being uploaded</param>
    /// <returns>File upload result with URL and metadata</returns>
    Task<FileUploadResult> PrepareUploadFileAsync(IFormFile file, string folder, FileType fileType);

    /// <summary>
    /// Uploads a file stream to Firebase Storage
    /// </summary>
    /// <param name="request">File upload request with stream and metadata</param>
    /// <returns>File upload result with URL and metadata</returns>
    Task<FileUploadResult> UploadFileAsync(FileUploadRequest request);

    /// <summary>
    /// Prepares an image for upload to Firebase Storage
    /// </summary>
    /// <param name="file">The image file to upload</param>
    /// <param name="folder">The folder path in storage</param>
    /// <returns>File upload result with URL and metadata</returns>
    Task<FileUploadResult> PrepareUploadImageAsync(IFormFile file, string folder);

    /// <summary>
    /// Deletes a file from Firebase Storage
    /// </summary>
    /// <param name="filePath">The path of the file to delete</param>
    /// <returns>True if deletion was successful</returns>
    Task<bool> DeleteFileAsync(string filePath);

    /// <summary>
    /// Gets the download URL for a file
    /// </summary>
    /// <param name="filePath">The path of the file</param>
    /// <returns>The download URL</returns>
    Task<string?> GetFileUrlAsync(string filePath);

    /// <summary>
    /// Validates if the file type is allowed
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <param name="fileType">The expected file type</param>
    /// <returns>True if file is valid</returns>
    bool ValidateFile(IFormFile file, FileType fileType);

    /// <summary>
    /// Add download token to existing file that doesn't have one
    /// </summary>
    /// <param name="filePath">The path of the file</param>
    /// <returns>New download URL with token</returns>
    Task<string> AddDownloadTokenToExistingFile(string filePath);

    /// <summary>
    /// Generate custom Firebase Auth token for user authentication
    /// </summary>
    /// <param name="uid">User ID</param>
    /// <param name="customClaims">Optional custom claims</param>
    /// <returns>Custom auth token</returns>
    Task<string> GenerateCustomTokenAsync(string uid, Dictionary<string, object>? customClaims = null);
} 