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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TSMoreland.Authorization.Demo.Middleware.Abstractions;

[Serializable]
public sealed class InvalidModelStateException : Exception
{

    public InvalidModelStateException(ModelStateDictionary modelState)
        : this(modelState, null, null)
    {
    }
    public InvalidModelStateException(ModelStateDictionary modelState, string? message)
        : this(modelState, message, null)
    {
    }
    public InvalidModelStateException(ModelStateDictionary modelState, string? message, Exception? innerException)
        :  base(message, innerException)
    {
        ModelState = modelState;
    }

    private InvalidModelStateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
        // no attempt to deserialize or serialize this for now
        ModelState = new ModelStateDictionary();
    }

    /// <summary>
    /// Model State containing errors which were the reason for the exception
    /// </summary>
    public ModelStateDictionary ModelState { get; }

    public static void ThrowIfNotValid(ModelStateDictionary modelState, string? message = null)
    {
        ArgumentNullException.ThrowIfNull(modelState, nameof(modelState));
        if (!modelState.IsValid)
        {
            throw new InvalidModelStateException(modelState, message);
        }
    }
}
