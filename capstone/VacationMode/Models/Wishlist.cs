using System;
using System.ComponentModel.DataAnnotations;

namespace VacationMode.Models;

public class Wishlist
{
    [Key]
    public int WishlistId { get; set; }

    public int UserId { get; set; }

    public int PropertyId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Property Property { get; set; } = null!;
}
