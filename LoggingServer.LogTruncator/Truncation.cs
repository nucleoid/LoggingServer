using LoggingServer.Server.Domain;

namespace LoggingServer.LogTruncator
{
    public class Truncation
    {
        public int RollingDays { get; set; }
        public LogLevel? LogLevel { get; set; }
    }
}
