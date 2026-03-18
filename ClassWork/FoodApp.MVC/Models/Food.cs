namespace FoodApp.MVC.Models;

public class Food
{
    public int Id { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public ICollection<Cart>? Carts { get; set; }
}