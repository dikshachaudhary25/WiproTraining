using Microsoft.EntityFrameworkCore;

namespace MyApi.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Add DbSets here (example)
    // public DbSet<Employee> Employees { get; set; }
}
