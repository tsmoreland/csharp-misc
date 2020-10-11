//
// Copyright © 2020 Terry Moreland
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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityDomain.Infrastructure
{
    public sealed class DemoDbContext : IdentityDbContext<DemoUser>
    {
        public DemoDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Country> Countries { get; set; } = null!; 

        /// <inheritdoc cref="IdentityDbContext{DemoUser}.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            const string schema = "demo";

            builder.Entity<Country>().ToTable("Countries", schema);
            builder.Entity<Country>().HasData(
                Country.None,
                new Country { Id = "CAN", Name = "Canada" },
                new Country { Id = "GBR", Name = "United Kingdom" },
                new Country { Id = "USA", Name = "United States" }
            );
            builder.Entity<DemoUser>(DemoUserBuilder);
            builder.Entity<IdentityRole>().ToTable("Role", schema);
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schema);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schema);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", schema);
            builder.Entity<IdentityUserLogin<string>>().ToTable("Logins", schema);
            builder.Entity<IdentityUserToken<string>>().ToTable("Tokens", schema);

            static void DemoUserBuilder(EntityTypeBuilder<DemoUser> userBuilder)
            {
                userBuilder.ToTable("DemoUsers", schema);
                userBuilder.Property(u => u.CountryId).IsRequired();
            }
        }

    }
}
