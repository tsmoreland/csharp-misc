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
using System.Globalization;

namespace Moreland.CSharp.Util
{
    /// <summary>
    /// Alternate to standard TryParse methods which return <see cref="Maybe"/> instead of relying on out parameter
    /// </summary>
    public static class NumericParser
    {
        /// <summary>
        /// Converts the string represenation of a number to its 32-bit signed Shorteger equivalent
        /// </summary>
        /// <returns><see cref="Maybe{Short}"/> containing a value on success or empty on failure</returns>
        public static Maybe<short> MaybeParseShort(string? s, NumberStyles style = NumberStyles.Any, IFormatProvider? provider = null) => 
            short.TryParse(s, style, provider, out short result)
            ? Maybe.Of(result)
            : Maybe.Empty<short>();

        /// <summary>
        /// Converts the string represenation of a number to its 32-bit signed integer equivalent
        /// </summary>
        /// <returns><see cref="Maybe{int}"/> containing a value on success or empty on failure</returns>
        public static Maybe<int> MaybeParseInt(string? s, NumberStyles style = NumberStyles.Any, IFormatProvider? provider = null) => 
            int.TryParse(s, style, provider, out int result)
            ? Maybe.Of(result)
            : Maybe.Empty<int>();

        /// <summary>
        /// Converts the string represenation of a number to its 64-bit signed integer equivalent
        /// </summary>
        /// <returns><see cref="Maybe{long}"/> containing a value on success or empty on failure</returns>
        public static Maybe<long> MaybeParseLong(string? s, NumberStyles style = NumberStyles.Any, IFormatProvider? provider = null) => 
            long.TryParse(s, style, provider, out long result)
            ? Maybe.Of(result)
            : Maybe.Empty<long>();

        /// <summary>
        /// Converts the string represenation of a number to its double-precision floating point equivalent equivalent
        /// </summary>
        /// <returns><see cref="Maybe{double}"/> containing a value on success or empty on failure</returns>
        public static Maybe<double> MaybeParseDouble(string? s, NumberStyles style = NumberStyles.Any, IFormatProvider? provider = null) => 
            double.TryParse(s, style, provider, out double result)
            ? Maybe.Of(result)
            : Maybe.Empty<double>();

        /// <summary>
        /// Converts the string represenation of a number to its float-precision floating point equivalent equivalent
        /// </summary>
        /// <returns><see cref="Maybe{float}"/> containing a value on success or empty on failure</returns>
        public static Maybe<float> MaybeParseFloat(string? s, NumberStyles style = NumberStyles.Any, IFormatProvider? provider = null) => 
            float.TryParse(s, style, provider, out float result)
            ? Maybe.Of(result)
            : Maybe.Empty<float>();

        /// <summary>
        /// Converts the string represenation of a number to its float-precision floating point equivalent equivalent
        /// </summary>
        /// <returns><see cref="Maybe{Decimal}"/> containing a value on success or empty on failure</returns>
        public static Maybe<decimal> MaybeParseDecimal(string? s, NumberStyles style = NumberStyles.Any, IFormatProvider? provider = null) => 
            decimal.TryParse(s, style, provider, out decimal result)
            ? Maybe.Of(result)
            : Maybe.Empty<decimal>();
    }
}
