using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Models;

namespace MovieCatalogApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Director> Directors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationship: One Director -> Many Movies
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId);
        }
    }
}