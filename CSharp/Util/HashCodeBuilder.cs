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

namespace Maple.Util
{
    [DebuggerDisplay("{_piecewiseHashCodeBuilder}")]
    public class HashCodeBuilder : IEquatable<HashCodeBuilder>, IComparable<HashCodeBuilder>
    {
        #region Public
        public static HashCodeBuilder Create(params object?[] objects) => new HashCodeBuilder(objects);
        public HashCodeBuilder AddValues(params object?[] objects) =>
            new HashCodeBuilder(objects.Select(o => (o?.GetHashCode() ?? 0) * 7)?.Sum() ?? 0);
        public int Build() => 31 * _piewiseCode;

        public static bool operator==(HashCodeBuilder? leftHandSide, HashCodeBuilder? rightHandSide) => leftHandSide?._piewiseCode == rightHandSide?._piewiseCode;
        public static bool operator!=(HashCodeBuilder? leftHandSide, HashCodeBuilder? rightHandSide) => !(leftHandSide == rightHandSide);

        #endregion
        #region Private
        private readonly int _piewiseCode;

        private HashCodeBuilder() : this(0)
        {
        }
        private HashCodeBuilder(params object?[] objects) : this(objects.Select(o => (o?.GetHashCode() ?? 0) * 7)?.Sum() ?? 0)
        {
        }
        private HashCodeBuilder(int piecewiseHashCodeBuilder)
        {
            _piewiseCode = piecewiseHashCodeBuilder;
        }
        #endregion

        #region Object

        public override bool Equals(object? obj) => Equals(obj as HashCodeBuilder);
        public override int GetHashCode() => _piewiseCode.GetHashCode();

        #endregion
        #region IEquatable{HashCodeBuilder}
        public bool Equals(HashCodeBuilder? other) =>
            other != null && other._piewiseCode == _piewiseCode;

        #endregion
        #region IComparable{HashCodeBuilder}
        public int CompareTo(HashCodeBuilder? other) =>
            other is HashCodeBuilder hashCode 
            ? _piewiseCode.CompareTo(hashCode._piewiseCode) 
            : 1; // 1 is based on the decompiled version of System.String's response to the equivalent criteria
        #endregion
    }
}
