using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSMoreland.ObjectTracker.Data;
using TSMoreland.ObjectTracker.Data.Abstractions;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddAuthorization();
services
    .AddDbContext<ObjectContext>(options => 
    { 
        options.UseSqlite("Data Source=objectTracker.db", b => 
            b.MigrationsAssembly(typeof(ObjectContext).Assembly.FullName));
    });
services.AddScoped<IObjectRepository, ObjectRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ObjectContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/error", () => Results.Problem());

app.MapPost("/objects", async ([FromServices] IObjectRepository repository, ObjectEntity entity, CancellationToken cancellationToken) =>
{
    var createdEntity = await repository.Add(entity, cancellationToken);
    await repository.Commit(cancellationToken);
    return Results.Created($"/objects/{createdEntity.Id}", createdEntity);
});

app.MapGet("/objects", 
    ([FromServices] IObjectRepository repository, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken) => 
        repository.GetAll(pageNumber ?? 1, pageSize ?? 10, cancellationToken));

app.MapGet("/objects/{id:int}",
    async ([FromServices] IObjectRepository repository, [FromRoute] int id, CancellationToken cancellationToken) =>
    {
        var result = await repository.GetById(id, cancellationToken);
        return result is not null
            ? Results.Ok(result)
            : Results.NotFound();
    });

app.MapPost("/objects/{id:int}/logs", async ([FromServices] IObjectRepository repository, [FromRoute] int id, LogEntity entity, CancellationToken cancellationToken) =>
{
    var createdEntity = await repository.AddMessage(id, entity, cancellationToken);
    await repository.Commit(cancellationToken);
    return Results.Created($"/objects/{id}/logs/{createdEntity.Id}", createdEntity);
});

app.MapGet("/objects/{id:int}/logs", 
    ([FromServices] IObjectRepository repository, [FromRoute] int id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken) => 
        repository.GetLogsForObjectById(id, pageNumber ?? 1, pageSize ?? 10, cancellationToken));


app.Run();
