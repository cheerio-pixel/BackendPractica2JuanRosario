namespace backend.Utils
{
    internal class TextLoggerProvider
    : ILoggerProvider
    {
        private readonly string logPathFile;

        public TextLoggerProvider(string logPathFile)
        {
            this.logPathFile = logPathFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TextLogger(logPathFile, categoryName);
        }

        public void Dispose()
        {
        }
    }
}