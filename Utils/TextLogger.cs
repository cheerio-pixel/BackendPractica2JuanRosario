
namespace backend.Utils
{
    internal class TextLogger
    : ILogger
    {
        private readonly string logPathFile;
        private readonly string _categoryName;

        public TextLogger(string logPathFile, string categoryName)
        {
            this.logPathFile = logPathFile;
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Ensure that only information level and higher logs are recorded
            if (!IsEnabled(logLevel))
            {
                return;
            }

            // Get the formatted log message
            var message = formatter(state, exception);

            //Write log messages to text file
            File.AppendAllText(logPathFile, $"[{logLevel}] [${_categoryName}] {message}\n");
        }
    }
}