//
// Copyright © 2022 Terry Moreland
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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Tsmoreland.EntityFramework.Core.Experimental.Domain.Models;
using Tsmoreland.EntityFramework.Core.Experimental.Infrastructure;

namespace Tsmoreland.EntityFramework.Core.Experimental.App;

public sealed class WorkerService : BackgroundService
{
    private readonly IDbContextFactory<PeopleDbContext> _factory;

    /// <inheritdoc />
    public WorkerService(IDbContextFactory<PeopleDbContext> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using PeopleDbContext dbContext = await _factory.CreateDbContextAsync(stoppingToken);

        await dbContext.Database.EnsureDeletedAsync(stoppingToken);

        await dbContext.Database.MigrateAsync(stoppingToken);

        int max = dbContext.People.AsNoTracking()
            .Where(m => m.LastName == "Smith")
            .Max(m => (int?)m.Id) ?? 0; // use of (int?) is to avoid exception when no matching rows found, bit of a hack/work around
        Console.WriteLine(max);

        Person p = new("Smith", "John");
        dbContext.People.Add(p);
        await dbContext.SaveChangesAsync(stoppingToken);

        Console.WriteLine(p.Id);

        Person? maybePerson = await dbContext.People.AsNoTracking().FirstOrDefaultAsync(m => m.LastName == "Smith", stoppingToken);

        Console.WriteLine($"{maybePerson?.Id ?? -1} {maybePerson?.FullName}");

    }
}
