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
using CSharp.Util;
using Xunit;

namespace CSharp.Test.Util
{
    public sealed class NumericParserTest
    {
        [Fact]
        public void MaybeParseShort_ParsesValidShort() =>
            Context.MaybeParse_ParsesValidNumber(input => NumericParser.MaybeParseShort(input), "42", (short)42);
        [Fact]
        public void MaybeParseShort_DoesNotParseInvalidShort() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseShort(input), "FourtyTwo");
        [Fact]
        public void MaybeParseShort_ThrowsExceptionForNullString() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseShort(input), null!);

        [Fact]
        public void MaybeParseInt_ParsesValidInt() =>
            Context.MaybeParse_ParsesValidNumber(input => NumericParser.MaybeParseInt(input), "42", 42);
        [Fact]
        public void MaybeParseInt_DoesNotParseInvalidInt() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseInt(input), "FourtyTwo");
        [Fact]
        public void MaybeParseInt_ThrowsExceptionForNullString() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseInt(input), null!);

        [Fact]
        public void MaybeParseLong_ParsesValidLong() =>
            Context.MaybeParse_ParsesValidNumber(input => NumericParser.MaybeParseLong(input), "42", 42L);
        [Fact]
        public void MaybeParseLong_DoesNotParseInvalidLong() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseLong(input), "FourtyTwo");
        [Fact]
        public void MaybeParseLong_ThrowsExceptionForNullString() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseLong(input), null!);

        [Fact]
        public void MaybeParseDouble_ParsesValidDouble() =>
            Context.MaybeParse_ParsesValidNumber(input => NumericParser.MaybeParseDouble(input), "42.42", 42.42);
        [Fact]
        public void MaybeParseDouble_DoesNotParseInvalidDouble() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseDouble(input), "FourtyTwo");
        [Fact]
        public void MaybeParseDouble_ThrowsExceptionForNullString() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseDouble(input), null!);

        [Fact]
        public void MaybeParseFloat_ParsesValidFloat() =>
            Context.MaybeParse_ParsesValidNumber(input => NumericParser.MaybeParseFloat(input), "42.42", 42.42f);
        [Fact]
        public void MaybeParseFloat_DoesNotParseInvalidFloateger() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseFloat(input), "FourtyTwo");
        [Fact]
        public void MaybeParseFloat_ThrowsExceptionForNullString() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseFloat(input), null!);

        [Fact]
        public void MaybeParseDecimal_ParsesValidDecimal()
        {
            Context.MaybeParse_ParsesValidNumber(input => NumericParser.MaybeParseDecimal(input), "42.42", 42.42M);
            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void MaybeParseDecimal_DoesNotParseInvalidDecimaleger() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseDecimal(input), "FourtyTwo");
        [Fact]
        public void MaybeParseDecimal_ThrowsExceptionForNullString() =>
            Context.MaybeParse_DoesNotParseInvalidNumber(input => NumericParser.MaybeParseDecimal(input), null!);

        private sealed class Context
        {
            private Context()
            {

            }
            public static Context<T> Arrange<T>(string input, Maybe<T> expectedValue) =>
                new Context<T>(input, expectedValue);

            public static void MaybeParse_ParsesValidNumber<T>(Func<string, Maybe<T>> producer, string input, T expectedValue)
            {
                Context
                    .Arrange<T>(input, Maybe.Of(expectedValue))
                    .Act(producer)
                    .AssertResult();
            }
            public static void MaybeParse_DoesNotParseInvalidNumber<T>(Func<string, Maybe<T>> producer, string input)
            {
                Context
                    .Arrange<T>(input, Maybe.Empty<T>())
                    .Act(producer)
                    .AssertResult();
            }
        }
        private sealed class Context<T>
        {
            public Context(string input, Maybe<T> expectedValue)
            {
                _input = input;
                _actualValue = null!;
                _expectedValue = expectedValue;
            }

            public Context<T> Act(Func<string, Maybe<T>> act)
            {
                _actualValue = act(_input);
                return this;
            }
            public void AssertResult()
            {
                Assert.False(_actualValue == null!, "actual result not set, ensure Act is called before AssertResult"); 
                Assert.Equal(_expectedValue, _actualValue);
            }

            private readonly string _input;
            private Maybe<T> _actualValue;
            private readonly Maybe<T> _expectedValue;

        }
    }
}
