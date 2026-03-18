using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationMode.Data;
using VacationMode.DTOs;
using VacationMode.Models;

namespace VacationMode.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReservationController(AppDbContext context)
    {
        _context = context;
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

        return Ok(reservation);
    }

    [Authorize]
    [HttpGet("my-reservations")]
    public IActionResult GetMyReservations()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var reservations = _context.Reservations
            .Where(r => r.UserId == userId)
            .ToList();

        return Ok(reservations);
    }

    [Authorize(Roles = "Owner")]
    [HttpGet("owner-reservations")]
    public IActionResult GetOwnerReservations()
    {
        var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var reservations = _context.Reservations
            .Where(r => r.Property.OwnerId == ownerId)
            .ToList();

        return Ok(reservations);
    }

    [Authorize(Roles = "Owner")]
    [HttpPut("confirm/{id}")]
    public IActionResult ConfirmReservation(int id)
    {
        var reservation = _context.Reservations.Find(id);

        if (reservation == null)
            return NotFound();

        reservation.ReservationStatus = "Confirmed";
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
