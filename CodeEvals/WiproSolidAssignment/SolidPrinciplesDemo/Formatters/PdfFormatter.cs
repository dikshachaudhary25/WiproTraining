using SolidPrinciplesDemo.Interfaces;

namespace SolidPrinciplesDemo.Formatters
{
    public class PdfFormatter : IReportFormatter
    {
        public string Format(string content)
        {
            return $"PDF FORMAT: {content}";
        }
    }
}