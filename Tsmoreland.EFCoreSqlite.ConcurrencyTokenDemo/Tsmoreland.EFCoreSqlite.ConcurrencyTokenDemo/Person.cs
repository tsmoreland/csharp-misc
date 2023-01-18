// 
// Copyright (c) 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Tsmoreland.EFCoreSqlite.ConcurrencyTokenDemo;

public sealed class Person
{
    public Person(string firstName, string lastName, Address address, string email)
        : this(Guid.NewGuid(), firstName, lastName, address, email, Guid.NewGuid().ToString("N"))
    {
    }
    private Person(Guid id, string firstName, string lastName, Address address, string email, string timestamp)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Email = email;
        TimeStamp = timestamp;
    }
    private Person()
    {
        // used by ef 
    }

    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public Address Address { get; set; } = Address.None;

    public string Email { get; set; } = string.Empty;

    public string TimeStamp { get; init; } = string.Empty;
}
