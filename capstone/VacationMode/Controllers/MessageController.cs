using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationMode.Data;
using VacationMode.Models;
using System.Security.Claims;
using VacationMode.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace VacationMode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<VacationMode.Hubs.ChatHub> _hubContext;

        public MessageController(AppDbContext context, Microsoft.AspNetCore.SignalR.IHubContext<VacationMode.Hubs.ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendMessage(SendMessageDto dto)
        {
            var senderIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(senderIdStr, out int senderId)) return Unauthorized();

            var property = _context.Properties.Find(dto.PropertyId);
            if (property == null)
                return NotFound("Property not found");

            var receiverId = dto.ReceiverId ?? property.OwnerId;

            var message = new Message
            {
                PropertyId = dto.PropertyId,
                MessageText = dto.MessageText,
                SenderId = senderId,
                ReceiverId = receiverId,
                SentAt = DateTime.Now,
                IsRead = false,
                Sender = null!,
                Receiver = null!,
                Property = null!
            };

            _context.Messages.Add(message);

            
            var senderName = User.FindFirst(ClaimTypes.Name)?.Value ?? "A user";
            var notification = new Notification
            {
                UserId = receiverId,
                Type = "Message",
                ReferenceId = property.PropertyId,
                Message = $"New message regarding {property.Title}.",
                IsRead = false,
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(notification);

            _context.SaveChanges();

            
            _hubContext.Clients.User(receiverId.ToString())
                .SendAsync("ReceiveNotification", notification);

            
            var messageDtoToDispatch = new {
                MessageId = message.MessageId,
                PropertyId = message.PropertyId,
                SenderId = message.SenderId,
                SenderName = senderName,
                ReceiverId = message.ReceiverId,
                MessageText = message.MessageText,
                SentAt = message.SentAt
            };

            
            _hubContext.Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", senderId.ToString(), messageDtoToDispatch);

            
            _hubContext.Clients.User(senderId.ToString())
                .SendAsync("ReceiveMessage", senderId.ToString(), messageDtoToDispatch);

            return Ok(messageDtoToDispatch);
        }

        [Authorize]
        [HttpGet("my-messages")]
        public IActionResult GetMyMessages()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var messages = _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Include(m => m.Property)
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new {
                    m.MessageId,
                    m.PropertyId,
                    PropertyTitle = m.Property != null ? m.Property.Title : "Unknown Property",
                    m.SenderId,
                    SenderName = m.Sender != null ? m.Sender.FullName : "Unknown User",
                    m.ReceiverId,
                    ReceiverName = m.Receiver != null ? m.Receiver.FullName : "Unknown User",
                    m.MessageText,
                    m.SentAt,
                    m.IsRead
                })
                .ToList();

            return Ok(messages);
        }

        [Authorize]
        [HttpGet("property/{propertyId}")]
        public IActionResult GetPropertyMessages(int propertyId)
        {
            var messages = _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Include(m => m.Property)
                .Where(m => m.PropertyId == propertyId)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new {
                    m.MessageId,
                    m.PropertyId,
                    PropertyTitle = m.Property != null ? m.Property.Title : "Unknown Property",
                    m.SenderId,
                    SenderName = m.Sender != null ? m.Sender.FullName : "Unknown User",
                    m.ReceiverId,
                    ReceiverName = m.Receiver != null ? m.Receiver.FullName : "Unknown User",
                    m.MessageText,
                    m.SentAt,
                    m.IsRead
                })
                .ToList();

            return Ok(messages);
        }

        [Authorize]
        [HttpGet("conversation/{propertyId}/{otherUserId}")]
        public IActionResult GetConversation(int propertyId, int otherUserId)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var messages = _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.PropertyId == propertyId && 
                            ((m.SenderId == userId && m.ReceiverId == otherUserId) || 
                             (m.SenderId == otherUserId && m.ReceiverId == userId)))
                .OrderBy(m => m.SentAt)
                .Select(m => new {
                    m.MessageId,
                    m.SenderId,
                    SenderName = m.Sender != null ? m.Sender.FullName : "Unknown User",
                    m.ReceiverId,
                    m.MessageText,
                    m.SentAt
                })
                .ToList();

            return Ok(messages);
        }
    }
}