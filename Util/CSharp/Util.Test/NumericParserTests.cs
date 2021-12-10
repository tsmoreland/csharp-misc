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
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test
{
    public abstract class NumericParserTests<T> where T : struct, IComparable
    {
        private readonly Func<T> _builder;
        private string _validValueString = string.Empty;
        private string _invalidValueString = string.Empty;
        private T _validValue = default;
        private readonly MaybeParseDelegate _maybeParser;

        protected delegate Maybe<T> MaybeParseDelegate(string? @string, NumberStyles style = NumberStyles.Any,
            IFormatProvider? provider = null);

        protected NumericParserTests(Func<T> builder, MaybeParseDelegate maybeParser)
        {
            _builder = builder;
            _maybeParser = maybeParser;
        }
        [SetUp]
        public void Setup()
        {
            _validValue = _builder();
            _validValueString = _validValue.ToString()!;
            _invalidValueString = $"str-{BuildRandomString()}";
        }

        [Test]
        public void MaybeParse_ReturnsMaybeWithValue_OnSuccess()
        {
            var actual = _maybeParser(_validValueString);

            Assert.That(actual.HasValue, Is.True);
        }

        [Test]
        public virtual void MaybeParse_ReturnsCorrectValue_OnSuccess()
        {
            var actual = _maybeParser(_validValueString);

            // using virtual method to allow float/double to handle differently
            Assert.That(actual.Value, Is.EqualTo(_validValue));
        }

        [Test]
        public void MaybeParse_ReturnsEmpty_OnFailure()
        {
            var actual = _maybeParser(_invalidValueString);

            Assert.That(actual.IsEmpty, Is.True);
        }
    }

    [TestFixture]
    public class ShortNumericParserTests : NumericParserTests<short>
    {
        public ShortNumericParserTests()
            : base(() => BuildRandomInt16(), NumericParser.MaybeParseShort)
        {
        }
    }
    [TestFixture]
    public class Int32NumericParserTests : NumericParserTests<int>
    {
        public Int32NumericParserTests()
            : base(() => BuildRandomInt32(), NumericParser.MaybeParseInt)
        {
        }
    }
    [TestFixture]
    public class Int64NumericParserTests : NumericParserTests<long>
    {
        public Int64NumericParserTests()
            : base(() => BuildRandomInt64(), NumericParser.MaybeParseLong)
        {
        }
    }
    [TestFixture]
    public class FloatNumericParserTests : NumericParserTests<float>
    {
        public FloatNumericParserTests()
            : base(() => BuildRandomFloat(), NumericParser.MaybeParseFloat)
        {
        }

        // ignoring as float's can't easily be compared
        public override void MaybeParse_ReturnsCorrectValue_OnSuccess()
        {
        }
    }
    [TestFixture]
    public class DoubleNumericParserTests : NumericParserTests<double>
    {
        public DoubleNumericParserTests()
            : base(() => BuildRandomDouble(), NumericParser.MaybeParseDouble)
        {
        }


        // ignoring as double's can't easily be compared
        public override void MaybeParse_ReturnsCorrectValue_OnSuccess()
        {
        }
    }
    [TestFixture]
    public class DecimalNumericParserTests : NumericParserTests<decimal>
    {
        public DecimalNumericParserTests()
            : base(() => BuildRandomDecimal(), NumericParser.MaybeParseDecimal)
        {
        }
    }

}
