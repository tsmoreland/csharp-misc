//
// Copyright © 2021 Terry Moreland
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

using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TSMoreland.ObjectTracker.Data.Abstractions;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;
using TSMoreland.ObjectTracker.Data.Abstractions.ViewModels;

namespace TSMoreland.ObjectTracker.Data;

public sealed class TenantObjectRepository : ITenantObjectRepository
{
    private readonly ITenantDbContextFactory _dbContextFactory;

    public TenantObjectRepository(ITenantDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }


    /// <inheritdoc />
    public async Task<ObjectEntity> Add(string tenant, ObjectEntity entity, CancellationToken cancellationToken)
    {
        await using ObjectContext context = _dbContextFactory.CreateDbContext(tenant);
        return (await context.AddAsync(entity, cancellationToken)).Entity;
    }

    /// <inheritdoc />
    public Task<LogEntity> AddMessage(string tenant, int id, LogEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IdNamePair> GetAll(string tenant, int pageNumber, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using ObjectContext context = _dbContextFactory.CreateDbContext(tenant);
        IAsyncEnumerable<IdNamePair> enumerable = context.Objects
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new IdNamePair(e.Id, e.Name))
            .AsAsyncEnumerable();
        await foreach (IdNamePair item in enumerable.WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<LogViewModel> GetLogsForObjectById(string tenant, int id, int pageNumber, int pageSize,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using ObjectContext context = _dbContextFactory.CreateDbContext(tenant);

        IAsyncEnumerable<LogViewModel> enumerable = context.Objects
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(e => e.Logs)
            .SelectMany(e => e.Logs)
            .Select(e => new LogViewModel(e.Severity, e.Message))
            .AsAsyncEnumerable();

        await foreach (LogViewModel item in enumerable.WithCancellation(cancellationToken))
        {
            yield return item;
        }
    }

    /// <inheritdoc />
    public Task<ObjectViewModel?> GetById(string tenant, int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc />
    public Task Update(string tenant, int id, ObjectEntity entity, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc />
    public Task Delete(string tenant, int id, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc />
    public Task<int> Commit(string tenant, CancellationToken cancellationToken) => throw new NotImplementedException();
}
