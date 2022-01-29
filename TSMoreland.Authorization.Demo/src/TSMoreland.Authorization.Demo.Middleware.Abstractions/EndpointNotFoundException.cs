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

using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace TSMoreland.Authorization.Demo.Middleware.Abstractions;

[Serializable]
public sealed class EndpointNotFoundException : Exception
{
    public EndpointNotFoundException(Uri? endpoint)
        : this(endpoint, null, null)
    {
    }
    public EndpointNotFoundException(Uri? endpoint, string? message)
        : this(endpoint, message, null)
    {
    }

    public EndpointNotFoundException(Uri? endpoint, Exception? innerException)
        : this(endpoint, null, innerException)
    {
    }

    public EndpointNotFoundException(Uri? endpoint, string? message, Exception? innerException)
        : base(message, innerException)
    {
        Endpoint = endpoint;
    }

    private EndpointNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        string? endpoint = info.GetString(nameof(Endpoint));

        if (endpoint is {Length:>0} && Uri.TryCreate(endpoint, new UriCreationOptions(), out Uri? endpointUri))
        {
            Endpoint = endpointUri;
        }
    }

    public Uri? Endpoint { get; }

    /// <inheritdoc/>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Endpoint), Endpoint?.ToString());
    }

    public static void ThrowIfStatusCodeIs404(HttpContext? httpContext)
    {
        if (httpContext?.Response.StatusCode is not StatusCodes.Status404NotFound)
        {
            return;
        }
    }
}
