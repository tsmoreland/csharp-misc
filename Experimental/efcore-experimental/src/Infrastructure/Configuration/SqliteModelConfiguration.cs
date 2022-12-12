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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.CompiledModels;

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.Configuration;

public sealed class SqliteModelConfiguration : IModelConfiguration
{
    private readonly ILogger<SqliteModelConfiguration> _logger;
    private readonly string _connectionString;
    private readonly bool _isDevelopment;

    public SqliteModelConfiguration(
        IConfiguration configuration,
        IHostEnvironment environment,
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _logger = loggerFactory.CreateLogger<SqliteModelConfiguration>();

        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _isDevelopment = environment.IsDevelopment();
    }

    /// <inheritdoc />
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteModelConfiguration).Assembly);
    }

    /// <inheritdoc />
    public void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder
            .UseSqlite(
                _connectionString,
                options => options.MigrationsAssembly(typeof(SqliteModelConfiguration).Assembly.FullName))
            .LogTo(message => _logger.LogInformation("{SQL}", message))
            .EnableSensitiveDataLogging(_isDevelopment);

        // enable only for release builds under the assumption that the dbcontext may change frequently enough
        // to make it more awkward for debug builds
        if (!_isDevelopment)
        {
            optionsBuilder.UseModel(PeopleDbContextModel.Instance);
        }
    }
}
