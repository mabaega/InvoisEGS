namespace InvoisEGS.Utilities
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly string _categoryName;
        private readonly LogLevel _minLogLevel;

        public FileLogger(string filePath, string categoryName, LogLevel minLogLevel)
        {
            _filePath = filePath;
            _categoryName = categoryName;
            _minLogLevel = minLogLevel;
        }

        public IDisposable? BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string logRecord = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {formatter(state, exception)}";
            if (exception != null)
            {
                logRecord += Environment.NewLine + exception;
            }

            lock (_filePath)
            {
                File.AppendAllText(_filePath, logRecord + Environment.NewLine);
            }
        }
    }

    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _logDirectory;
        private readonly LogLevel _minLogLevel;

        public FileLoggerProvider(string logDirectory, LogLevel minLogLevel)
        {
            // Use environment variable for Azure compatibility
            _logDirectory = logDirectory ?? Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? "D:\\local", "Logs");
            _minLogLevel = minLogLevel;

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            string logFilePath = Path.Combine(_logDirectory, $"log-{DateTime.UtcNow:yyyy-MM-dd}.log");
            return new FileLogger(logFilePath, categoryName, _minLogLevel);
        }

        public void Dispose() { }
    }


}
