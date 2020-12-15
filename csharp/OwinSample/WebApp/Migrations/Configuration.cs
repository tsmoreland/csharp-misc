using System.Data.Entity.Migrations;

namespace OwinSample.WebApp.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OwinSample.WebApp.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
