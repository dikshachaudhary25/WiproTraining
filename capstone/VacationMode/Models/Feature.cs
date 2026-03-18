using System.ComponentModel.DataAnnotations;

namespace VacationMode.Models;

public class Feature
{
    [Key]
    public int FeatureId { get; set; }

    public string FeatureName { get; set; } = null!;

    public ICollection<PropertyFeature> PropertyFeatures { get; set; } = new List<PropertyFeature>();
}