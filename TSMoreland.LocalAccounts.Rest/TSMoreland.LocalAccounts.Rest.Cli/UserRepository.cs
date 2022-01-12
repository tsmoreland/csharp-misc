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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TSMoreland.LocalAccounts.Rest.Infrastructure.Abstractions.Entities;
using TSMoreland.LocalAccounts.Rest.Infrastructure.Data;

namespace TSMoreland.LocalAccounts.Rest.Cli;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(ApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbContext.Database.Migrate();
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    /// <inheritdoc />
    public void PrintAll()
    {
        var users = _dbContext.Users.Select(u => new {u.Id, u.UserName});
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}: {user.UserName}");
        }
    }

    public void Add(string username, string password)
    {
        User newUser = new() { UserName = username, };
        EntityEntry<User> user = _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        Update(user.Entity, password);
        Console.WriteLine("User added");
    }

    public void Upsert(int id, string username, string password)
    {
        User? user = _dbContext.Users.Find(id);
        if (user == null)
        {
            Add(username, password);
        }
        else
        {
            Update(user, password);
            Console.WriteLine("User updated");
        }
    }

    private void Update(User user, string password)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        User? user = _dbContext.Users.Find(id);
        if (user is null)
        {
            return;
        }

        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
        Console.WriteLine("User deleted");
    }

}
