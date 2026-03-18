using SolidPrinciplesDemo.Interfaces;

namespace SolidPrinciplesDemo.Formatters
{
    public class ExcelFormatter : IReportFormatter
    {
        public string Format(string content)
        {
            return $"EXCEL FORMAT: {content}";
        }
    }
}