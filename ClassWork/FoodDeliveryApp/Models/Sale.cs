namespace FoodDeliveryApp.API.Models;

public class Sale
{
    public int SaleId { get; set; }

    public decimal TotalAmount { get; set; }

    public string UserId { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Status { get; set; } = string.Empty;

    public ICollection<ProductsSold>? ProductsSold { get; set; }
}