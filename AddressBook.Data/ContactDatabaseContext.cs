//
// Copyright © 2020 Terry Moreland
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

using AddressBook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;

namespace AddressBook.Data
{
    public class ContactDatabaseContext : DbContext
    {
        public DbSet<ContactEntity> Contacts { get; init; } = null!;

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            const string connectionString = "Data Source=ContactList.db;Cache=Shared"; // this should come from options injected into the constructor
            optionsBuilder
                .UseSqlite(connectionString)
                .LogTo(Console.WriteLine, new[] {DbLoggerCategory.Database.Command.Name}, LogLevel.Information); // logger should not be console, other settings should come from injected options
        }


        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            const string schema = "AddressBook";

            var entity = modelBuilder.Entity<ContactEntity>();
            entity.ToTable("Contacts", schema);
            entity.HasKey(c => c.Id);

            entity.Property(c => c.CompleteName)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(c => c.GivenName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(c => c.MiddleName)
                .HasMaxLength(100);
            entity.Property(c => c.Surname)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
