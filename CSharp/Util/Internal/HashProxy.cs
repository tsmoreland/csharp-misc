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

#pragma warning disable S1128 // False postive regaarding unnecessary using
#if !(NETSTANDARD2_0 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48)
using System;
#endif
#pragma warning restore S1128 // False postive regaarding unnecessary using

namespace Moreland.CSharp.Util.Internal
{
    internal static class HashProxy
    {
#       if NETSTANDARD2_0 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
        /// <summary>uses <see cref="HashCodeBuilder"/> to build a hashcode for <paramref name="objects"/></summary>
        public static int Combine(params object?[] objects) => 
            HashCodeBuilder.Create(objects).ToHashCode();
#       else
        /// <summary><see cref="HashCode.Combine{T1}(T1)"/></summary>
        public static int Combine<TValue1>(TValue1 value1) =>
            HashCode.Combine(value1);
        /// <summary><see cref="HashCode.Combine{T1, T2}(T1, T2)"/></summary>
        public static int Combine<TValue1, TValue2>(TValue1 value1, TValue2 value2) =>
            HashCode.Combine(value1, value2);
        /// <summary><see cref="HashCode.Combine{T1, T2, T3}(T1, T2, T3)"/></summary>
        public static int Combine<TValue1, TValue2, TValue3>(TValue1 value1, TValue2 value2, TValue3 value3) =>
            HashCode.Combine(value1, value2, value3);
#       pragma warning disable S2436 // Types and methods should not have too many generic parameters
        /// <summary><see cref="HashCode.Combine{T1, T2, T3, T4}(T1, T2, T3, T4)"/></summary>
        public static int Combine<TValue1, TValue2, TValue3, TValue4>(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4) =>
            HashCode.Combine(value1, value2, value3, value4);
        /// <summary><see cref="HashCode.Combine{T1, T2, T3, T4, T5}(T1, T2, T3, T4, T5)"/></summary>
        public static int Combine<TValue1, TValue2, TValue3, TValue4, TValue5>(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5) =>
            HashCode.Combine(value1, value2, value3, value4, value5);
        /// <summary><see cref="HashCode.Combine{T1, T2, T3, T4, T5, T6}(T1, T2, T3, T4, T5, T6)"/></summary>
        public static int Combine<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6) =>
            HashCode.Combine(value1, value2, value3, value4, value5, value6);
        /// <summary><see cref="HashCode.Combine{T1, T2, T3, T4, T5, T6, T7}(T1, T2, T3, T4, T5, T6, T7)"/></summary>
        public static int Combine<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7) =>
            HashCode.Combine(value1, value2, value3, value4, value5, value6, value7);
#       pragma warning disable S107 // Methods should not have too many parameters
        /// <summary><see cref="HashCode.Combine{T1, T2, T3, T4, T5, T6, T7, T8}(T1, T2, T3, T4, T5, T6, T7, T8)"/></summary>
        public static int Combine<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7, TValue8 value8) =>
            HashCode.Combine(value1, value2, value3, value4, value5, value6, value7, value8);
#       pragma warning restore S107 // Methods should not have too many parameters
#       pragma warning restore S2436 // Types and methods should not have too many generic parameters
#       endif
    }
}
