using System.ComponentModel.DataAnnotations;
namespace VacationMode.Models;

public class PropertyImage
{
    [Key]
    public int ImageId { get; set; }

    public int PropertyId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public Property Property { get; set; } = null!;
}