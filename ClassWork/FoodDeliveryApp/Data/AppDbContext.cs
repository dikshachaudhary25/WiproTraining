using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.API.Models;

namespace FoodDeliveryApp.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<ProductsSold> ProductsSold { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductsSold>()
            .HasKey(ps => new { ps.ProductId, ps.SaleId });

        modelBuilder.Entity<Food>()
            .Property(f => f.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Cart>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Cart>()
            .Property(c => c.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(s => s.TotalAmount)
            .HasPrecision(18, 2);
    }
}