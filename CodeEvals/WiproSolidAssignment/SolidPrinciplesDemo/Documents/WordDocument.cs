namespace SolidPrinciplesDemo.Documents
{
    public class WordDocument : IDocument
    {
        public string Open()
        {
            return "Word Document Opened";
        }
    }
}