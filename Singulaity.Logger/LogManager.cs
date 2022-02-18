namespace Singulaity.Logger;
using Serilog;

public static class LogManager
{
    private static ILogger _logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.File("logs/singularity.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()
        .CreateLogger();
    /// <summary>
    /// Get global instance of logger
    /// </summary>
    public static ILogger Log => _logger;
}
