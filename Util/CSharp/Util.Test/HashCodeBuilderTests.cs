﻿//
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


using System.Collections.Generic;
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class HashCodeBuilderTests
    {
        private object?[] _values = null!;

        [SetUp]
        public void Setup()
        {
            var values = new List<object?> 
            { 
                BuildRandomInt32(),
                BuildRandomBoolean(),
                BuildRandomDecimal(),
                null,
                BuildRandomString(),
                BuildRandomListOfString(),
                BuildRandomGuid()
            };
            _values = values.ToArray();
        }

        [Test]
        public void ToHashCode_ReturnsNonZero_WhenNotEmpty()
        {
            var builder = HashCodeBuilder.Create(_values);

            var hashCode = builder.ToHashCode();

            Assert.That(hashCode, Is.Not.EqualTo(0));
        }

        [Test]
        public void Equals_ReturnsTrue_WhenBuiltSameValuesUsingCreate()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(1, 2, 3);

            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void Equals_ReturnsTrue_WhenBuiltSameValuesUsingCreateOrAdd()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            first = first.WithAddedValues(4, 5, 6);
            var second = HashCodeBuilder.Create(1, 2, 3, 4, 5, 6);

            Assert.That(first, Is.EqualTo(second));
        }

        [TestCase(10, 20, ExpectedResult = false)]
        [TestCase(10, 10, ExpectedResult = true)]
        [TestCase(20, 10, ExpectedResult = false)]
        [TestCase(20, null, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsExpectedResult_WhenUsingProvidedValues(int firstValue, int? secondValue)
        {
            var first = HashCodeBuilder.Create(firstValue);
            object? second = secondValue != null ? HashCodeBuilder.Create(secondValue) : null;

            return first.Equals(second);
        }

        [TestCase(10, 20, ExpectedResult = false)]
        [TestCase(10, 10, ExpectedResult = true)]
        [TestCase(20, 10, ExpectedResult = false)]
        [TestCase(20, null, ExpectedResult = false)]
        public bool IEquatableEquals_ReturnsExpectedResult_WhenUsingProvidedValues(int firstValue, int? secondValue)
        {
            var first = HashCodeBuilder.Create(firstValue);
            var second = secondValue != null ? HashCodeBuilder.Create(secondValue) : null;

            return first.Equals(second);
        }

        [TestCase(10, 20, ExpectedResult = false)]
        [TestCase(10, 10, ExpectedResult = true)]
        [TestCase(20, 10, ExpectedResult = false)]
        [TestCase(null, 10, ExpectedResult = false)]
        [TestCase(20, null, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsExpectedResult_WhenComparingProvidedValue(int? firstValue, int? secondValue)
        {
            var first = firstValue != null ? HashCodeBuilder.Create(firstValue) : null;
            var second = secondValue != null ? HashCodeBuilder.Create(secondValue) : null;

            return first == second;
        }

        [TestCase(10, 20, ExpectedResult = true)]
        [TestCase(10, 10, ExpectedResult = false)]
        [TestCase(20, 10, ExpectedResult = true)]
        [TestCase(null, 10, ExpectedResult = true)]
        [TestCase(20, null, ExpectedResult = true)]
        public bool OperatorNotEqual_ReturnsExpectedResult_WhenComparingProvidedValue(int? firstValue, int? secondValue)
        {
            var first = firstValue != null ? HashCodeBuilder.Create(firstValue) : null;
            var second = secondValue != null ? HashCodeBuilder.Create(secondValue) : null;

            return first != second;
        }

        [Test]
        public void GetHashCode_ReturnsToHashCode_Always()
        {
            var builder = HashCodeBuilder.Create(_values);

            int toHashCode = builder.ToHashCode();
            int getHashCode = builder.GetHashCode();

            Assert.That(getHashCode, Is.EqualTo(toHashCode));
        }
    }
}
