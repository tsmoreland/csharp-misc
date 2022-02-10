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

namespace TSMoreland.Authorization.Demo.LocalUsers.Abstractions.Entities;

internal class DemoApiKey
{
    public DemoApiKey(string name, DemoUser user, DateTime? notAfter = null)
        : this(name, user?.Id ?? throw new ArgumentNullException(nameof(user)), notAfter)
    {
    }

    public DemoApiKey(string name, Guid userId, DateTime? notAfter = null)
    {
        if (name is not { Length: > 0 })
        {
            throw new ArgumentException("Name cannot be empty");
        }

        Id = Guid.NewGuid();
        Name = name;
        UserId = userId;
        ApiKey = BuildApiKey();
        NotBefore = DateTime.UtcNow;
        NotAfter = notAfter;
    }

    private DemoApiKey()
    {
        // required by entity framework
    }

    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid UserId { get; init; }

    public string ApiKey { get; init; } = string.Empty;

    public DateTime NotBefore { get; init; } 

    public DateTime? NotAfter { get; init; } 

    private static string BuildApiKey()
    {
        Span<byte> apiKeyBytes = stackalloc byte[32];
        Span<byte> upperApiKeyBytes = apiKeyBytes[16..];

        Guid.NewGuid().TryWriteBytes(apiKeyBytes);
        Guid.NewGuid().TryWriteBytes(upperApiKeyBytes);

        return Convert.ToBase64String(apiKeyBytes);
    }
}
