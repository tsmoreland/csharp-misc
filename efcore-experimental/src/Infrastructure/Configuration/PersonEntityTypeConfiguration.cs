using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsmoreland.EntityFramework.Core.Experimental.Domain.Models;

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.Configuration;

public sealed class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .ToTable("people");

        builder
            .Property(m => m.LastName)
            .HasColumnName("last_name");

        builder
            .Property(m => m.Id)
            .HasColumnName("id");

        builder
            .Property<string>("_firstName")
            .HasColumnName("first_name");

        builder
            .Property(m => m.Id)
            .HasValueGenerator<AutoIncrementWithLastNamePartitionValueGenerator>();

        builder.Ignore(m => m.FullName);
    }
}
