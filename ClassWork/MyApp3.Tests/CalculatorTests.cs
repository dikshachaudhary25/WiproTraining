using Xunit;
using MyApp3;

namespace MyApp3.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calc = new Calculator();

            // Act
            var result = calc.Add(2, 3);

            // Assert
            Assert.Equal(5, result);
        }
    }
}