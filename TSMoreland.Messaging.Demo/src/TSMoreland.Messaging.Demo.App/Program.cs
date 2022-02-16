using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using TSMoreland.Messaging.Demo.App;
using IMsHost = Microsoft.Extensions.Hosting.IHost;

IHostBuilder builder = Host.CreateDefaultBuilder();
builder
    .ConfigureLogging(configureLogging =>
        configureLogging
            .SetMinimumLevel(LogLevel.Information)
            .AddDebug()
            .AddConsole());
builder.ConfigureServices((hostContext, services) =>
{
    services
        .AddMassTransit(configureTransit =>
        {
            Assembly entry = Assembly.GetEntryAssembly()!;
            IEnumerable<Type> consumers = entry.GetTypes()
                .Union(entry.GetReferencedAssemblies()
                    .Where(assemblyName => assemblyName.FullName.StartsWith("TSMoreland"))
                    .SelectMany(assemblyName => Assembly.Load(assemblyName).GetTypes()))
                .Where(type => typeof(IConsumer).IsAssignableFrom(type));
            foreach (Type consumer in consumers)
            {
                configureTransit.AddConsumer(consumer);
            }

            configureTransit.UsingInMemory((busRegistrationContext, factoryConfigurator) =>
            {
                factoryConfigurator.ConfigureEndpoints(busRegistrationContext);
            });

        })
        .AddMassTransitHostedService()
        .AddHostedService<WorkerService>();
});

IMsHost host = builder.Build();

host.Run();
