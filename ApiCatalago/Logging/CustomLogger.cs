
namespace ApiCatalago.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string _loggerName;

        readonly CustomLoggerProviderConfiguration _providerConfiguration;

        public CustomLogger(string loggerName, CustomLoggerProviderConfiguration providerConfiguration)
        {
            _loggerName = loggerName;
            _providerConfiguration = providerConfiguration;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _providerConfiguration.logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            string msg = $"{logLevel.ToString()}: {eventId.Name} - {eventId.Id} ##### --> {formatter(state, exception)}";
            WriteTextLog(msg);
        }

        private void WriteTextLog(string msg)
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory} + \custom Log";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                try
                {
                    streamWriter.WriteLine(msg);
                    streamWriter.Close();
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
}
