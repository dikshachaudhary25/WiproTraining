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
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "renter")]
        [HttpPost]
        public IActionResult CreateReview(CreateReviewDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var existingReview = _context.Reviews
                .FirstOrDefault(r => r.UserId == userId && r.PropertyId == dto.PropertyId);

            if (existingReview != null)
                return BadRequest("You have already reviewed this property");

            var review = new Review
            {
                PropertyId = dto.PropertyId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            var property = _context.Properties.Find(dto.PropertyId);

            if (property == null)
                return NotFound("Property not found");

            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok(review);
        }

        [HttpGet("property/{propertyId}")]
        public IActionResult GetPropertyReviews(int propertyId)
        {
            var reviews = _context.Reviews
                .Where(r => r.PropertyId == propertyId)
                .ToList();

            return Ok(reviews);
        }

        [Authorize]
        [HttpGet("my-reviews")]
        public IActionResult GetMyReviews()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var reviews = _context.Reviews
                .Where(r => r.UserId == userId)
                .ToList();

            return Ok(reviews);
        }
    }
}