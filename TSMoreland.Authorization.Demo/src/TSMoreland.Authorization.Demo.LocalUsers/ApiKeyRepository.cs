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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Entities;
using TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Repositories;

namespace TSMoreland.Authorization.Demo.LocalUsers;

public sealed class ApiKeyRepository : IApiKeyRepository
{
    private readonly AuthenticationDbContext _dbContext;

    public ApiKeyRepository(AuthenticationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public ValueTask<DemoUser> GetUserFromApiKeyAsync(string apiKey, CancellationToken cancellationToken)
    {
        if (apiKey is not { Length: > 0 })
        {
            return ValueTask.FromException<DemoUser>(new ArgumentNullException(nameof(apiKey)));
        }

        return GetUserFromApiKey(_dbContext, apiKey, cancellationToken);

        static async ValueTask<DemoUser> GetUserFromApiKey(
            AuthenticationDbContext dbContext,
            string apiKey,
            CancellationToken cancellationToken)
        {
            Guid userId = await dbContext.ApiKeys
                .AsNoTracking()
                .Where(e => e.ApiKey == apiKey)
                .Select(e => e.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if (userId == Guid.Empty)
            {
                throw new ApiKeyNotFoundException();
            }

            DemoUser? user = await dbContext.Users
                .AsNoTracking()
                .Where(e => e.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            return user ?? throw new UserNotFoundException();
        }
    }

    /// <inheritdoc />
    public ValueTask<DemoApiKey> AddApiKeyForUserAsync(DemoUser user, string name, DateTime? notAfter, CancellationToken cancellationToken)
    {
        try
        {
            return AddApiKey(_dbContext, new DemoApiKey(name, user, notAfter), cancellationToken);
        }
        catch (Exception ex)
        {
            return ValueTask.FromException<DemoApiKey>(ex);
        }

        static async ValueTask<DemoApiKey> AddApiKey(AuthenticationDbContext dbContext, DemoApiKey apiKey, CancellationToken cancellationToken)
        {
            EntityEntry<DemoApiKey> apiKeyEntity = await dbContext.ApiKeys.AddAsync(apiKey, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return apiKeyEntity.Entity;
        }
    }

    /// <inheritdoc />
    public ValueTask RemoveApiKeyForUserAsync(string name, CancellationToken cancellationToken)
    {
        return name is not { Length: > 0 } id
            ? ValueTask.FromException(new ArgumentException("name must have a value", nameof(name)))
            : RemoveApiKeyForUser(_dbContext, name, cancellationToken);

        static async ValueTask RemoveApiKeyForUser(
            DbContext dbContext,
            string name,
            CancellationToken cancellationToken)
        {
            string normalizedName = name.ToUpperInvariant();

            _ = await dbContext.Database.ExecuteSqlRawAsync(
                @"DELETE FROM ApiKeys WHERE NormalizedName = {0}",
                new object[] { normalizedName },
                cancellationToken);
        }


    }
}
