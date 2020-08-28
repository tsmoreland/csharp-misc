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
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class MaybeTests
    {
        private MaybeTests<Guid> _valueTests = null!;
        private MaybeTests<string> _equatableRefTests = null!;
        private MaybeTests<List<string>> _refTests = null!;

        [SetUp]
        public void Setup()
        {
            _valueTests = new MaybeTests<Guid>(() => BuildRandomGuid());
            _equatableRefTests = new MaybeTests<string>(() => BuildRandomString());
            _refTests = new MaybeTests<List<string>>(() => BuildRandomListOfString());
        }

        [Test]
        public void Constructor_ReturnsEmpty_WhenUsingValueType() =>
            _valueTests.Constructor_ReturnsEmpty_WhenInvoked();

        [Test]
        public void Constructor_ReturnsEmpty_WhenUsingEquatableRefType() =>
            _equatableRefTests.Constructor_ReturnsEmpty_WhenInvoked();

        [Test]
        public void Constructor_ReturnsEmpty_WhenUsingRefType() =>
            _refTests.Constructor_ReturnsEmpty_WhenInvoked();
    }

    public sealed class MaybeTests<T>
    {
        private readonly Func<T> _factory;

        public MaybeTests(Func<T> factory)
        {
            _factory = factory;
        }

        public void Constructor_ReturnsEmpty_WhenInvoked()
        {
            var maybe = new Maybe<T>();

            Assert.That(maybe, Is.EqualTo(Maybe.Empty<T>()));
        }
    }
}
