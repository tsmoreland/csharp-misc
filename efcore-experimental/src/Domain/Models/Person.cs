namespace Tsmoreland.EntityFramework.Core.Experimental.Domain.Models;

public sealed class Person : IEquatable<Person>
{
    private string _firstname = string.Empty;

    public Person(int id, string lastname, string firstname)
    {
        Id = id;
        LastName = lastname;
        FirstName = firstname;
    }

    public Person(string lastname, string firstname)
        : this(0, lastname, firstname)
    {
    }

    private Person()
    {
        Id = 0;
        LastName = string.Empty;
        _firstname = string.Empty;
        // for ef-core
    }

    public int Id { get; init; }

    public string LastName { get; init; }

    public string FirstName
    {
        get => _firstname;
        set
        {
            if (value is not { Length: > 0 })
            {
                throw new ArgumentException("Invalid name, cannot be empty", nameof(value));
            }
            _firstname = value;
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    /// <inheritdoc />
    public bool Equals(Person? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id && LastName == other.LastName;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Person p && Equals(p);
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(Id, LastName);
}
