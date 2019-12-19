using System;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestProject1
{
    public class MockLogger: ILogger, IDisposable
    {
        public bool Called { get; private set; }
        
        public string With { get; private set; }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Called = true;
            With = formatter.Invoke(state, exception);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
    
    public class MockLoggerProvider: ILoggerProvider
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return ServiceLocator.Current.GetService<ILogger>();
        }
    }
}