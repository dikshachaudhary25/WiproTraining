using SolidPrinciplesDemo.Interfaces;

namespace SolidPrinciplesDemo.Services
{
    public class ReportService
    {
        private readonly IGeneratable _generator;
        private readonly IReportFormatter _formatter;
        private readonly ISavable _saver;

        public ReportService(
            IGeneratable generator,
            IReportFormatter formatter,
            ISavable saver)
        {
            _generator = generator;
            _formatter = formatter;
            _saver = saver;
        }

        public void CreateAndSaveReport()
        {
            var content = _generator.Generate();

            var formattedContent = _formatter.Format(content);

            _saver.Save(formattedContent);
        }
    }
}