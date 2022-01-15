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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using TSMoreland.ObjectTracker.Data.Abstractions;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;

namespace TSMoreland.ObjectTracker.Data;

public sealed class ObjectContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ObjectContext(DbContextOptions<ObjectContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public DbSet<ObjectEntity> Objects { get; set; } = null!; // initialization will be handled by EF

    public Task<int> DeleteById(int id, CancellationToken cancellationToken)
    {
        return Database.ExecuteSqlRawAsync(@"BEGIN TRANSACTION;
DELETE FROM LogEntity WHERE ObjectEntityId = {0};
DELETE FROM Objects WHERE Id = {0};
COMMIT;", new object[] { id }, cancellationToken);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            DataBaseConnectionOptions options = _configuration
                .GetRequiredSection(DataBaseConnectionOptions.SectionName)
                .Get<DataBaseConnectionOptions>();

            string connectionString = options.Pooling
                ? "Data Source=objectTracker.db; Pooling = True;"
                : "Data Source=objectTracker.db";

            optionsBuilder.UseSqlite(connectionString, b =>
                b.MigrationsAssembly(typeof(ObjectContext).Assembly.FullName));
        }

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // table configuration for fluent design goes here, we don't need any yet as convention is enough
        ConfigureObject(modelBuilder.Entity<ObjectEntity>());
        ConfigureLog(modelBuilder.Entity<LogEntity>());

        static void ConfigureObject(EntityTypeBuilder<ObjectEntity> entity)
        {
            entity
                .HasIndex(t => t.Name)
                .IsUnique();
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(p => p.Progress)
                .IsRequired()
                .HasDefaultValue(0);
            entity.Property(p => p.LastModified)
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            OwnedNavigationBuilder<ObjectEntity, Address> ownedEntity = entity.OwnsOne(e => e.Address);
            ownedEntity.Property(p => p.HouseNumber)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("HouseNumber");
            ownedEntity.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode()
                .HasColumnName("Street");
            ownedEntity.Property(p => p.PostCode)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode()
                .HasColumnName("Postcode");
        }
        static void ConfigureLog(EntityTypeBuilder<LogEntity> entity)
        {
            entity.Property(p => p.Message)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(true);
            entity.Property(p => p.Severity)
                .IsRequired()
                .HasDefaultValue(0);
            entity
                .HasOne<ObjectEntity>()
                .WithMany(e => e.Logs)
                .HasForeignKey(e => e.ObjectEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
