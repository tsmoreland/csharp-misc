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

using Moreland.CSharp.Util.Internal;
using System;
using System.Diagnostics;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Results
{
    /// <summary>
    /// Factory methods for <see cref="QueryResult{T}"/>
    /// </summary>
    public static class QueryResult
    {
        /// <summary>
        /// Factory method for successful query with result <paramref name="value"/>
        /// </summary>
        public static QueryResult<TValue> Ok<TValue>(TValue value) => 
            new QueryResult<TValue>(value, true, string.Empty);
        /// <summary>
        /// Factory methods for successful query with result <paramref name="value"/> and <paramref name="message"/>
        /// </summary>
        public static QueryResult<TValue> Ok<TValue>(TValue value, string message) => 
            new QueryResult<TValue>(value, true, message ?? string.Empty);
        /// <summary>
        /// Factory method for Failed Query
        /// </summary>
        /// <typeparam name="TValue">the type of returned <see cref="QueryResult{TValue}"/></typeparam>
        /// <param name="message">message detailed while the query failed</param>
        /// <param name="cause">optional exception providing further details</param>
        public static QueryResult<TValue> Failed<TValue>(string message, Exception? cause = null) => 
            new QueryResult<TValue>(default!, false, message ?? throw new ArgumentNullException(nameof(message)), cause); // allow default, which may be null in this case as it is a failure anyway and we shouldn't be accessing the value
    }

    /// <summary>Wrapper around the result of a query providing the value on success or a detailed reason or cause on failure</summary>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("{GetType().Name,nq}: {Value} {Success} {Message,nq}")]
    public readonly struct QueryResult<TValue> : IValueResult<TValue>, IEquatable<QueryResult<TValue>>
    {
        internal QueryResult(TValue value, bool success, string message, Exception? cause = null)
        {
            ValueResult = new ValueResultCore<TValue>(value, success, message, cause);
        }

        /// <summary>Resulting Value of the Query</summary>
        /// <exception cref="InvalidOperationException">thrown if <see cref="Success"/> is <c>false</c></exception>
        public TValue Value => Success 
            ? ValueResult.Value 
            : throw new InvalidOperationException(ProjectResources.InvalidResultValueAccess);

        /// <summary>The result of the operation</summary>
        public bool Success => ValueResult.Success;
        /// <summary>Optional message; should be non-null on failure but may contain a value on success</summary>
        public string Message => ValueResult.Message;
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public Exception? Cause => ValueResult.Cause;
        /// <summary>
        /// Returns <see cref="Success"/>
        /// </summary>
        public bool ToBoolean() => Success;

        /// <summary>
        /// Returns <c>true</c> if <paramref name="leftHandSide"/> is equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator ==(QueryResult<TValue> leftHandSide, QueryResult<TValue> rightHandSide) => 
            leftHandSide.Equals(rightHandSide);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="leftHandSide"/> is not equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator !=(QueryResult<TValue> leftHandSide, QueryResult<TValue> rightHandSide) => 
            !(leftHandSide == rightHandSide);

        /// <summary>
        /// Returns <see cref="ToBoolean"/>
        /// </summary>
        public static implicit operator bool(QueryResult<TValue> result) => result.ToBoolean();
        /// <summary>
        /// Returns <see cref="Value"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">if the <paramref name="result"/> does not have a value</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by Value property")]
        public static explicit operator TValue(QueryResult<TValue> result) => result.Value;

        /// <summary>Deconstructs the components of <see cref="QueryResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out Maybe<TValue> value)
        {
            success = Success;
            value = Success ? Maybe.Of(Value) : Maybe.Empty<TValue>();
        }

        /// <summary>Deconstructs the components of <see cref="QueryResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out Maybe<TValue> value, out string message)
        {
            success = Success;
            value = Success ? Maybe.Of(Value) : Maybe.Empty<TValue>();
            message = Message;
        }
        /// <summary>Deconstructs the components of <see cref="QueryResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out Maybe<TValue> value, out string message, out Exception? cause)
        {
            success = Success;
            value = Success ? Maybe.Of(Value) : Maybe.Empty<TValue>();
            message = Message;
            cause = Cause;
        }


        /// <summary>
        /// If a value is present, apply the provided Optional-bearing mapping function to it, 
        /// return that result, otherwise return an empty Optional.
        /// </summary>
        public QueryResult<TMappedValue> Select<TMappedValue>(Func<TValue, QueryResult<TMappedValue>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return !Success 
                ? QueryResult.Failed<TMappedValue>(Message, Cause) 
                : selector.Invoke(Value);
        }

        /// <summary>
        /// If a value is present, apply the provided mapping function to it, 
        /// and if the result is non-null, return an Optional describing the result. 
        /// </summary>
        public QueryResult<TMappedValue> Select<TMappedValue>(Func<TValue, TMappedValue> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return !Success 
                ? QueryResult.Failed<TMappedValue>(Message, Cause) 
                : QueryResult.Ok(selector.Invoke(Value));
        }

        /// <summary>Returns the value if present, otherwise returns <paramref name="other"/>.</summary>
        /// <remarks>slightly awkward name due to OrElse being reserved keyword (VB)</remarks>
        public TValue ValueOr(TValue other) => Success 
            ? Value 
            : other;

        /// <summary>Returns the value if present, otherwise invokes supplier and returns the result of that invocation.</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="supplier"/> is <c>null</c></exception>
        public TValue ValueOr(Func<TValue> supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));
            return Success ? Value : supplier.Invoke();
        }

        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="Success"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue ValueOrThrow(Func<Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (Success)
                return Value;
            throw exceptionSupplier.Invoke();
        }
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <param name="exceptionSupplier">exception builder taking the message and optional Exception that caused the failiure</param>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="Success"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue ValueOrThrow(Func<string, Exception?, Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (Success)
                return Value;
            throw exceptionSupplier.Invoke(Message, Cause);
        }

        /// <summary>
        /// Returns the current result if successful or uses <paramref name="exceptionSupplier"/> to handle the error
        /// allow the result to be converted to an alternate successful result
        /// </summary>
        /// <param name="exceptionSupplier">
        /// Functor given message and 
        /// exception cause and returns a <see cref="QueryResult{TValue}"/> to an alternate result
        /// </param>
        /// <returns>
        /// current result if successful or uses <paramref name="exceptionSupplier"/> to handle the error
        /// allow the result to be converted to an alternate successful result
        /// </returns>
        public QueryResult<TValue> ValueOrThrow(Func<string, Exception?, QueryResult<TValue>> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));

            return Success
                ? this
                : exceptionSupplier.Invoke(Message, Cause);
        }

        private ValueResultCore<TValue> ValueResult { get; }

        #region IEquatable{QueryResult{TValue}}
        /// <summary>
        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        /// </summary>
        public bool Equals(QueryResult<TValue> other) =>
            ValueResult.Equals(other.ValueResult);

        #endregion
        #region ValueType

        /// <summary>
        /// <inheritdoc cref="ValueType.Equals(object?)"/>
        /// </summary>
        public override bool Equals(object? obj) => 
            obj is QueryResult<TValue> rightHandSide && Equals(rightHandSide);
        /// <summary>
        /// <inheritdoc cref="ValueType.GetHashCode"/>
        /// </summary>
        public override int GetHashCode() => 
            ValueResult.GetHashCode();

        #endregion
    }
}
