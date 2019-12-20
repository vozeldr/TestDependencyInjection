using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using ClassLibrary1;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleApp2
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Program
    {
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureHostConfiguration(builder => { })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", optional: true);
                    builder.AddEnvironmentVariables();

                    if (args != null)
                    {
                        builder.AddCommandLine(args);
                    }
                })
                .ConfigureServices((context, collection) =>
                {
                    collection.AddOptions();
                    collection.Configure<AppConfig>(context.Configuration.GetSection("AppConfig"));
                    ConfigurationBuilder config = new ConfigurationBuilder();
                    config.AddJsonFile("autofac.config.json");

                    ConfigurationModule module = new ConfigurationModule(config.Build());

                    ContainerBuilder builder = new ContainerBuilder();
                    builder.Populate(collection);
                    builder.RegisterModule(module);

                    builder.RegisterType<App>().As<IHostedService>().SingleInstance();
                    IContainer appContainer = builder.Build();

                    AutofacServiceLocator locator = new AutofacServiceLocator(appContainer);
                    ServiceLocator.SetLocatorProvider(() => locator);
                })
                .ConfigureLogging((context, builder) => builder.AddConsole().AddSerilog());
    }

    public class AppConfig
    {
        public string Test { get; set; }
    }

    public class App : IHostedService, IDisposable
    {
        private readonly IDateWriter _writer;
        private readonly ILogger<App> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public App(IDateWriter writer, ILoggerFactory loggerFactory)
        {
            _writer = writer;
            _logger = loggerFactory.CreateLogger<App>();
            _loggerFactory = loggerFactory;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Using the injected logger from the App.");
            _writer.WriteDate();
            
            AsyncMessageWriter asyncMessageWriter = new AsyncMessageWriter(_loggerFactory.CreateLogger<AsyncMessageWriter>());
            return asyncMessageWriter.WriteMessage("Here is a test message.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}