using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnlEngineering.Core.DTOs;
using SnlEngineering.Core.Interfaces;

namespace SnlEngineering.Api.Controllers;

[ApiController]
[Route("api/admin/upload")]
[Authorize(Policy = "AdminOnly")]
public class UploadController : ControllerBase
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".doc", ".docx"
    };

    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    private readonly IFileStorageService _storage;

    public UploadController(IFileStorageService storage) => _storage = storage;

    [HttpPost("{folder}")]
    [RequestSizeLimit(MaxFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    public async Task<IActionResult> Upload(string folder, IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new ApiResponse<string>(false, null, "No file uploaded"));

        if (file.Length > MaxFileSize)
            return BadRequest(new ApiResponse<string>(false, null, "File exceeds 10 MB limit"));

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            return BadRequest(new ApiResponse<string>(false, null, $"File type '{extension}' is not allowed"));

        var safeFolder = string.Join("", folder.Where(char.IsLetterOrDigit));
        if (string.IsNullOrEmpty(safeFolder))
            return BadRequest(new ApiResponse<string>(false, null, "Invalid folder name"));

        var path = await _storage.SaveFileAsync(file.OpenReadStream(), file.FileName, safeFolder);
        return Ok(new ApiResponse<string>(true, path, "File uploaded successfully"));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return BadRequest(new ApiResponse<bool>(false, false, "Path is required"));

        var deleted = await _storage.DeleteFileAsync(path);
        return deleted
            ? Ok(new ApiResponse<bool>(true, true, "File deleted"))
            : NotFound(new ApiResponse<bool>(false, false, "File not found"));
    }
}
