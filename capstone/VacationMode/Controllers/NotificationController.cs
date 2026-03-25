using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationMode.Data;
using VacationMode.Models;

namespace VacationMode.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetMyNotifications()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var notifications = _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        return Ok(notifications);
    }

    [HttpPatch("read/{id}")]
    public IActionResult MarkAsRead(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var notification = _context.Notifications.Find(id);

        if (notification == null)
            return NotFound("Notification not found");

        if (notification.UserId != userId)
            return Forbid();

        notification.IsRead = true;
        _context.SaveChanges();

        return Ok();
    }

    [HttpPatch("read-all")]
    public IActionResult MarkAllAsRead()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var unreadNotifications = _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToList();

        foreach (var n in unreadNotifications)
        {
            n.IsRead = true;
        }

        _context.SaveChanges();

        return Ok();
    }
}
