namespace VacationMode.Models;

public class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int PropertyId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Property Property { get; set; } = null!;
}