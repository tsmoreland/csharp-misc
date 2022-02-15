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
            configureTransit.AddConsumer<MessageConsumer>();
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
