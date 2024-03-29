﻿using System.Collections.Immutable;
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
builder.ConfigureServices((_, services) =>
{
    const string rootNamespace = "TSMoreland.Messaging";

    Assembly entry = Assembly.GetEntryAssembly()!;
    Assembly[] assemblies = entry.GetReferencedAssemblies()
        .Where(assemblyName => assemblyName.FullName.StartsWith(rootNamespace))
        .Select(Assembly.Load)
        .Union(new [] { entry })
        .ToArray();

    services
        .AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(assemblies);
        })
        .AddMassTransit(configureTransit =>
        {
            ImmutableArray<Type> allTypes = entry.GetTypes()
                .Union(assemblies.SelectMany(assembly => assembly.GetTypes()))
                .ToImmutableArray();

            IEnumerable<Type> consumers = allTypes
                .Where(type => typeof(IConsumer).IsAssignableFrom(type));
            foreach (Type consumer in consumers)
            {
                if (consumer.FullName != typeof(QueueMessageConsumer).FullName)
                {
                    configureTransit.AddConsumer(consumer);
                }
            }

            configureTransit.UsingInMemory((busRegistrationContext, factoryConfigurator) =>
            {
                factoryConfigurator.ReceiveEndpoint("queue1", receiveEndpointConfigurer =>
                {
                    // relies on parameterless constructor, injection would require a configurer type which could also accept injected types or factories
                    receiveEndpointConfigurer.Consumer<QueueMessageConsumer>();
                });

                factoryConfigurator.ConfigureEndpoints(busRegistrationContext);
            });

            IEnumerable<Type> serviceRequests = allTypes
                .Where(type => typeof(IServiceRequest).IsAssignableFrom(type));
            foreach (Type serviceRequest in serviceRequests)
            {
                configureTransit.AddRequestClient(serviceRequest);
            }
        })
        .AddMassTransitHostedService()
        .AddHostedService<WorkerService>();
});

IMsHost host = builder.Build();

host.Run();
