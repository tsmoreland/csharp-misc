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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Entities;

namespace TSMoreland.Authorization.Demo.LocalUsers;

public sealed class AuthenticationDbContext : IdentityDbContext<DemoUser, DemoRole, Guid>
{
    private readonly IConfiguration _configuration;
    public const string ConnectionStringName = "AuthenticationDatabase";

    public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options, IConfiguration configuration)
        : base (options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public DbSet<DemoApiKey> ApiKeys { get; init; } = null!;

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder
            .UseSqlite(_configuration.GetConnectionString(ConnectionStringName),
                options => options.MigrationsAssembly(typeof(AuthenticationDbContext).FullName));

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        EntityTypeBuilder<DemoUser> userEntity = builder.Entity<DemoUser>();
        userEntity.ToTable("DemoUsers");
        
        EntityTypeBuilder<DemoRole> roleEntity = builder.Entity<DemoRole>();
        roleEntity.ToTable("DemoRole");

        builder.Entity<IdentityUserRole<Guid>>().ToTable("DemoUserRoles");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("DemoRoleClaims");

        EntityTypeBuilder<DemoApiKey> apiKeysEntity = builder.Entity<DemoApiKey>();
        apiKeysEntity.ToTable("DemoApiKeys");
        apiKeysEntity.HasKey(nameof(DemoApiKey.Id));
        apiKeysEntity.HasIndex(nameof(DemoApiKey.ApiKey));
        apiKeysEntity.HasIndex(nameof(DemoApiKey.Name));

        apiKeysEntity.Property(e => e.Id).IsRequired();
        apiKeysEntity.Property(e => e.Name).HasMaxLength(50).IsRequired().IsUnicode(false);
        apiKeysEntity.Property(e => e.UserId).IsRequired();
        apiKeysEntity
            .Property(e => e.NotBefore)
            .HasConversion(dateTime => dateTime.Ticks, ticks => new DateTime(ticks))
            .IsRequired();

        apiKeysEntity
            .Property(e => e.NotAfter)
            .HasConversion(
                dateTime =>
                    dateTime.HasValue ? dateTime.Value.Ticks : 0,
                ticks => ticks != 0 ? new DateTime(ticks) : null)
            .IsRequired(false);
    }
}
