using System.ComponentModel.DataAnnotations;

namespace VacationMode.DTOs;

public class PropertyCreateDto
{
    [Required, StringLength(100)]
    public string Title { get; set; } = null!;

    [Required, StringLength(1000)]
    public string Description { get; set; } = null!;

    [Required, StringLength(200)]
    public string Location { get; set; } = null!;

    [Required, StringLength(100)]
    public string City { get; set; } = null!;

    [Required, StringLength(100)]
    public string State { get; set; } = null!;

    [Required, StringLength(100)]
    public string Country { get; set; } = null!;

    [Required, StringLength(50)]
    public string PropertyType { get; set; } = null!;

    [Range(1, 1000000)]
    public decimal PricePerNight { get; set; }

    [Range(1, 50)]
    public int MaxGuests { get; set; }
}