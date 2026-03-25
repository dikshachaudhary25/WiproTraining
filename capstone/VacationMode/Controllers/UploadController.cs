using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationMode.Data;
using VacationMode.Models;

namespace VacationMode.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public UploadController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    
    
    
    [Authorize(Roles = "Owner")]
    [HttpPost("property/{propertyId}")]
    public async Task<IActionResult> UploadPropertyImages(int propertyId, [FromForm] List<IFormFile> images)
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null)
            return NotFound("Property not found.");

        if (property.OwnerId != ownerId)
            return Forbid();

        if (images == null || images.Count == 0)
            return BadRequest("No images provided.");

        if (images.Count > 5)
            return BadRequest("You can upload a maximum of 5 images per property.");

        
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "properties", propertyId.ToString());
        Directory.CreateDirectory(uploadsFolder);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var savedUrls = new List<string>();

        foreach (var file in images)
        {
            if (file.Length == 0)
                continue;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest($"File '{file.FileName}' has an unsupported extension. Allowed: jpg, jpeg, png, webp, gif.");

            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativeUrl = $"/uploads/properties/{propertyId}/{uniqueFileName}";
            savedUrls.Add(relativeUrl);

            var isFirst = !_context.PropertyImages.Any(i => i.PropertyId == propertyId);
            _context.PropertyImages.Add(new PropertyImage
            {
                PropertyId = propertyId,
                ImageUrl = relativeUrl,
                IsPrimary = isFirst && savedUrls.Count == 1
            });
        }

        await _context.SaveChangesAsync();

        return Ok(new { uploaded = savedUrls.Count, urls = savedUrls });
    }

    
    
    
    [Authorize(Roles = "Owner")]
    [HttpDelete("property-image/{imageId}")]
    public async Task<IActionResult> DeletePropertyImage(int imageId)
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var image = await _context.PropertyImages.FindAsync(imageId);
        if (image == null)
            return NotFound("Image not found.");

        var property = await _context.Properties.FindAsync(image.PropertyId);
        if (property == null || property.OwnerId != ownerId)
            return Forbid();

        
        var filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        _context.PropertyImages.Remove(image);
        await _context.SaveChangesAsync();

        return Ok("Image deleted.");
    }
}
