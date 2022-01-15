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

namespace TSMoreland.ObjectTracker.Data;

public sealed class TenantDbContextFactory : ITenantDbContextFactory
{
    private readonly IConfiguration _configuration;

    public TenantDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc />
    public ObjectContext CreateDbContext(string tenantName)
    {
        string filename = $"{tenantName}.db";
        var optionsBuilder = new DbContextOptionsBuilder<ObjectContext>();
        optionsBuilder.UseSqlite($"Data Source={filename}", b =>
            b.MigrationsAssembly(typeof(ObjectContext).Assembly.FullName));

        var context = new ObjectContext(optionsBuilder.Options, _configuration);

        // no need to migrate here, these are intended as short-lived databases so migration "shouldn't" be necessary
        // on the other hand if they live longer than intended then migration may be needed but that would mean
        // needing to migrate every time as we couldn't know the version offhand without checking
        context.Database.EnsureCreated();

        // Doing this means we won't get a Migration table so we won't be able to migrate at a future point so this
        // only works for a short lived database


        return context;

    }
}
