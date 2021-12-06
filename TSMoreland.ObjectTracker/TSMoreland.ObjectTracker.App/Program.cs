//
// Copyright � 2021 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
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
