using TSMoreland.ObjectTracker.Data.Abstractions.Entities;
using TSMoreland.ObjectTracker.Data.Abstractions.ViewModels;

namespace TSMoreland.ObjectTracker.Data.Abstractions;

public interface IObjectRepository
{
    Task<ObjectEntity> Add(ObjectEntity entity, CancellationToken cancellationToken);
    Task<LogEntity> AddMessage(int id, LogEntity entity, CancellationToken cancellationToken);
    IAsyncEnumerable<IdNamePair> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
    IAsyncEnumerable<LogViewModel> GetLogsForObjectById(int id, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ObjectViewModel?> GetById(int id, CancellationToken cancellationToken);
    Task Update(int id, ObjectEntity entity, CancellationToken cancellationToken);
    Task<int> Commit(CancellationToken cancellationToken);
}