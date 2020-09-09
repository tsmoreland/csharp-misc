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


using System;

namespace Moreland.CSharp.Util.Results
{
    /// <summary>
    /// Common Value Result definitions
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IValueResult<TValue>
    {
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        Exception? Cause { get; }
        /// <summary>Optional message; should be non-null on failure but may contain a value on success</summary>
        string Message { get; }
        /// <summary>The result of the operation</summary>
        bool Success { get; }
        /// <summary>Resulting Value</summary>
        /// <exception cref="InvalidOperationException">thrown if <see cref="Success"/> is <c>false</c></exception>
        TValue Value { get; }

        /// <summary>Deconstructs the components of <see cref="IValueResult{TValue}"/> into seperate variables</summary>
        void Deconstruct(out bool success, out Maybe<TValue> value);
        /// <summary>Deconstructs the components of <see cref="IValueResult{TValue}"/> into seperate variables</summary>
        void Deconstruct(out bool success, out Maybe<TValue> value, out string message);
        /// <summary>Deconstructs the components of <see cref="IValueResult{TValue}"/> into seperate variables</summary>
        void Deconstruct(out bool success, out Maybe<TValue> value, out string message, out Exception? cause);
        /// <summary>Returns the value if present, otherwise returns <paramref name="other"/>.</summary>
        /// <remarks>slightly awkward name due to OrElse being reserved keyword (VB)</remarks>
        TValue ValueOr(TValue other);
        /// <summary>Returns the value if present, otherwise invokes other and returns the result of that invocation.</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="other"/> is <c>null</c></exception>
        TValue ValueOr(Func<TValue> other);
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="Success"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        TValue ValueOrThrow(Func<Exception> exceptionSupplier);
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <param name="exceptionSupplier">exception builder taking the message and optional Exception that caused the failiure</param>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="Success"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        TValue ValueOrThrow(Func<string, Exception?, Exception> exceptionSupplier);
        /// <summary>
        /// Returns <see cref="Success"/>
        /// </summary>
        bool ToBoolean();
    }
}
