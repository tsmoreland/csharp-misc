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

using Maple.Util.Internal;
using System;
using System.Diagnostics;

namespace Maple.Util
{
    [DebuggerDisplay("{Value} {Success} {Reason}")]
    public struct QueryResult<TValue> : IEquatable<QueryResult<TValue>>
    {
        private QueryResult(TValue value, bool success, string reason, Exception? cause = null)
        {
            ValueResult = new ValueResultCore<TValue>(value, success, reason, cause);
        }

        public static QueryResult<TValue> Ok(TValue value) => new QueryResult<TValue>(value, true, string.Empty);
        public static QueryResult<TValue> Failed(string reason, Exception? cause = null) => 
            new QueryResult<TValue>(default!, false, reason, cause); // allow default, which may be null in this case as it is a failure anyway and we shouldn't be accessing the value

        public TValue Value => Success ? ValueResult.Value : throw new InvalidOperationException("Cannot access resulting value when Query failed");
        public bool Success => ValueResult.Success;
        public string Reason => ValueResult.Reason;
        public Exception? Cause => ValueResult.Cause;

        public static bool operator==(QueryResult<TValue>? leftHandSide, QueryResult<TValue>? rightHandSide) => leftHandSide?.Equals(rightHandSide) == true;
        public static bool operator!=(QueryResult<TValue>? leftHandSide, QueryResult<TValue>? rightHandSide) => !(leftHandSide == rightHandSide);
        public static bool operator==(QueryResult<TValue> leftHandSide, QueryResult<TValue> rightHandSide) => leftHandSide.Equals(rightHandSide);
        public static bool operator!=(QueryResult<TValue> leftHandSide, QueryResult<TValue> rightHandSide) => !(leftHandSide == rightHandSide);

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
