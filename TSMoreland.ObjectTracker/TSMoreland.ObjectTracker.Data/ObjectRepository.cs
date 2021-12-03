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
    public Task<ObjectEntity> Add(ObjectEntity entity, CancellationToken cancellationToken)
    {
        var task = _context.AddAsync(entity, cancellationToken);
        return task.IsCompleted 
            ? Task.FromResult(task.Result.Entity) 
            : Await();

        async Task<ObjectEntity> Await()
        {
            return (await task).Entity;
        }
    }
    /// <inheritdoc />
    public Task<LogEntity> AddMessage(int id, LogEntity entity, CancellationToken cancellationToken)
    {
        entity.ObjectEntityId = id;

        var task = _context.AddAsync(entity, cancellationToken);
        return task.IsCompleted 
            ? Task.FromResult(task.Result.Entity) 
            : Await();

        async Task<LogEntity> Await()
        {
            return (await task).Entity;
        }
    }


    /// <inheritdoc />
    public IAsyncEnumerable<IdNamePair> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return _context.Objects
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new IdNamePair(e.Id, e.Name))
            .AsAsyncEnumerable();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<LogViewModel> GetLogsForObjectById(int id, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return _context.Objects
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
        return _context.Objects
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => (ObjectViewModel?) new ObjectViewModel(e.Id, e.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Update(int id, ObjectEntity entity, CancellationToken cancellationToken)
    {
        var existing = await _context.Objects.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (existing is null)
        {
            throw new Exception("Not found"); // ideally use a more specific exception type
        }

        existing.Name = entity.Name;
    }

    public Task<int> Commit(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

}