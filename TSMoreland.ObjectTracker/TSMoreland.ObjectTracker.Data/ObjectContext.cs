using Microsoft.EntityFrameworkCore;
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;

namespace TSMoreland.ObjectTracker.Data;

public sealed class ObjectContext : DbContext
{
    public ObjectContext(DbContextOptions<ObjectContext> options)
        : base(options)
    {
    }

    public DbSet<ObjectEntity> Objects { get; set; } = null!; // initialization will be handled by EF

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=objectTracker.db", b => 
            b.MigrationsAssembly(typeof(ObjectContext).Assembly.FullName));

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // table configuration for fluent design goes here, we don't need any yet as convention is enough
    }

}
