using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDomain.Infrastructure
{
    public sealed class DemoDbContext : IdentityDbContext<User>
    {
        public DemoDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        /// <inheritdoc cref="IdentityDbContext{User}.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(user => user.HasIndex(u => u.Role).IsUnique(false));

            builder.Entity<Country>(country =>
            {
                country.ToTable("Countries");
                country.HasKey(c => c.Id);

                country.HasMany<User>().WithOne().HasForeignKey(u => u.CountryId).IsRequired(false);
            });

        }
    }
}
