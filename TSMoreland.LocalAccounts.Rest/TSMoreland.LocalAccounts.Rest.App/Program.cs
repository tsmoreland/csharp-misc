using TSMoreland.LocalAccounts.Rest.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureWebHostDefaults(webHostBuilder =>
        webHostBuilder
            .UseKestrel(kestrelOptions => kestrelOptions.AddServerHeader = false));
// use serilog, ...

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddInfrastructure();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/Error");
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

app.MapGet("/api/Error", () => Results.Problem());
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
