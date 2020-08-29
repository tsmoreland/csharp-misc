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
using NSubstitute;
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class MaybeValueTypeTests : MaybeTests<Guid>
    {
        public MaybeValueTypeTests()
            : base(() => BuildRandomGuid())
        {
        }
    }

    [TestFixture]
    public sealed class MaybeEquatableRefTypeTests : MaybeTests<string>
    {
        public MaybeEquatableRefTypeTests()
            : base(() => BuildRandomString())
        {
        }
    }

    [TestFixture]
    public sealed class MaybeRefTypeTests : MaybeTests<List<string>>
    {
        public MaybeRefTypeTests()
            : base(() => BuildRandomListOfString())
        {
        }
    }

    public abstract class MaybeTests<T>
    {
        private readonly Func<T> _builder;
        private Maybe<T> _maybeWithValue;
        private Func<T, object> _selector = null!;
        private Func<T, Maybe<object>> _maybeSelector = null!;

        protected MaybeTests(Func<T> builder)
        {
            _builder = builder;
        }

        [SetUp]
        public void Setup()
        {
            _maybeWithValue = Maybe.Of(_builder());
            _selector = Substitute.For<Func<T, object>>();
            _maybeSelector = Substitute.For<Func<T, Maybe<object>>>();
        }


        [Test]
        public void Constructor_ReturnsEmpty_WhenInvoked()
        {
            var maybe = new Maybe<T>();

            Assert.That(maybe, Is.EqualTo(Maybe.Empty<T>()));
        }

        [Test]
        public void HasValue_ReturnsTrue_BuiltUsingOfWithNonNull()
        {
            bool hasValue = _maybeWithValue.HasValue;

            Assert.That(hasValue, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Where_ReturnsEmpty_WhenDoesNotHaveValue(bool filterResult)
        {
            var predicate = Substitute.For<Predicate<T>>();
            predicate.Invoke(Arg.Any<T>()).Returns(filterResult);

            var maybe = Maybe.Empty<T>();

            var actual = maybe.Where(predicate);
            
            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void Where_ReturnsEmpty_WhenHasValueButPredicateReturnsFalse()
        {
            var predicate = Substitute.For<Predicate<T>>();
            predicate.Invoke(Arg.Any<T>()).Returns(false);

            var actual = _maybeWithValue.Where(predicate);

            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void Where_ReturnsWithValue_WhenHasValueAndPredicateReturnsTrue()
        {
            var predicate = Substitute.For<Predicate<T>>();
            predicate.Invoke(Arg.Any<T>()).Returns(true);

            var actual = _maybeWithValue.Where(predicate);

            Assert.That(actual.Value, Is.EqualTo(_maybeWithValue.Value));
        }

        [Test]
        public void Select_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _maybeWithValue.Select((Func<T, object>)null!));
            Assert.That(ex.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void Select_ReturnsUpdatedMaybe_WhenHasValueAndSelectorIsNonNull()
        {
            var expected = new object();
            _selector.Invoke(Arg.Any<T>()).Returns(expected);

            var actual = _maybeWithValue.Select(_selector);

            Assert.That(actual.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Select_ReturnsEmpty_WhenDoesNotHasValue()
        {
            _selector.Invoke(Arg.Any<T>()).Returns(new object());

            var actual = Maybe.Empty<T>().Select(_selector);

            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void SelectMaybe_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _maybeWithValue.Select((Func<T, Maybe<object>>)null!));
            Assert.That(ex.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void SelectMaybe_ReturnsUpdatedMaybe_WhenHasValueAndSelectorIsNonNull()
        {
            var expected = Maybe.Of(new object());
            _maybeSelector.Invoke(Arg.Any<T>()).Returns(expected);

            var actual = _maybeWithValue.Select(_maybeSelector);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SelectMaybe_ReturnsEmpty_WhenDoesNotHasValue()
        {
            _maybeSelector.Invoke(Arg.Any<T>()).Returns(Maybe.Of(new object()));

            var actual = Maybe.Empty<T>().Select(_maybeSelector);

            Assert.That(actual.IsEmpty, Is.True);
        }
    }
}
