using SolidPrinciplesDemo.Interfaces;
using SolidPrinciplesDemo.Models;

namespace SolidPrinciplesDemo.Generators
{
    public class ReportGenerator : IGeneratable
    {
        private readonly Report _report;

        public ReportGenerator(Report report)
        {
            _report = report;
        }

        public string Generate()
        {
            return _report.Generate();
        }
    }
}