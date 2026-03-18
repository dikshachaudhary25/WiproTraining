using SolidPrinciplesDemo.Interfaces;
using System.IO;

namespace SolidPrinciplesDemo.Savers
{
    public class ReportSaver : ISavable
    {
        public void Save(string content)
        {
            File.WriteAllText("report.txt", content);
        }
    }
}