using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MyApp3;

namespace MyApp3.Tests
{
    public class AppDbContextTests
    {
        [Fact]
        public void AddUser_SavesToInMemoryDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using var context = new AppDbContext(options);

            // Act
            context.Users.Add(new User { Name = "Test User" });
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.Users.Count());
        }
    }
}