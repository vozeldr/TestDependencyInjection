using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using ClassLibrary1;
using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterServices();
            
            AsyncMessageWriter asyncMessageWriter = new AsyncMessageWriter(ServiceLocator.Current.GetService<ILogger<AsyncMessageWriter>>());
            asyncMessageWriter.WriteMessage("Here is a test message.").Wait(100);

            IDateWriter writer = ServiceLocator.Current.GetService<IDateWriter>();
            writer.WriteDate();
        }
        
        private static void RegisterServices()
        {
            ServiceCollection collection = new ServiceCollection();
            ContainerBuilder builder = new ContainerBuilder();
            collection.AddLogging(c => c.AddConsole().AddSerilog());
            builder.Populate(collection);
            
            builder.RegisterType<ConsoleOutput>().As<IOutput>();
            builder.RegisterType<TodayWriter>().As<IDateWriter>();
            IContainer appContainer = builder.Build();

            AutofacServiceLocator locator = new AutofacServiceLocator(appContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}