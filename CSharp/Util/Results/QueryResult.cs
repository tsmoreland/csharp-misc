﻿//
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

using Util.Internal;
using System;
using System.Diagnostics;
using Resources = Util.Properties.Resources;

namespace Util.Results
{
    public static class QueryResult
    {
        public static QueryResult<TValue> Ok<TValue>(TValue value) => new QueryResult<TValue>(value, true, string.Empty);
        public static QueryResult<TValue> Failed<TValue>(string reason, Exception? cause = null) => 
            new QueryResult<TValue>(default!, false, reason, cause); // allow default, which may be null in this case as it is a failure anyway and we shouldn't be accessing the value
    }

    /// <summary>Wrapper around the result of a query providing the value on success or a detailed reason or cause on failure</summary>
    /// <typeparam name="TValue"></typeparam>
    [DebuggerDisplay("{Value} {Success} {Reason}")]
    public struct QueryResult<TValue> : IEquatable<QueryResult<TValue>>
    {
        internal QueryResult(TValue value, bool success, string reason, Exception? cause = null)
        {
            ValueResult = new ValueResultCore<TValue>(value, success, reason, cause);
        }

        /// <summary>Resulting Value of the Query</summary>
        /// <exception cref="InvalidOperationException">thrown if <see cref="Success"/> is <c>false</c></exception>
        public TValue Value => Success ? ValueResult.Value : throw new InvalidOperationException(Resources.InvalidQueryResultValueAccess);
        /// <summary>The result of the operation</summary>
        public bool Success => ValueResult.Success;
        /// <summary>The reason for failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public string Reason => ValueResult.Reason;
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public Exception? Cause => ValueResult.Cause;
        public bool ToBoolean() => Success;

        public static bool operator==(QueryResult<TValue> leftHandSide, QueryResult<TValue> rightHandSide) => leftHandSide.Equals(rightHandSide);
        public static bool operator!=(QueryResult<TValue> leftHandSide, QueryResult<TValue> rightHandSide) => !(leftHandSide == rightHandSide);
        public static implicit operator bool(QueryResult<TValue> result) => result.ToBoolean();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by Value property")]
        public static explicit operator TValue(QueryResult<TValue> result) => result.Value;

        private ValueResultCore<TValue> ValueResult { get; }

        #region ValueType

        public override bool Equals(object? obj) => obj is QueryResult<TValue> rightHandSide ? Equals(rightHandSide) : false;
        public override int GetHashCode() => ValueResult.GetHashCode();

        #endregion
        #region IEquatable{QueryResult{TValue}}
        public bool Equals(QueryResult<TValue> other) =>
            ValueResult.Equals(other.ValueResult);


        #endregion
    }
}
