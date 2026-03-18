using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidPrinciplesDemo.Factories;
using SolidPrinciplesDemo.Documents;

namespace SolidPrinciplesDemo.Tests
{
    [TestClass]
    public class DocumentFactoryTests
    {
        [TestMethod]
        public void Create_Should_Return_PdfDocument()
        {
            var doc = DocumentFactory.Create("PDF");

            Assert.IsInstanceOfType(doc, typeof(PdfDocument));
        }

        [TestMethod]
        public void Create_Should_Return_WordDocument()
        {
            var doc = DocumentFactory.Create("WORD");

            Assert.IsInstanceOfType(doc, typeof(WordDocument));
        }
    }
}