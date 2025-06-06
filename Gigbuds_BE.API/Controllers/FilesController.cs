using Gigbuds_BE.Application.DTOs.Files;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[AllowAnonymous]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IFileStorageService fileStorageService, ILogger<FilesController> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a single file to any folder
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="folder">The folder to upload to (e.g., 'uploads', 'images', 'avatars', 'company-logos')</param>
    /// <param name="fileType">The type of file being uploaded</param>
    /// <returns>File upload result with URL</returns>
    [HttpPost("upload")]
    [RequestSizeLimit(10_000_000)] // 10MB limit
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(FileUploadResult), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromQuery] FolderType folder = FolderType.Files,
        [FromQuery] FileType fileType = FileType.Document)
    {
        _logger.LogInformation("Starting upload for file: {FileName}, Content-Type: {ContentType}, Size: {Size} to folder: {Folder}",
            file.FileName, file.ContentType, file.Length, folder);
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file provided" });
            }

            FileUploadResult result;

            // Handle image files (avatars, company logos, general images)
            if (IsImageFile(file.ContentType))
            {
                result = await _fileStorageService.PrepareUploadImageAsync(file, folder.ToString());
            }
            else
            {
                // Handle documents and other files
                result = await _fileStorageService.PrepareUploadFileAsync(file, folder.ToString(), fileType);
            }
            
            if (!result.Success)
            {
                return BadRequest(new { error = result.ErrorMessage });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to folder {Folder}", folder);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Upload multiple files to the same folder
    /// </summary>
    /// <param name="files">The files to upload</param>
    /// <param name="folder">The folder to upload to (optional, defaults to 'uploads')</param>
    /// <param name="fileType">The type of files being uploaded</param>
    /// <returns>List of file upload results</returns>
    [HttpPost("upload-multiple")]
    [RequestSizeLimit(50_000_000)] // 50MB limit for multiple files
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(List<FileUploadResult>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<List<FileUploadResult>>> UploadMultipleFiles(
        List<IFormFile> files,
        [FromQuery] string folder = "uploads",
        [FromQuery] FileType fileType = FileType.Document)
    {
        try
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { message = "No files provided" });
            }

            var results = new List<FileUploadResult>();
            
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    FileUploadResult result;

                    // Handle image files
                    if (IsImageFile(file.ContentType) || 
                        folder.Equals("avatars", StringComparison.OrdinalIgnoreCase) ||
                        folder.Equals("company-logos", StringComparison.OrdinalIgnoreCase) ||
                        folder.Equals("images", StringComparison.OrdinalIgnoreCase))
                    {
                        result = await _fileStorageService.PrepareUploadImageAsync(file, folder);
                    }
                    else
                    {
                        result = await _fileStorageService.PrepareUploadFileAsync(file, folder, fileType);
                    }
                    
                    results.Add(result);
                }
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading multiple files to folder {Folder}", folder);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete a file
    /// </summary>
    /// <param name="filePath">The path of the file to delete</param>
    /// <returns>Success status</returns>
    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteFile([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(new { error = "File path is required" });
            }

            var success = await _fileStorageService.DeleteFileAsync(filePath);
            
            if (!success)
            {
                return NotFound(new { error = "File not found or could not be deleted" });
            }

            return Ok(new { message = "File deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Helper method to check if a file is an image
    /// </summary>
    /// <param name="contentType">The content type of the file</param>
    /// <returns>True if the file is an image</returns>
    private static bool IsImageFile(string contentType)
    {
        return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
}