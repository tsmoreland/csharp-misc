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

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TSMoreland.ObjectTracker.Data;

public sealed class DesignTimeObjectContextFactory : IDesignTimeDbContextFactory<ObjectContext>
{

    /// <inheritdoc />
    public ObjectContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ObjectContext>();
        optionsBuilder.UseSqlite("Data Source=objectTracker.db", b =>
            b.MigrationsAssembly(typeof(ObjectContext).Assembly.FullName));

        using MemoryStream stream = new();
        using (StreamWriter streamWriter = new(stream, new UTF8Encoding(false), 8196, true))
        {
            streamWriter.Write(@"{""databaseOptions"":{""pooling"":false}}");
            streamWriter.Flush();
        }
        stream.Seek(0, SeekOrigin.Begin);

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        return new ObjectContext(optionsBuilder.Options, configuration);
    }
}
