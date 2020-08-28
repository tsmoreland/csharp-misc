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
using System.Linq;

namespace Moreland.CSharp.Util
{
#if NET40 || NET45 || NET46 || NET472 || NET48 || NETSTANDARD2_0
    /// <summary>
    /// Intended for use in .NET Standard 2.0 or .NET Framework 4.8 and below, 
    /// </summary>
#else
    /// <summary>
    /// Intended for use in .NET Standard 2.0 or .NET Framework 4.8 and below, 
    /// </summary>
    /// <remarks>
    /// <see cref="HashCode.Combine{T1}"/> variations should be preferred
    /// </remarks>
#endif
    [DebuggerDisplay("{_piecewiseCode}")]
    public sealed class HashCodeBuilder : IEquatable<HashCodeBuilder>, IComparable<HashCodeBuilder>
    {
        #region Public

        /// <summary>
        /// Creates a new HashCode Builder initialized using <paramref name="objects"/>
        /// </summary>
        public static HashCodeBuilder Create(params object?[] objects) => new HashCodeBuilder(objects);
        /// <summary>
        /// Updates the <see cref="HashCodeBuilder"/> with <paramref name="objects"/>
        /// </summary>
        public HashCodeBuilder WithAddedValues(params object?[] objects) =>
            new HashCodeBuilder(CalculateHashCode(_piecewiseCode, objects));
        /// <summary>
        /// Builds the HashCode using the stored values
        /// </summary>
        public int ToHashCode() => 13 * _piecewiseCode;

        /// <summary>
        /// returns <c>true</c> if <paramref name="leftHandSide"/> is equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator==(HashCodeBuilder? leftHandSide, HashCodeBuilder? rightHandSide) => 
            leftHandSide?._piecewiseCode == rightHandSide?._piecewiseCode;
        /// <summary>
        /// returns <c>true</c> if <paramref name="leftHandSide"/> is not equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator!=(HashCodeBuilder? leftHandSide, HashCodeBuilder? rightHandSide) => 
            !(leftHandSide == rightHandSide);
        /// <summary>
        /// returns <c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>
        /// </summary>
        public static bool operator <(HashCodeBuilder left, HashCodeBuilder right) =>
            ReferenceEquals(left, null!) ? !ReferenceEquals(right, null!) : left.CompareTo(right) < 0;
        /// <summary>
        /// returns <c>true</c> if <paramref name="left"/> is less or equal tothan <paramref name="right"/>
        /// </summary>
        public static bool operator <=(HashCodeBuilder left, HashCodeBuilder right) =>
            ReferenceEquals(left, null!) || left.CompareTo(right) <= 0;
        /// <summary>
        /// returns <c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>
        /// </summary>
        public static bool operator >(HashCodeBuilder left, HashCodeBuilder right) =>
            !ReferenceEquals(left, null!) && left.CompareTo(right) > 0;
        /// <summary>
        /// returns <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>
        /// </summary>
        public static bool operator >=(HashCodeBuilder left, HashCodeBuilder right) =>
            ReferenceEquals(left, null!) ? ReferenceEquals(right, null!) : left.CompareTo(right) >= 0;

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
        private static int CalculateHashCode(in int initialValue, object?[] objects) =>
            objects.Aggregate(initialValue, (current, @object) => (current * 397) ^ (@object?.GetHashCode() ?? 0));
        #endregion

        #region IComparable{HashCodeBuilder}
        /// <summary>
        /// <inheritdoc cref="IComparable{HashCodeBuilder}.CompareTo"/>
        /// </summary>
        public int CompareTo(HashCodeBuilder? other) =>
            other != null
            ? _piecewiseCode.CompareTo(other._piecewiseCode) 
            : 1; // 1 is based on the decompiled version of System.String's response to the equivalent criteria

        #endregion
        #region IEquatable{HashCodeBuilder}
        /// <summary>
        /// <inheritdoc cref="IEquatable{HashCodeBuilder}.Equals(HashCodeBuilder)"/>
        /// </summary>
        public bool Equals(HashCodeBuilder? other) =>
            other != null && other._piecewiseCode == _piecewiseCode;

        #endregion
        #region Object

        /// <summary>
        /// <inheritdoc cref="object.Equals(object?)"/>
        /// </summary>
        public override bool Equals(object? obj) => Equals(obj as HashCodeBuilder);
        /// <summary>
        /// <inheritdoc cref="object.GetHashCode"/>
        /// </summary>
        public override int GetHashCode() => _piecewiseCode.GetHashCode();

        #endregion
    }
}
