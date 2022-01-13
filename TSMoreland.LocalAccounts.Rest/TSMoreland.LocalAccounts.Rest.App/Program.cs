using Serilog;
using TSMoreland.LocalAccounts.Rest.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
    .UseKestrel(kestrelOptions => kestrelOptions.AddServerHeader = false);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddInfrastructure();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseMigrationsEndPoint();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/api/error", "?statusCode={0}");
app.MapGet("/api/error", (int? statusCode) => Results.Problem(statusCode: statusCode));
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
