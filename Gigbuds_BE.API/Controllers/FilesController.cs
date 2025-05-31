using Gigbuds_BE.Application.DTOs.Files;
using Gigbuds_BE.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
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
    /// Upload a single file
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="folder">The folder to upload to (optional, defaults to 'uploads')</param>
    /// <param name="fileType">The type of file being uploaded</param>
    /// <returns>File upload result with URL</returns>
    [HttpPost("upload")]
    public async Task<ActionResult<FileUploadResult>> UploadFile(
        IFormFile file, 
        [FromQuery] string folder = "uploads", 
        [FromQuery] FileType fileType = FileType.Document)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file provided" });
            }

            var result = await _fileStorageService.UploadFileAsync(file, folder, fileType);
            
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Upload an image with optional resizing
    /// </summary>
    /// <param name="file">The image file to upload</param>
    /// <param name="folder">The folder to upload to (optional, defaults to 'images')</param>
    /// <param name="maxWidth">Maximum width for resizing (optional)</param>
    /// <param name="maxHeight">Maximum height for resizing (optional)</param>
    /// <returns>File upload result with URL</returns>
    [HttpPost("upload-image")]
    public async Task<ActionResult<FileUploadResult>> UploadImage(
        IFormFile file,
        [FromQuery] string folder = "images",
        [FromQuery] int? maxWidth = null,
        [FromQuery] int? maxHeight = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No image file provided" });
            }

            var result = await _fileStorageService.UploadImageAsync(file, folder, maxWidth, maxHeight);
            
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Upload multiple files
    /// </summary>
    /// <param name="files">The files to upload</param>
    /// <param name="folder">The folder to upload to (optional, defaults to 'uploads')</param>
    /// <param name="fileType">The type of files being uploaded</param>
    /// <returns>List of file upload results</returns>
    [HttpPost("upload-multiple")]
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
                    var result = await _fileStorageService.UploadFileAsync(file, folder, fileType);
                    results.Add(result);
                }
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading multiple files");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Upload avatar image
    /// </summary>
    /// <param name="file">The avatar image file</param>
    /// <returns>File upload result with URL</returns>
    [HttpPost("upload-avatar")]
    public async Task<ActionResult<FileUploadResult>> UploadAvatar(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No avatar image provided" });
            }

            // Resize avatar to standard size (300x300)
            var result = await _fileStorageService.UploadImageAsync(file, "avatars", 300, 300);
            
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading avatar");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Upload company logo
    /// </summary>
    /// <param name="file">The company logo image file</param>
    /// <returns>File upload result with URL</returns>
    [HttpPost("upload-company-logo")]
    public async Task<ActionResult<FileUploadResult>> UploadCompanyLogo(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No company logo provided" });
            }

            // Resize logo to standard size (500x200)
            var result = await _fileStorageService.UploadImageAsync(file, "company-logos", 500, 200);
            
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading company logo");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete a file
    /// </summary>
    /// <param name="filePath">The path of the file to delete</param>
    /// <returns>Success status</returns>
    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteFile([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(new { message = "File path is required" });
            }

            var success = await _fileStorageService.DeleteFileAsync(filePath);
            
            if (success)
            {
                return Ok(new { message = "File deleted successfully" });
            }
            else
            {
                return NotFound(new { message = "File not found or could not be deleted" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get file URL
    /// </summary>
    /// <param name="filePath">The path of the file</param>
    /// <returns>File URL</returns>
    [HttpGet("url")]
    public async Task<ActionResult<string>> GetFileUrl([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(new { message = "File path is required" });
            }

            var url = await _fileStorageService.GetFileUrlAsync(filePath);
            
            if (!string.IsNullOrEmpty(url))
            {
                return Ok(new { url });
            }
            else
            {
                return NotFound(new { message = "File not found" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file URL for {FilePath}", filePath);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
} 