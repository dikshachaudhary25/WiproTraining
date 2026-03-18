using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidPrinciplesDemo.Generators;
using SolidPrinciplesDemo.Models;
using SolidPrinciplesDemo.Savers;
using SolidPrinciplesDemo.Formatters;
using SolidPrinciplesDemo.Services;
using System.IO;

namespace SolidPrinciplesDemo.Tests
{
    [TestClass]
    public class ReportServiceTests
    {
        [TestMethod]
        public void CreateAndSaveReport_With_PdfFormatter_Should_Create_File()
        {
            var report = new SalesReport();
            var generator = new ReportGenerator(report);
            var formatter = new PdfFormatter();
            var saver = new ReportSaver();

            var service = new ReportService(generator, formatter, saver);

            service.CreateAndSaveReport();

            Assert.IsTrue(File.Exists("report.txt"));
        }

        [TestMethod]
        public void CreateAndSaveReport_With_ExcelFormatter_Should_Write_Formatted_Content()
        {
            var report = new SalesReport();
            var generator = new ReportGenerator(report);
            var formatter = new ExcelFormatter();
            var saver = new ReportSaver();

            var service = new ReportService(generator, formatter, saver);

            service.CreateAndSaveReport();

            var content = File.ReadAllText("report.txt");

            Assert.Contains("EXCEL FORMAT", content);
        }
    }
}