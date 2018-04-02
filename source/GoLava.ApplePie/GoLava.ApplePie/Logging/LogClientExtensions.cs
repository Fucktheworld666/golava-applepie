namespace GoLava.ApplePie.Logging
{
    /// <summary>
    /// Extensions for the ILogClient interface.
    /// </summary>
    public static class LogClientExtensions
    {
        /// <summary>
        /// Checks if debug level is enabled.
        /// </summary>
        public static bool IsDebugEnabled(this ILogClient logClient)
        {
            return logClient.IsEnabled(LogLevel.Debug);
        }

        /// <summary>
        /// Checks if info level is enabled.
        /// </summary>
        public static bool IsInfoEnabled(this ILogClient logClient)
        {
            return logClient.IsEnabled(LogLevel.Info);
        }

        /// <summary>
        /// Checks if warning level is enabled.
        /// </summary>
        public static bool IsWarningEnabled(this ILogClient logClient)
        {
            return logClient.IsEnabled(LogLevel.Warning);
        }

        /// <summary>
        /// Checks if error level is enabled.
        /// </summary>
        public static bool IsErrorEnabled(this ILogClient logClient)
        {
            return logClient.IsEnabled(LogLevel.Error);
        }

        /// <summary>
        /// Write a debug log message.
        /// </summary>
        /// <param name="logClient">The log client to be used to write the message.</param>
        /// <param name="fmt">Format to be used to build the message to log using string.Format.</param>
        /// <param name="args">Arguments to be to build the message to log using string.Format.</param>
        public static void Debug(this ILogClient logClient, string fmt, params object[] args)
        {
            Write(logClient, LogLevel.Debug, fmt, args);
        }

        /// <summary>
        /// Write a info log message.
        /// </summary>
        /// <param name="logClient">The log client to be used to write the message.</param>
        /// <param name="fmt">Format to be used to build the message to log using string.Format.</param>
        /// <param name="args">Arguments to be to build the message to log using string.Format.</param>
        public static void Info(this ILogClient logClient, string fmt, params object[] args)
        {
            Write(logClient, LogLevel.Info, fmt, args);
        }

        /// <summary>
        /// Write a warning log message.
        /// </summary>
        /// <param name="logClient">The log client to be used to write the message.</param>
        /// <param name="fmt">Format to be used to build the message to log using string.Format.</param>
        /// <param name="args">Arguments to be to build the message to log using string.Format.</param>
        public static void Warning(this ILogClient logClient, string fmt, params object[] args)
        {
            Write(logClient, LogLevel.Warning, fmt, args);
        }

        /// <summary>
        /// Write a error log message.
        /// </summary>
        /// <param name="logClient">The log client to be used to write the message.</param>
        /// <param name="fmt">Format to be used to build the message to log using string.Format.</param>
        /// <param name="args">Arguments to be to build the message to log using string.Format.</param>
        public static void Error(this ILogClient logClient, string fmt, params object[] args)
        {
            Write(logClient, LogLevel.Error, fmt, args);
        }

        private static void Write(ILogClient logClient, LogLevel logLevel, string fmt, params object[] args)
        {
            if (!logClient.IsEnabled(logLevel))
                return;

            var message = string.Format(fmt, args);
            logClient.Write(logLevel, message);
        }
    }
}