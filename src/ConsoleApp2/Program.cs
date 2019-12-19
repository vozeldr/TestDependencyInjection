using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using ClassLibrary1;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleApp2
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Program
    {
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        public static void Main(string[] args)
        {
            RegisterServices();
            
            AsyncMessageWriter asyncMessageWriter = new AsyncMessageWriter(ServiceLocator.Current.GetService<ILogger<AsyncMessageWriter>>());
            asyncMessageWriter.WriteMessage("Here is a test message.").Wait(100);

            IDateWriter writer = ServiceLocator.Current.GetService<IDateWriter>();
            writer.WriteDate();
        }

        static void RegisterServices()
        {
            ConfigurationBuilder config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.config.json");

            ConfigurationModule module = new ConfigurationModule(config.Build());

            ServiceCollection collection = new ServiceCollection();
            ContainerBuilder builder = new ContainerBuilder();
            collection.AddLogging(c => c.AddConsole().AddSerilog());
            builder.Populate(collection);
            builder.RegisterModule(module);
            
            IContainer appContainer = builder.Build();

            AutofacServiceLocator locator = new AutofacServiceLocator(appContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}