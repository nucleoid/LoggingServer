using System;

namespace LoggingServer.Common.Targets
{
    public class FileLogException : Exception
    {
        public FileLogException(string message) : base(message)
        {
        }
    }
}
