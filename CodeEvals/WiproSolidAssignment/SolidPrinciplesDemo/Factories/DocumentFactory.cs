using SolidPrinciplesDemo.Documents;
using System;

namespace SolidPrinciplesDemo.Factories
{
    public class DocumentFactory
    {
        public static IDocument Create(string type)
        {
            return type switch
            {
                "PDF" => new PdfDocument(),
                "WORD" => new WordDocument(),
                _ => throw new ArgumentException("Invalid document type")
            };
        }
    }
}