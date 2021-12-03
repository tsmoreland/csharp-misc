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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}