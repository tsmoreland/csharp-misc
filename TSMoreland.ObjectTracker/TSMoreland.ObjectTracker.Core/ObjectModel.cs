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
