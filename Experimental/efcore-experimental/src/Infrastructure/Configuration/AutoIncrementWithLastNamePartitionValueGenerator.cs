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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Tsmoreland.EntityFramework.Core.Experimental.Domain.Models;

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.Configuration;

public sealed class AutoIncrementWithLastNamePartitionValueGenerator : ValueGenerator<int>
{
    /// <inheritdoc />
    public override int Next(EntityEntry entry)
    {
        if (entry.Context is not PeopleDbContext context)
        {
            throw new NotSupportedException();
        }

        if (entry.Entity is not Person person)
        {
            throw new NotSupportedException();
        }

        string lastname = person.LastName;
        int max = context.People.AsNoTracking()
            .Where(m => m.LastName == lastname)
            .Max(m => (int?)m.Id) ?? 0; // use of (int?) is to avoid exception when no matching rows found, bit of a hack/work around

        return max + 1;
    }

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;
}
