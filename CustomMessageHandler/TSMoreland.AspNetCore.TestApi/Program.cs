var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

app.UseRouting();

app.Use((context, next) =>
{
    logger.LogInformation(context.Request.Path);
    logger.LogInformation(context.Request.Headers.ContentLength?.ToString() ?? "no length set");
    return next(context);
});

app.UseAuthorization();

app.MapGet("/", context => Task.FromResult(context.Request.ContentLength));
app.MapPost("/", context => Task.FromResult(context.Request.ContentLength));

app.Run();
