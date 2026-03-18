using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiApp.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [ForeignKey("dept")]
    public int DeptId { get; set; }

    public Department? dept { get; set; }
}