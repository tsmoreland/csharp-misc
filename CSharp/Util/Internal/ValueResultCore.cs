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
using System.Diagnostics;

namespace Maple.Util.Internal
{
    [DebuggerDisplay("{Value} {Success} {Reason}")]
    public struct ValueResultCore<TValue> : IEquatable<ValueResultCore<TValue>>
    {
        public ValueResultCore(TValue value, bool success, string reason, Exception? cause)
        {
            Value = value;
            Result = new ResultCore(success, reason, cause);
        }

        public TValue Value { get; }
        public bool Success => Result.Success;
        public string Reason => Result.Reason;
        public Exception? Cause => Result.Cause;

        private ResultCore Result { get; }

        #region ValueType

        public override bool Equals(object? obj) => obj is ValueResultCore<TValue> rightHandSide ? Equals(rightHandSide) : false;
        public override int GetHashCode() => HashCodeBuilder.Create(Value, Result).Build();

        #endregion
        #region IEquatable{QueryResult{TValue}}
        public bool Equals(ValueResultCore<TValue> other) =>
            Value?.Equals(other.Value) == true &&
            Result.Equals(other.Result);
        #endregion
    }
}
