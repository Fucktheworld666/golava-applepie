namespace GoLava.ApplePie.Logging
{
    /// <summary>
    /// Simplest log interface ever.
    /// </summary>
    public interface ILogClient
    {
        /// <summary>
        /// Returns true if the given log level is enabled, false otherwise.
        /// </summary>
        /// <param name="logLevel">The log level to check.</param>
        bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// Writes a log message build from the given log level, format and arguments.
        /// </summary>
        /// <param name="logLevel">The log level that is used to log the message.</param>
        /// <param name="message">The log message to be written.</param>
        void Write(LogLevel logLevel, string message);
    }
}