using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationMode.Data;
using VacationMode.Models;

namespace VacationMode.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Renter")]
public class WishlistController : ControllerBase
{
    private readonly AppDbContext _context;

    public WishlistController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("add/{propertyId}")]
    public IActionResult AddToWishlist(int propertyId)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

        var property = _context.Properties.Find(propertyId);
        if (property == null) return NotFound("Property not found");

        var existing = _context.Wishlists.FirstOrDefault(w => w.UserId == userId && w.PropertyId == propertyId);
        if (existing != null) return BadRequest("Already in wishlist");

        var wishlistItem = new Wishlist
        {
            UserId = userId,
            PropertyId = propertyId,
            CreatedAt = DateTime.UtcNow,
            User = null!, 
            Property = null!
        };

        _context.Wishlists.Add(wishlistItem);
        _context.SaveChanges();
        return Ok(new { message = "Added to wishlist" });
    }

    [HttpDelete("remove/{propertyId}")]
    public IActionResult RemoveFromWishlist(int propertyId)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

        var existing = _context.Wishlists.FirstOrDefault(w => w.UserId == userId && w.PropertyId == propertyId);
        if (existing == null) return NotFound("Not in wishlist");

        _context.Wishlists.Remove(existing);
        _context.SaveChanges();
        return Ok(new { message = "Removed from wishlist" });
    }

    [HttpGet]
    public IActionResult GetMyWishlist()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

        var wishlist = _context.Wishlists
            .Include(w => w.Property)
                .ThenInclude(p => p.Owner)
            .Include(w => w.Property)
                .ThenInclude(p => p.Images)
            .Include(w => w.Property)
                .ThenInclude(p => p.Reviews)
            .Where(w => w.UserId == userId)
            .Select(w => new {
                w.WishlistId,
                w.PropertyId,
                Property = w.Property == null ? null : new {
                    w.Property.PropertyId,
                    w.Property.OwnerId,
                    OwnerName = w.Property.Owner != null ? w.Property.Owner.FullName : "Unknown",
                    w.Property.Title,
                    w.Property.City,
                    w.Property.State,
                    PricePerNight = (double)w.Property.PricePerNight,
                    Images = w.Property.Images != null ? w.Property.Images.Select(i => i.ImageUrl).ToList() : new List<string>(),
                    Rating = (w.Property.Reviews != null && w.Property.Reviews.Any()) ? w.Property.Reviews.Average(r => r.Rating) : 0
                }
            })
            .ToList();

        return Ok(wishlist);
    }
}
