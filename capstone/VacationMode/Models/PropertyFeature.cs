using System.ComponentModel.DataAnnotations;

namespace VacationMode.Models;

public class PropertyFeature
{
    [Key]
    public int PropertyFeatureId { get; set; }

    public int PropertyId { get; set; }

    public int FeatureId { get; set; }

    public Property Property { get; set; } = null!;
    public Feature Feature { get; set; } = null!;
}