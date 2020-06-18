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

namespace DotNet.Util.Internal
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value} {Success} {Message,nq}")]
    public struct ValueResultCore<TValue> : IEquatable<ValueResultCore<TValue>>
    {
        public ValueResultCore(TValue value, bool success, string message, Exception? cause)
        {
            Value = value;
            Result = new ResultCore(success, message, cause);
        }

        public TValue Value { get; }
        public bool Success => Result.Success;
        public string Message => Result.Message;
        public Exception? Cause => Result.Cause;
        public static bool operator==(ValueResultCore<TValue> leftHandSide, ValueResultCore<TValue> rightHandSide) => leftHandSide.Equals(rightHandSide);
        public static bool operator!=(ValueResultCore<TValue> leftHandSide, ValueResultCore<TValue> rightHandSide) => !(leftHandSide == rightHandSide);

        private ResultCore Result { get; } 

        #region IEquatable{QueryResult{TValue}}
        public bool Equals(ValueResultCore<TValue> other) =>
            Value?.Equals(other.Value) == true &&
            Result.Equals(other.Result);
        #endregion
        #region ValueType

        public override bool Equals(object? obj) => obj is ValueResultCore<TValue> rightHandSide && Equals(rightHandSide);
        public override int GetHashCode() =>
#           if NETSTANDARD2_0 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
            HashCodeBuilder.Create(Value, Result).ToHashCode();
            #else
            HashCode.Combine(Value, Result);
            #endif

        #endregion
    }
}
