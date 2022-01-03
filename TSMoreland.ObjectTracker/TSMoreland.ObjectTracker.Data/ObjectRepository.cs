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
using TSMoreland.ObjectTracker.Data.Abstractions;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;
using TSMoreland.ObjectTracker.Data.Abstractions.ViewModels;

namespace TSMoreland.ObjectTracker.Data;

public sealed class ObjectRepository : IObjectRepository
{
    private readonly ObjectContext _context;

    public ObjectRepository(ObjectContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<ObjectEntity> Add(ObjectEntity entity, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        return (await _context.AddAsync(entity, cancellationToken)).Entity;
    }
    /// <inheritdoc />
    public async Task<LogEntity> AddMessage(int id, LogEntity entity, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        entity.ObjectEntityId = id;
        return (await _context.AddAsync(entity, cancellationToken)).Entity;
    }


    /// <inheritdoc />
    public IAsyncEnumerable<IdNamePair> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return _disposed
            ? ThrowIfDisposed<IdNamePair>()
            : _context.Objects
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new IdNamePair(e.Id, e.Name))
                .AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<LogViewModel> GetLogsForObjectById(int id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return _disposed
            ? ThrowIfDisposed<LogViewModel>()
            :  _context.Objects
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(e => e.Logs)
                .SelectMany(e => e.Logs)
                .Select(e => new LogViewModel(e.Severity, e.Message))
                .AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public Task<ObjectViewModel?> GetById(int id, CancellationToken cancellationToken)
    {
        return _disposed
            ? Task.FromException<ObjectViewModel?>(new ObjectDisposedException("instance has been disposed"))
            :  _context.Objects
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => (ObjectViewModel?) new ObjectViewModel(e.Id, e.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Update(int id, ObjectEntity entity, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        ObjectEntity? existing = await _context.Objects.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (existing is null)
        {
            throw new EntityNotFoundException();
        }

        existing.Name = entity.Name;
    }

    /// <inheritdoc />
    public Task Delete(int id, CancellationToken cancellationToken)
    {
        return _context.DeleteById(id, cancellationToken);
    }

    public Task<int> Commit(CancellationToken cancellationToken)
    {
        return _disposed
            ? Task.FromException<int>(new ObjectDisposedException("instance has been disposed"))
            : _context.SaveChangesAsync(cancellationToken);
    }

    #region IDisposable / IAsyncDisposable
    private bool _disposed;

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("instance has been disposed");
        }
    }
    private async IAsyncEnumerable<T> ThrowIfDisposed<T>()
    {
        await Task.CompletedTask;
        if (_disposed)
        {
            throw new ObjectDisposedException("instance has been disposed");
        }

        yield break;
    }


    ~ObjectRepository() => Dispose(false);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (disposing)
        {
            _context.Dispose();
        }
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return ValueTask.CompletedTask;
        }

        _disposed = true;
        GC.SuppressFinalize(this);
        return _context.DisposeAsync();
    }
    #endregion

}
