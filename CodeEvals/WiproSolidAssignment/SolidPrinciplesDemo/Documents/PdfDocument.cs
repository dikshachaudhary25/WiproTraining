namespace SolidPrinciplesDemo.Documents
{
    public class PdfDocument : IDocument
    {
        public string Open()
        {
            return "PDF Document Opened";
        }
    }
}