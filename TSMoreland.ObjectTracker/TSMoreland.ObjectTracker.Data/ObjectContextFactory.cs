using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TSMoreland.ObjectTracker.Data;

public sealed class ObjectContextFactory : IDesignTimeDbContextFactory<ObjectContext>
{
    /// <inheritdoc />
    public ObjectContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ObjectContext>();
        optionsBuilder.UseSqlite("Data Source=objectTracker.db", b => 
            b.MigrationsAssembly(typeof(ObjectContext).Assembly.FullName));

        return new ObjectContext(optionsBuilder.Options);
    }
}