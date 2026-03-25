using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationMode.Data;
using System.Security.Claims;

namespace VacationMode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Owner")]
    public class OwnerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OwnerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var properties = _context.Properties
                .Where(p => p.OwnerId == ownerId)
                .ToList();

            var propertyIds = properties.Select(p => p.PropertyId).ToList();

            var reservations = _context.Reservations
                .Where(r => propertyIds.Contains(r.PropertyId))
                .ToList();

            var reviews = _context.Reviews
                .Where(r => propertyIds.Contains(r.PropertyId))
                .ToList();

            var totalProperties = properties.Count;

            var totalReservations = reservations.Count;

            var confirmedReservations = reservations
                .Count(r => r.ReservationStatus == "Confirmed");

            var totalRevenue = reservations
                .Where(r => r.ReservationStatus == "Confirmed")
                .Sum(r => r.TotalAmount);

            var averageRating = reviews.Any()
                ? reviews.Average(r => r.Rating)
                : 0;

            return Ok(new
            {
                totalProperties,
                totalReservations,
                confirmedReservations,
                totalRevenue,
                averageRating
            });
        }
    }
}