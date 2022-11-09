//
// Copyright (c) 2022 Terry Moreland
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

using System.Buffers;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using TSMoreland.Authorization.Demo.Authentication.Abstractions;

namespace TSMoreland.Authorization.Demo.BasicAuthentication;

internal static class StringValuesExtensions
{
    public static (string username, string password) GetBasicUsernameAndPasswordOrThrow(this in StringValues values)
    {
        string encodedAuth = values.FirstOrDefault() ??
                             throw new AuthenticationFailedException(AuthenticateResult.Fail("missing api key"));

        return encodedAuth.Length > 1024
            ? DecodeWithPoolAllocatedBufferOrThrow(encodedAuth)
            : DecodeWithStackAllocatedBufferOrThrow(encodedAuth);
    }

    private static (string Username, string Password) DecodeWithPoolAllocatedBufferOrThrow(string encoded)
    {
        byte[]? pooledBuffer = null;
        try
        {
            pooledBuffer = ArrayPool<byte>.Shared.Rent(encoded.Length);
            Span<byte> buffer = new(pooledBuffer);
            if (!Convert.TryFromBase64String(encoded, buffer, out int length))
            {
                 throw new AuthenticationFailedException(AuthenticateResult.Fail("authorization is not base64 encoded"));
            }
            buffer = buffer[..length];
            return DecocdeOrThrow(buffer);

        }
        finally
        {
            if (pooledBuffer is not null)
            {
                ArrayPool<byte>.Shared.Return(pooledBuffer);
            }
        }



    }

    private static (string Username, string Password) DecodeWithStackAllocatedBufferOrThrow(string encoded)
    {
        Span<byte> buffer = stackalloc byte[encoded.Length];
        if (!Convert.TryFromBase64String(encoded, buffer, out int length))
        {
             throw new AuthenticationFailedException(AuthenticateResult.Fail("authorization is not base64 encoded"));
        }
        buffer = buffer[..length];
        return DecocdeOrThrow(buffer);
    }

    private static (string Username, string Password) DecocdeOrThrow(in Span<byte> decoded)
    {
        int index = decoded.IndexOf(':');
        if (index == -1)
        {
            throw new AuthenticationFailedException(AuthenticateResult.Fail("malformed authorization value"));
        }

        Span<byte> rawUser = decoded[..index];
        index++;
        if (++index >= decoded.Length)
        {
            return (Encoding.UTF8.GetString(rawUser), string.Empty);
        }
        Span<byte> rawPassword = decoded[index..];

        return (Encoding.UTF8.GetString(rawUser), Encoding.UTF8.GetString(rawPassword));
    }

}
