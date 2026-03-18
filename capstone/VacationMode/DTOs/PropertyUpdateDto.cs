namespace VacationMode.DTOs;

public class PropertyUpdateDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string PropertyType { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
}