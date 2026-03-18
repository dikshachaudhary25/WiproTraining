using Serilog;

namespace SecureUserApp
{
    public static class LoggerService
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}