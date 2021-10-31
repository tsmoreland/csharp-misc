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
using System.Collections.Generic;
using Moreland.CSharp.Util.Collections;
using Moreland.CSharp.Util.Test.TestData;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Collections
{
    [TestFixture]
    public sealed class ValueTypeFluentArrayTests : FluentArrayTests<int>
    {
        public ValueTypeFluentArrayTests()
            : base(() => RandomValueFactory.BuildRandomInt32())
        {
            
        }
    }

    [TestFixture]
    public sealed class ReferenceTypeFluentArrayTests : FluentArrayTests<List<string>>
    {
        public ReferenceTypeFluentArrayTests()
            : base(() => RandomValueFactory.BuildRandomListOfString())
        {
            
        }
    }

    public abstract class FluentArrayTests<T>
    {
        private readonly Func<T> _builder;

        protected FluentArrayTests(Func<T> builder)
        {
            _builder = builder;
        }

        [Test]
        public void Empty_ReturnsZeroLengthArray_Always()
        {
            var actual = FluentArray.Empty<T>();
            Assert.That(actual.Length, Is.EqualTo(0));
        }

        [Test]
        public void Of_ReturnsArrayContainingProvidedValues_Always()
        {
            var value1 = _builder();
            var value2 = _builder();
            var value3 = _builder();

            var actual = FluentArray.Of(value1, value2, value3);

            Assert.That(actual, Is.EquivalentTo(new[] {value1, value2, value3}));
        }
    }

}
