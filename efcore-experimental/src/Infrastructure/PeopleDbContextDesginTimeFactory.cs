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
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.Configuration;

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure;

public sealed class PeopleDbContextDesginTimeFactory : IDesignTimeDbContextFactory<PeopleDbContext>
{
    /// <inheritdoc />
    public PeopleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PeopleDbContext>();
        optionsBuilder
            .EnableSensitiveDataLogging(true)
            .LogTo(Console.WriteLine)
            .UseSqlite(
                "Data Source=designTime.db",
                options => options
                    .MigrationsAssembly(typeof(SqliteModelConfiguration).Assembly.FullName));

        return new PeopleDbContext(
            optionsBuilder.Options,
            new SqliteModelConfiguration(
                new ConfigurationBuilder().Build(),
                new DesignTimeEnvironment(),
                new LoggerFactory()));
    }

    private sealed class DesignTimeEnvironment : IHostEnvironment
    {
        /// <inheritdoc />
        public string ApplicationName { get; set; } = string.Empty;

        /// <inheritdoc />
        public IFileProvider ContentRootFileProvider { get; set; } = null!;

        /// <inheritdoc />
        public string ContentRootPath { get; set; } = string.Empty;

        /// <inheritdoc />
        public string EnvironmentName { get; set; } = "DesignTime";
    }
}
