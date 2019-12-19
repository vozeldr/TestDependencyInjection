using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestProject1
{
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