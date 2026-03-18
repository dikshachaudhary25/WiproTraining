namespace FoodApp.MVC.Models;

public class ProductsSold
{
    public int ProductId { get; set; }   

    public int SaleId { get; set; }     

    public Food? Food { get; set; }

    public Sale? Sale { get; set; }
}