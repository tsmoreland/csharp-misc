using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TSMoreland.LocalAccounts.Rest.Cli;
using TSMoreland.LocalAccounts.Rest.Infrastructure;


ILogger logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("cli.log")
    .CreateLogger();

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
#if DEBUG
    .AddJsonFile("appsettings.Development.json")
#endif
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(Program).Assembly)
    .Build();

IServiceCollection services = new ServiceCollection();

services
    .AddSingleton(configuration)
    .AddLogging(loggingOptions => loggingOptions.AddSerilog(logger, dispose: false))
    .AddInfrastructure()
    .AddScoped<IUserRepository, UserRepository>()
    .AddSingleton<CommandLineProcessor>();

IServiceProvider provider = services.BuildServiceProvider();

using IServiceScope scope = provider.CreateScope();

CommandLineProcessor processor = scope.ServiceProvider.GetRequiredService<CommandLineProcessor>();
processor.Process(args);

