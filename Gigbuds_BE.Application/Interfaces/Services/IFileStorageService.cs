using Gigbuds_BE.Application.DTOs.Files;
using Microsoft.AspNetCore.Http;

namespace Gigbuds_BE.Application.Interfaces.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to Firebase Storage
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="folder">The folder path in storage</param>
    /// <param name="fileType">The type of file being uploaded</param>
    /// <returns>File upload result with URL and metadata</returns>
    Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder, FileType fileType);

    /// <summary>
    /// Uploads a file stream to Firebase Storage
    /// </summary>
    /// <param name="request">File upload request with stream and metadata</param>
    /// <returns>File upload result with URL and metadata</returns>
    Task<FileUploadResult> UploadFileAsync(FileUploadRequest request);

    /// <summary>
    /// Uploads an image with optional resizing
    /// </summary>
    /// <param name="file">The image file to upload</param>
    /// <param name="folder">The folder path in storage</param>
    /// <param name="maxWidth">Maximum width for resizing (optional)</param>
    /// <param name="maxHeight">Maximum height for resizing (optional)</param>
    /// <returns>File upload result with URL and metadata</returns>
    Task<FileUploadResult> UploadImageAsync(IFormFile file, string folder, int? maxWidth = null, int? maxHeight = null);

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
} 