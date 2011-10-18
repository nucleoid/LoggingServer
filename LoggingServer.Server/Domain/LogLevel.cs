using System;

namespace LoggingServer.Server.Domain
{
    [Flags]
    public enum LogLevel
    {
        Fatal = 1 << 0,
        Error = 1 << 1,
        Warn = 1 << 2,
        Off = 1 << 3,
        Info = 1 << 4,
        Debug = 1 << 5,
        Trace = 1 << 6
    }
}
