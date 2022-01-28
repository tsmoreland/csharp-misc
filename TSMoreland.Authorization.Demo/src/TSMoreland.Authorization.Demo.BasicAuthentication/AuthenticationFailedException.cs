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

using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace TSMoreland.Authorization.Demo.BasicAuthentication;

[Serializable]
public class AuthenticationFailedException : Exception
{

    public AuthenticationFailedException(AuthenticateResult result)
        : this(result, null, null)
    {
    }
    public AuthenticationFailedException(AuthenticateResult result, string? message)
        : this(result, message, null)
    {
    }
    public AuthenticationFailedException(AuthenticateResult result, string? message, Exception? innerException)
        : base(message, innerException)
    {
        Result = result;
    }

    protected AuthenticationFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
        string? serialized = serializationInfo.GetString(nameof(Result));
        AuthenticateResult? result = null;
        if (serialized is { Length: > 0 })
        {
            try
            {
                result = JsonSerializer.Deserialize<AuthenticateResult>(serialized);
            }
            catch (Exception)
            {
                // ... ignore, we can't deserilize
            }
        }
        Result = result ?? AuthenticateResult.Fail("Unable to deserialize");
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        string serialized;
        try
        {
            serialized = JsonSerializer.Serialize(Result);
        }
        catch (Exception)
        {
            serialized = string.Empty;
        }
        info.AddValue(nameof(Result), serialized);
    }

    public AuthenticateResult Result { get; }
}
