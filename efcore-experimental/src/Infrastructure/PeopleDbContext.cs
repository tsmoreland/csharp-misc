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
using Tsmoreland.EntityFramework.Core.Experimental.Domain.Models;
using Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.Configuration;

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure;

public sealed class PeopleDbContext : DbContext
{
    private readonly IModelConfiguration _modelConfiguration;

    /// <inheritdoc />
    public PeopleDbContext(DbContextOptions options, IModelConfiguration modelConfiguration)
        : base(options)
    {
        _modelConfiguration = modelConfiguration ?? throw new ArgumentNullException(nameof(modelConfiguration));
    }

    public DbSet<Person> People { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Person).Assembly);
        _modelConfiguration.ConfigureModel(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _modelConfiguration.ConfigureContext(optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }
}
