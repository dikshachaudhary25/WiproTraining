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

    public PropertyController(AppDbContext context)
    {
        _context = context;
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
            .Select(p => new
            {
                p.PropertyId,
                p.OwnerId,
                p.Title,
                p.Description,
                p.Location,
                p.City,
                p.State,
                p.Country,
                p.PropertyType,
                p.PricePerNight,
                p.MaxGuests,
                p.CreatedAt,
                ImageUrl = p.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()
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
            .Where(p => p.PropertyId == id)
            .Select(p => new
            {
                p.PropertyId,
                p.OwnerId,
                p.Title,
                p.Description,
                p.Location,
                p.City,
                p.State,
                p.Country,
                p.PropertyType,
                p.PricePerNight,
                p.MaxGuests,
                p.CreatedAt,
                ImageUrl = p.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()
            })
            .FirstOrDefault();

        if (property == null)
            return NotFound("Property not found");

        return Ok(property);
    }

    [Authorize(Roles = "Owner")]
    [HttpDelete("{id}")]
    public IActionResult DeleteProperty(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var property = _context.Properties.Find(id);

        if (property == null)
            return NotFound("Property not found");

        if (property.OwnerId != userId)
            return Forbid();

        _context.Properties.Remove(property);
        _context.SaveChanges();

        return Ok("Property deleted successfully");
    }
}
