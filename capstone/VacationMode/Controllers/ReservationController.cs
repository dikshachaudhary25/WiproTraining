using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacationMode.Data;
using VacationMode.DTOs;
using VacationMode.Models;
using VacationMode.Services;

namespace VacationMode.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public ReservationController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [Authorize(Roles = "Renter")]
    [HttpPost]
    public IActionResult CreateReservation(CreateReservationDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var property = _context.Properties.Find(dto.PropertyId);

        if (property == null)
            return NotFound("Property not found");

        var nights = (dto.CheckOutDate - dto.CheckInDate).Days;

        if (nights <= 0)
            return BadRequest("Check-out date must be after check-in date");

        
        var overlappingReservation = _context.Reservations
            .Where(r => r.PropertyId == dto.PropertyId 
                     && r.ReservationStatus != "Cancelled" 
                     && r.ReservationStatus != "Rejected")
            .FirstOrDefault(r => dto.CheckInDate < r.CheckOutDate && dto.CheckOutDate > r.CheckInDate);

        if (overlappingReservation != null)
        {
            return BadRequest("Property is not available for the selected dates.");
        }

        var reservation = new Reservation
        {
            UserId = userId,
            PropertyId = dto.PropertyId,
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            TotalAmount = property.PricePerNight * nights,
            ReservationStatus = "Pending",
            CreatedAt = DateTime.Now
        };

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        var renterName = User.FindFirst(ClaimTypes.Name)?.Value ?? "A user";

        
        var notification = new Notification
        {
            UserId = property.OwnerId,
            Type = "Booking",
            ReferenceId = reservation.ReservationId,
            Message = $"{renterName} has booked {property.Title}.",
            IsRead = false,
            CreatedAt = DateTime.Now
        };
        _context.Notifications.Add(notification);
        _context.SaveChanges();

        
        var owner = _context.Users.Find(property.OwnerId);
        if (owner != null && !string.IsNullOrEmpty(owner.Email))
        {
            var subject = $"New Reservation for {property.Title}";
            var body = $@"
                <h3>New Reservation Alert</h3>
                <p>Hello {owner.FullName},</p>
                <p><strong>{renterName}</strong> has just booked your property <strong>{property.Title}</strong>.</p>
                <ul>
                    <li><strong>Check-in:</strong> {dto.CheckInDate.ToShortDateString()}</li>
                    <li><strong>Check-out:</strong> {dto.CheckOutDate.ToShortDateString()}</li>
                    <li><strong>Total Amount:</strong> ₹{reservation.TotalAmount}</li>
                </ul>
                <p>Please log in to your dashboard to confirm or reject this reservation.</p>
            ";

            _ = _emailService.SendEmailAsync(owner.Email, subject, body);
        }

        return Ok(reservation);
    }

    [HttpGet("property/{id}/booked-dates")]
    public IActionResult GetBookedDates(int id)
    {
        var reservations = _context.Reservations
            .Where(r => r.PropertyId == id 
                     && r.ReservationStatus != "Cancelled" 
                     && r.ReservationStatus != "Rejected")
            .Select(r => new { r.CheckInDate, r.CheckOutDate })
            .ToList();

        var bookedDates = new List<string>();

        foreach (var res in reservations)
        {
            for (var dt = res.CheckInDate; dt < res.CheckOutDate; dt = dt.AddDays(1))
            {
                bookedDates.Add(dt.ToString("yyyy-MM-dd"));
            }
        }

        return Ok(bookedDates.Distinct().ToList());
    }

    [Authorize]
    [HttpGet("my-reservations")]
    public IActionResult GetMyReservations()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var reservations = _context.Reservations
            .Include(r => r.Property)
                .ThenInclude(p => p.Owner)
            .Include(r => r.Property)
                .ThenInclude(p => p.Images)
            .Include(r => r.Property)
                .ThenInclude(p => p.Reviews)
            .Where(r => r.UserId == userId)
            .Select(r => new
            {
                r.ReservationId,
                r.UserId,
                r.PropertyId,
                r.CheckInDate,
                r.CheckOutDate,
                TotalAmount = (double)r.TotalAmount,
                r.ReservationStatus,
                r.CreatedAt,
                Property = r.Property == null ? null : new {
                    r.Property.PropertyId,
                    r.Property.OwnerId,
                    OwnerName = r.Property.Owner != null ? r.Property.Owner.FullName : "Unknown",
                    r.Property.Title,
                    r.Property.City,
                    r.Property.State,
                    PricePerNight = (double)r.Property.PricePerNight,
                    Images = r.Property.Images != null ? r.Property.Images.Select(i => i.ImageUrl).ToList() : new List<string>(),
                    Rating = (r.Property.Reviews != null && r.Property.Reviews.Any()) ? r.Property.Reviews.Average(rev => rev.Rating) : 0
                }
            })
            .ToList();

        return Ok(reservations);
    }

    [Authorize(Roles = "Owner")]
    [HttpGet("owner-reservations")]
    public IActionResult GetOwnerReservations()
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var reservations = _context.Reservations
            .Include(r => r.Property)
                .ThenInclude(p => p.Owner)
            .Include(r => r.Property)
                .ThenInclude(p => p.Images)
            .Include(r => r.Property)
                .ThenInclude(p => p.Reviews)
            .Include(r => r.User)
            .Where(r => r.Property.OwnerId == ownerId)
            .Select(r => new
            {
                r.ReservationId,
                r.PropertyId,
                r.UserId,
                RenterName = r.User.FullName,
                r.CheckInDate,
                r.CheckOutDate,
                TotalAmount = (double)r.TotalAmount,
                r.ReservationStatus,
                r.CreatedAt,
                Property = r.Property == null ? null : new {
                    r.Property.PropertyId,
                    r.Property.OwnerId,
                    OwnerName = r.Property.Owner != null ? r.Property.Owner.FullName : "Unknown",
                    r.Property.Title,
                    r.Property.City,
                    r.Property.State,
                    PricePerNight = (double)r.Property.PricePerNight,
                    Images = r.Property.Images != null ? r.Property.Images.Select(i => i.ImageUrl).ToList() : new List<string>(),
                    Rating = (r.Property.Reviews != null && r.Property.Reviews.Any()) ? r.Property.Reviews.Average(rev => rev.Rating) : 0
                }
            })
            .ToList();

        return Ok(reservations);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut("confirm/{id}")]
    public IActionResult ConfirmReservation(int id)
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var reservation = _context.Reservations.Include(r => r.Property).FirstOrDefault(r => r.ReservationId == id);

        if (reservation == null)
            return NotFound();

        if (reservation.Property.OwnerId != ownerId)
            return Forbid();

        reservation.ReservationStatus = "Confirmed";
        _context.SaveChanges();

        return Ok(reservation);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut("reject/{id}")]
    public IActionResult RejectReservation(int id)
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var reservation = _context.Reservations.Include(r => r.Property).FirstOrDefault(r => r.ReservationId == id);

        if (reservation == null)
            return NotFound();

        if (reservation.Property.OwnerId != ownerId)
            return Forbid();

        reservation.ReservationStatus = "Rejected";
        _context.SaveChanges();

        return Ok(reservation);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult CancelReservation(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var reservation = _context.Reservations.Find(id);

        if (reservation == null)
            return NotFound("Reservation not found");

        if (reservation.UserId != userId)
            return Forbid();

        reservation.ReservationStatus = "Cancelled";
        _context.SaveChanges();

        return Ok("Reservation cancelled");
    }
}
