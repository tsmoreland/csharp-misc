using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSMoreland.ObjectTracker.Data;
using TSMoreland.ObjectTracker.Data.Abstractions;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;
using TSMoreland.ObjectTracker.Data.Abstractions.ViewModels;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
services.AddAuthorization();
services.AddDbContext<ObjectContext>();

services.AddScoped<IObjectRepository, ObjectRepository>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ObjectContext db = scope.ServiceProvider.GetRequiredService<ObjectContext>();
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
    ObjectEntity createdEntity = await repository.Add(entity, cancellationToken);
    await repository.Commit(cancellationToken);
    return Results.Created($"/objects/{createdEntity.Id}", createdEntity);
});

app.MapGet("/objects", 
    ([FromServices] IObjectRepository repository, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken) => 
        repository.GetAll(pageNumber ?? 1, pageSize ?? 10, cancellationToken));

app.MapGet("/objects/{id:int}",
    async ([FromServices] IObjectRepository repository, [FromRoute] int id, CancellationToken cancellationToken) =>
    {
        ObjectViewModel? result = await repository.GetById(id, cancellationToken);
        return result is not null
            ? Results.Ok(result)
            : Results.NotFound();
    });

app.MapPost("/objects/{id:int}/logs", async ([FromServices] IObjectRepository repository, [FromRoute] int id, LogEntity entity, CancellationToken cancellationToken) =>
{
    LogEntity createdEntity = await repository.AddMessage(id, entity, cancellationToken);
    await repository.Commit(cancellationToken);
    return Results.Created($"/objects/{id}/logs/{createdEntity.Id}", createdEntity);
});

app.MapGet("/objects/{id:int}/logs", 
    ([FromServices] IObjectRepository repository, [FromRoute] int id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken) => 
        repository.GetLogsForObjectById(id, pageNumber ?? 1, pageSize ?? 10, cancellationToken));


app.Run();
