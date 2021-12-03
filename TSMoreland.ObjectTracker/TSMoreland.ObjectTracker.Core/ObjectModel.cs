using System.Diagnostics;

namespace TSMoreland.ObjectTracker.Core;

[DebuggerDisplay("{Id} {Name}")]
public sealed class ObjectModel : IEquatable<ObjectModel>
{
    private readonly List<LogModel> _logs = new();

    public ObjectModel(int id, string name)
        : this(id, name, Array.Empty<LogModel>())
    {
    }

    public ObjectModel(int id, string name, IEnumerable<LogModel> logs)
    {
        Id = id;
        Name = name;
        _logs.AddRange(logs);
    }

    public int Id { get; }
    public string Name { get; }

    public IEnumerable<LogModel> LogModels => _logs.AsEnumerable();

    public void Deconstruct(out int id, out string name, out IReadOnlyList<LogModel> logs)
    {
        id = Id;
        name = Name;
        logs = new List<LogModel>(_logs);
    }

    /// <inheritdoc />
    public bool Equals(ObjectModel? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ObjectModel other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(_logs, Id, Name);
    }
}