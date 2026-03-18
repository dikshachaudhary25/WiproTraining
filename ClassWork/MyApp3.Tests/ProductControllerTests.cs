using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyApp3.Controllers;

namespace MyApp3.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Get_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var controller = new ProductController();

            // Act
            var result = controller.Get(0);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}