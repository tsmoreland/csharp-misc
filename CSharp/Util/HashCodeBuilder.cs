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

namespace Moreland.CSharp.Util
{
    /// <summary>
    /// Intended for use in .NET Standard 2.0 or .NET Framework 4.8 and below, 
    /// <see cref="HashCode.Combine{T1}(T1)"/> variations should be preferred
    /// if available
    /// </summary>
    [DebuggerDisplay("{_piecewiseCode}")]
    public sealed class HashCodeBuilder : IEquatable<HashCodeBuilder>, IComparable<HashCodeBuilder>
    {
        #region Public
        public static HashCodeBuilder Create(params object?[] objects) => new HashCodeBuilder(objects);
        public HashCodeBuilder WithAddedValues(params object?[] objects) =>
            new HashCodeBuilder(CalculateHashCode(_piecewiseCode, objects));
        public int ToHashCode() => 13 * _piecewiseCode;

        public static bool operator==(HashCodeBuilder? leftHandSide, HashCodeBuilder? rightHandSide) => 
            leftHandSide?._piecewiseCode == rightHandSide?._piecewiseCode;
        public static bool operator!=(HashCodeBuilder? leftHandSide, HashCodeBuilder? rightHandSide) => 
            !(leftHandSide == rightHandSide);
        public static bool operator <(HashCodeBuilder left, HashCodeBuilder right) =>
            left is null ? right is object : left.CompareTo(right) < 0;
        public static bool operator <=(HashCodeBuilder left, HashCodeBuilder right) =>
            left is null || left.CompareTo(right) <= 0;
        public static bool operator >(HashCodeBuilder left, HashCodeBuilder right) =>
            left is object && left.CompareTo(right) > 0;
        public static bool operator >=(HashCodeBuilder left, HashCodeBuilder right) =>
            left is null ? right is null : left.CompareTo(right) >= 0;

        #endregion
        #region Private
        private readonly int _piecewiseCode;

        private HashCodeBuilder(params object?[] objects) : this(CalculateHashCode(0, objects))
        {
        }
        private HashCodeBuilder(int piecewiseHashCodeBuilder)
        {
            _piecewiseCode = piecewiseHashCodeBuilder;
        }
        private static int CalculateHashCode(in int initialValue, object?[] objects)
        {
            var result = initialValue;
            foreach (var @object in objects)
                result = (result * 397) ^ (@object?.GetHashCode() ?? 0);
            return result;
        }
        #endregion

        #region IComparable{HashCodeBuilder}
        public int CompareTo(HashCodeBuilder? other) =>
            other is HashCodeBuilder hashCode 
            ? _piecewiseCode.CompareTo(hashCode._piecewiseCode) 
            : 1; // 1 is based on the decompiled version of System.String's response to the equivalent criteria

        #endregion
        #region IEquatable{HashCodeBuilder}
        public bool Equals(HashCodeBuilder? other) =>
            other != null && other._piecewiseCode == _piecewiseCode;

        #endregion
        #region Object

        public override bool Equals(object? obj) => Equals(obj as HashCodeBuilder);
        public override int GetHashCode() => _piecewiseCode.GetHashCode();

        #endregion
    }
}
