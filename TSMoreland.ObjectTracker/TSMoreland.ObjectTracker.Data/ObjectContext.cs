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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;

namespace TSMoreland.ObjectTracker.Data;

public sealed class ObjectContext : DbContext
{
    public ObjectContext(DbContextOptions<ObjectContext> options)
        : base(options)
    {
    }

    public DbSet<ObjectEntity> Objects { get; set; } = null!; // initialization will be handled by EF

    public Task<int> DeleteById(int id, CancellationToken cancellationToken)
    {
        return Database.ExecuteSqlRawAsync(@"BEBIN TRANSACTION;
DELETE FROM LogEntity WHERE ObjectEntityId = {0};    
DELETE FROM Objects WHERE Id = {0};
COMMIT;", new object[] { id }, cancellationToken);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=objectTracker.db", b =>
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
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(p => p.Progress)
                .IsRequired()
                .HasDefaultValue(0);
            entity.Property(p => p.LastModified)
                .HasDefaultValue(DateTime.MinValue)
                .IsConcurrencyToken();
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
        }
    }

}
