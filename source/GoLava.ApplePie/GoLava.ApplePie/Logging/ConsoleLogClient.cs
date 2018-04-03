using System;

namespace GoLava.ApplePie.Logging
{
    public sealed class ConsoleLogClient : ILogClient
    {
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void LogMessage(LogLevel logLevel, string message)
        {
            Console.WriteLine("{0:yyyyMMddHHmmss}::{1,-7}::{2}", DateTime.UtcNow, logLevel, message);
        }
    }
}
