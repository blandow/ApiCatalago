using System.Collections.Concurrent;

namespace ApiCatalago.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly CustomLoggerProviderConfiguration LogConfig;

        readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration logConfig)
        {
            LogConfig = logConfig;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, LogConfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
