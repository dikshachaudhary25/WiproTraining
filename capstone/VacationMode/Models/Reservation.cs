namespace VacationMode.Models;

public class Reservation
{
    public int ReservationId { get; set; }

    public int UserId { get; set; }

    public int PropertyId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string ReservationStatus { get; set; } = null!; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Property Property { get; set; } = null!;
}