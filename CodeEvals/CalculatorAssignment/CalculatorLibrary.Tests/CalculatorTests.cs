using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLibrary;
using System;

namespace CalculatorLibrary.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator _calculator = null!;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new Calculator();
        }

        [TestMethod]
        public void AddResult()
        {
            Assert.AreEqual(8, _calculator.Add(5, 3));
        }

        [TestMethod]
        public void Subtract_Result()
        {
            Assert.AreEqual(6, _calculator.Subtract(10, 4));
        }

        [TestMethod]
        public void Multiply_ReturnsCorrectResult()
        {
            Assert.AreEqual(30, _calculator.Multiply(6, 5));
        }

        [TestMethod]
        public void Divide_ReturnsCorrectResult()
        {
            Assert.AreEqual(5, _calculator.Divide(20, 4));
        }

        [TestMethod]
        public void Divide_ByZero_ThrowsException()
        {
            try
            {
                _calculator.Divide(10, 0);
                Assert.Fail("Expected DivideByZeroException was not thrown.");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(DivideByZeroException));
            }
        }
    }
}