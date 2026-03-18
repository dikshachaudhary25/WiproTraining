using Microsoft.EntityFrameworkCore;
using FoodApp.MVC.Models;

namespace FoodApp.MVC.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<ProductsSold> ProductsSold { get; set; }
    public DbSet<Party> Parties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed 3 parties: 1 external, 2 internal
        modelBuilder.Entity<Party>().HasData(
            new Party { Id = 1, Name = "External Vendor Alpha", IsExternal = true },
            new Party { Id = 2, Name = "Internal Dept HR", IsExternal = false },
            new Party { Id = 3, Name = "Internal Dept IT", IsExternal = false }
        );

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