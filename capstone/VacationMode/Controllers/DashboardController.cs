using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationMode.Data;

namespace VacationMode.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Owner")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("owner")]
    public IActionResult GetOwnerDashboard()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return Unauthorized();
        }

        var properties = _context.Properties
            .Where(p => p.OwnerId == userId)
            .ToList();

        var propertyIds = properties.Select(p => p.PropertyId).ToList();

        var reservations = _context.Reservations
            .Include(r => r.Property)
            .Include(r => r.User)
            .Where(r => propertyIds.Contains(r.PropertyId))
            .ToList();

        var totalProperties = properties.Count;
        var totalReservations = reservations.Count;
        
        var confirmedReservations = reservations.Where(r => r.ReservationStatus == "Confirmed").ToList();
        var totalRevenue = confirmedReservations.Sum(r => r.TotalAmount);

        var upcomingReservations = reservations
            .Where(r => r.CheckInDate >= DateTime.Today && r.ReservationStatus != "Cancelled" && r.ReservationStatus != "Rejected")
            .OrderBy(r => r.CheckInDate)
            .Take(5)
            .Select(r => new {
                r.ReservationId,
                PropertyName = r.Property.Title,
                GuestName = r.User.FullName,
                r.CheckInDate,
                r.CheckOutDate,
                r.ReservationStatus,
                r.TotalAmount
            })
            .ToList();

        var propertyBookings = reservations
            .GroupBy(r => r.PropertyId)
            .Select(g => new { PropertyId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .FirstOrDefault();

        var mostBookedProperty = propertyBookings != null 
            ? properties.FirstOrDefault(p => p.PropertyId == propertyBookings.PropertyId)?.Title 
            : "None";

        return Ok(new
        {
            totalProperties,
            totalReservations,
            totalRevenue,
            upcomingReservations,
            mostBookedProperty
        });
    }
}
