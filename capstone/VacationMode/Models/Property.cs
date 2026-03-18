namespace VacationMode.Models;

public class Property
{
    public int PropertyId { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public decimal PricePerNight { get; set; }

    public int MaxGuests { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Owner { get; set; } = null!;
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<PropertyFeature> PropertyFeatures { get; set; } = new List<PropertyFeature>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}