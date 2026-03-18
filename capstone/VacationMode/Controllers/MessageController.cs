

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationMode.Data;
using VacationMode.Models;
using System.Security.Claims;
using VacationMode.DTOs;

namespace VacationMode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendMessage(SendMessageDto dto)
        {
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var message = new Message
            {
                PropertyId = dto.PropertyId,
                MessageText = dto.MessageText,
                SenderId = senderId,
                SentAt = DateTime.Now,
                IsRead = false
            };

            var property = _context.Properties.Find(dto.PropertyId);

            if (property == null)
                return NotFound("Property not found");

            message.ReceiverId = property.OwnerId;

            _context.Messages.Add(message);
            _context.SaveChanges();

            return Ok(message);
        }

        [Authorize]
        [HttpGet("my-messages")]
        public IActionResult GetMyMessages()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var messages = _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .ToList();

            return Ok(messages);
        }

        [Authorize]
        [HttpGet("property/{propertyId}")]
        public IActionResult GetPropertyMessages(int propertyId)
        {
            var messages = _context.Messages
                .Where(m => m.PropertyId == propertyId)
                .ToList();

            return Ok(messages);
        }
    }
}