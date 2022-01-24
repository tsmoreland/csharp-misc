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
    }
}
