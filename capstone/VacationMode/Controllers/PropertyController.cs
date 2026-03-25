using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationMode.Data;
using VacationMode.DTOs;
using VacationMode.Models;

namespace VacationMode.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<PropertyController> _logger;

    public PropertyController(AppDbContext context, ILogger<PropertyController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize(Roles = "Owner")]
    [HttpPost]
    public IActionResult CreateProperty(PropertyCreateDto dto)
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var property = new Property
        {
            OwnerId = ownerId,
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PropertyType = dto.PropertyType,
            PricePerNight = dto.PricePerNight,
            MaxGuests = dto.MaxGuests
        };

        _context.Properties.Add(property);
        _context.SaveChanges();

        return Ok(property);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut("{id}")]
    public IActionResult UpdateProperty(int id, PropertyUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var property = _context.Properties.Find(id);

        if (property == null)
            return NotFound("Property not found");

        if (property.OwnerId != userId)
            return Forbid();

        property.Title = dto.Title;
        property.Description = dto.Description;
        property.Location = dto.Location;
        property.City = dto.City;
        property.State = dto.State;
        property.Country = dto.Country;
        property.PropertyType = dto.PropertyType;
        property.PricePerNight = dto.PricePerNight;
        property.MaxGuests = dto.MaxGuests;

        _context.SaveChanges();

        return Ok(property);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAllProperties()
    {
        var properties = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.Owner)
            .Include(p => p.Reviews)
            .Select(p => new
            {
                p.PropertyId,
                p.OwnerId,
                OwnerName = p.Owner != null ? p.Owner.FullName : "Unknown",
                p.Title,
                p.Description,
                p.Location,
                p.City,
                p.State,
                p.Country,
                p.PropertyType,
                PricePerNight = (double)p.PricePerNight,
                p.MaxGuests,
                p.CreatedAt,
                Images = p.Images != null ? p.Images.Select(i => i.ImageUrl).ToList() : new List<string>(),
                Rating = (p.Reviews != null && p.Reviews.Any()) ? p.Reviews.Average(r => r.Rating) : 0
            })
            .ToList();

        return Ok(properties);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetPropertyById(int id)
    {
        var property = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.Owner)
            .Include(p => p.Reviews)
            .Where(p => p.PropertyId == id)
            .Select(p => new
            {
                p.PropertyId,
                p.OwnerId,
                OwnerName = p.Owner != null ? p.Owner.FullName : "Unknown",
                p.Title,
                p.Description,
                p.Location,
                p.City,
                p.State,
                p.Country,
                p.PropertyType,
                PricePerNight = (double)p.PricePerNight,
                p.MaxGuests,
                p.CreatedAt,
                Images = p.Images != null ? p.Images.Select(i => i.ImageUrl).ToList() : new List<string>(),
                Rating = (p.Reviews != null && p.Reviews.Any()) ? p.Reviews.Average(r => r.Rating) : 0
            })
            .FirstOrDefault();

        if (property == null)
            return NotFound("Property not found");

        return Ok(property);
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public IActionResult SearchProperties(
        [FromQuery] string? city, 
        [FromQuery] string? type, 
        [FromQuery] decimal? minPrice, 
        [FromQuery] decimal? maxPrice,
        [FromQuery] DateTime? checkIn,
        [FromQuery] DateTime? checkOut,
        [FromQuery] string? features)
    {
        _logger.LogInformation($"Filters received: city={city}, type={type}, minPrice={minPrice}, maxPrice={maxPrice}, checkIn={checkIn}, checkOut={checkOut}, features={features}");

        if (string.IsNullOrWhiteSpace(city) && 
            string.IsNullOrWhiteSpace(type) && 
            !minPrice.HasValue && 
            !maxPrice.HasValue && 
            (!checkIn.HasValue || !checkOut.HasValue) && 
            string.IsNullOrWhiteSpace(features))
        {
            return GetAllProperties();
        }

        var query = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.Owner)
            .Include(p => p.Reviews)
            .Include(p => p.Reservations)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(p => p.City.Contains(city) || p.Location.Contains(city));

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.PropertyType == type);

        if (minPrice.HasValue)
            query = query.Where(p => p.PricePerNight >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.PricePerNight <= maxPrice.Value);

        
        if (!string.IsNullOrWhiteSpace(features))
        {
            var featureList = features.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim().ToLower());
            foreach (var feature in featureList)
            {
                query = query.Where(p => p.Description.ToLower().Contains(feature) || p.Title.ToLower().Contains(feature));
            }
        }

        
        if (checkIn.HasValue && checkOut.HasValue)
        {
            var inDate = checkIn.Value.Date;
            var outDate = checkOut.Value.Date;

            query = query.Where(p => !p.Reservations.Any(r => 
                r.ReservationStatus != "Cancelled" && 
                r.ReservationStatus != "Rejected" &&
                ((inDate >= r.CheckInDate.Date && inDate < r.CheckOutDate.Date) || 
                 (outDate > r.CheckInDate.Date && outDate <= r.CheckOutDate.Date) ||
                 (inDate <= r.CheckInDate.Date && outDate >= r.CheckOutDate.Date))
            ));
        }

        var results = query.Select(p => new
        {
            p.PropertyId,
            p.OwnerId,
            OwnerName = p.Owner != null ? p.Owner.FullName : "Unknown",
            p.Title,
            p.Description,
            p.Location,
            p.City,
            p.State,
            p.Country,
            p.PropertyType,
            PricePerNight = (double)p.PricePerNight,
            p.MaxGuests,
            p.CreatedAt,
            Images = p.Images != null ? p.Images.Select(i => i.ImageUrl).ToList() : new List<string>(),
            Rating = (p.Reviews != null && p.Reviews.Any()) ? p.Reviews.Average(r => r.Rating) : 0
        }).ToList();

        _logger.LogInformation($"SearchProperties executed with {results.Count} results found.");

        return Ok(results);
    }

    [Authorize(Roles = "Owner")]
    [HttpDelete("{id}")]
    public IActionResult DeleteProperty(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var property = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.PropertyFeatures)
            .Include(p => p.Reservations)
            .Include(p => p.Reviews)
            .FirstOrDefault(p => p.PropertyId == id);

        if (property == null)
            return NotFound("Property not found");

        if (property.OwnerId != userId)
            return Forbid();

        
        _context.PropertyImages.RemoveRange(property.Images);
        _context.PropertyFeatures.RemoveRange(property.PropertyFeatures);
        _context.Reservations.RemoveRange(property.Reservations);
        _context.Reviews.RemoveRange(property.Reviews);

        
        var wishlists = _context.Wishlists.Where(w => w.PropertyId == id).ToList();
        if (wishlists.Any()) _context.Wishlists.RemoveRange(wishlists);

        var messages = _context.Messages.Where(m => m.PropertyId == id).ToList();
        if (messages.Any()) _context.Messages.RemoveRange(messages);

        var alerts = _context.Notifications.Where(n => n.ReferenceId == id).ToList();
        if (alerts.Any()) _context.Notifications.RemoveRange(alerts);

        
        _context.Properties.Remove(property);
        _context.SaveChanges();

        return Ok("Property deleted successfully");
    }
}
