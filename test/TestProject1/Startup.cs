using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using ClassLibrary1;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestProject1
{
    public static class Startup
    {
        public static bool Initialized { get; private set; }
        
        public static void Configure()
        {
            if (Initialized) return;
            
            ServiceCollection collection = new ServiceCollection();
            ContainerBuilder builder = new ContainerBuilder();
            collection.AddSingleton<ILogger>(provider => new MockLogger());
            collection.AddLogging(c => c.AddProvider(new MockLoggerProvider()));
            builder.Populate(collection);
            
            builder.RegisterInstance(new MockOutput()).As<IOutput>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            IContainer appContainer = builder.Build();

            AutofacServiceLocator locator = new AutofacServiceLocator(appContainer);
            ServiceLocator.SetLocatorProvider(() => locator);

            Initialized = true;
        }
    }
}