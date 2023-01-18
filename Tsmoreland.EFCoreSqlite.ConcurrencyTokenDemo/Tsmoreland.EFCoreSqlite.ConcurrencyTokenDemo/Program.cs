//
// Copyright (c) 2022 Terry Moreland
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

using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tsmoreland.EFCoreSqlite.ConcurrencyTokenDemo;

IServiceCollection services = new ServiceCollection();

// alternate strategy - use IsConcurrencyToken instead of IsRowVersion and manully update the property prior to each save

services
    .AddDbContext<PeopleDbContext>(options =>
    {
        options.UseSqlite("Data Source=people.db",
            ob => ob.MigrationsAssembly(typeof(PeopleDbContext).Assembly.FullName));
        options.LogTo(Console.WriteLine, LogLevel.Information);
        options.EnableDetailedErrors(true);
        options.EnableSensitiveDataLogging(true);
    });

IServiceProvider provider = services.BuildServiceProvider();

Person person = new("John", "Smith", new Address(34, "Main St", "N1A2A2"), "john@smith.com");
using (IServiceScope initScope = provider.CreateScope())
{

    PeopleDbContext initDbCtx = initScope.ServiceProvider.GetRequiredService<PeopleDbContext>();

    await initDbCtx.Database.EnsureDeletedAsync();
    await initDbCtx.Database.MigrateAsync();

    initDbCtx.People.Add(person);
    await initDbCtx.SaveChangesAsync();
}

using IServiceScope scope1 = provider.CreateScope();
using IServiceScope scope2 = provider.CreateScope();


PeopleDbContext dbCtx1 = scope1.ServiceProvider.GetRequiredService<PeopleDbContext>();
PeopleDbContext dbCtx2 = scope2.ServiceProvider.GetRequiredService<PeopleDbContext>();

Person p1 = (await dbCtx1.People.FindAsync(new object[] { person.Id }, CancellationToken.None))!;
Person p2 = (await dbCtx2.People.FindAsync(new object[] { person.Id }, CancellationToken.None))!;

p1.Email = "anonymous@gmail.com";

await dbCtx1.SaveChangesAsync();

p2.Email = "anonymous@outlook.com";

try
{
    await dbCtx2.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException ex)
{
    Console.WriteLine("---- Failed to save ---- ");
    Console.WriteLine(ex);
    Console.WriteLine("------------------------ ");

    // this in theory should be in a loop -- creating new people dbcontext should clear the cache, alternatively uses FirstOrDefault (because Find willr return previous value and just repeat the exception)
    using IServiceScope scope3 = provider.CreateScope();
    // using scope3 because GetRequiredService<PeopleDbContext>() on either scope1 or scope2 would return their copies, in this scenario we may want a dbcontextfactory (if attempting to recover within the same scope/request)
    await ReloadPersonAndReApplyChanges(p2, scope3.ServiceProvider.GetRequiredService<PeopleDbContext>());

    Console.WriteLine("Save after reload/reapply successful.");
}
catch (Exception ex)
{
    Console.WriteLine("---- Failed to save ---- ");
    Console.WriteLine(ex);
    Console.WriteLine("------------------------ ");
}

static async Task ReloadPersonAndReApplyChanges(Person p, PeopleDbContext dbCtx)
{
    Person existing = (await dbCtx.People.FindAsync(new object[] { p.Id }, CancellationToken.None))!;
    existing.Email = p.Email;
    await dbCtx.SaveChangesAsync();
}
