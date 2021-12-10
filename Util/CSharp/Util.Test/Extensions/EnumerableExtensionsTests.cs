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
using System.Linq;
using Moreland.CSharp.Util.Collections;
using NSubstitute;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Extensions
{
    [TestFixture]
    public sealed class EnumerableExtensionsTests
    {
        [Test]
        public void MaybeFirstOrEmpty_ReturnsMaybeWithValue_WhenEnumerableNotEmpty()
        {
            IEnumerable<int> enumerable = FluentArray.Of(1, 2, 3);

            var actual = enumerable.MaybeFirstOrEmpty();

            Assert.That(actual.HasValue, Is.True);
        }

        [Test]
        public void MaybeFirstOrEmpty_ReturnsFirstValue_WhenEnumerableNotEmpty()
        {
            IEnumerable<int> enumerable = FluentArray.Of(1, 2, 3);

            var actual = enumerable.MaybeFirstOrEmpty();

            Assert.That(actual.Value, Is.EqualTo(1));
        }

        [Test]
        public void MaybeFirstOrEmpty_UsingPredicate_ReturnsMaybeWithValue_WhenEnumerableNotEmpty()
        {
            IEnumerable<int> enumerable = FluentArray.Of(1, 2, 3);
            static bool Predicate(int value) => value > 1;

            var actual = enumerable.MaybeFirstOrEmpty(Predicate);

            Assert.That(actual.HasValue, Is.True);
        }

        [Test]
        public void MaybeFirstOrEmpty_UsingPredicate_ReturnsFirstValueWhereConditionIsMet_WhenEnumerableNotEmpty()
        {
            IEnumerable<int> enumerable = FluentArray.Of(1, 2, 3);
            static bool Predicate(int value) => value > 1;

            var actual = enumerable.MaybeFirstOrEmpty(Predicate);

            Assert.That(actual.Value, Is.EqualTo(2));
        }

        [Test]
        public void AsOrToArray_ReturnsSource_WhenGivenArray()
        {
            var expected = FluentArray.Of(1, 2, 3);

            var actual = expected.AsOrToArray();

            Assert.That(ReferenceEquals(actual, expected), Is.True);
        }

        [Test]
        public void AsOrToArray_ReturnsArray_WhenNotGivenArray()
        {
            var list = FluentList.Of(1, 2, 3);

            var actual = list.AsOrToArray();

            Assert.That(actual, Is.TypeOf(typeof(int[])));
        }

        [Test]
        public void AsOrToList_ReturnsSource_WhenGivenList()
        {
            var expected = FluentList.Of(1, 2, 3);

            var actual = expected.AsOrToList();

            Assert.That(ReferenceEquals(actual, expected), Is.True);
        }

        [Test]
        public void AsOrToList_ReturnsList_WhenNotGivenList()
        {
            var list = FluentArray.Of(1, 2, 3);

            var actual = list.AsOrToList();

            Assert.That(actual, Is.TypeOf(typeof(List<int>)));
        }

        [Test]
        public void ForEach_ThrowsArgumentNullException_WhenItemsIsNull()
        {
            IEnumerable<int> items = null!;
            Action<int> consumer = Substitute.For<Action<int>>();

            var ex = Assert.Throws<ArgumentNullException>(() => items.ForEach(consumer));
            Assert.That(ex!.ParamName, Is.EqualTo("items"));
        }

        [Test]
        public void ForEach_ThrowsArgumentNullException_WhenConsumerIsNull()
        {
            IEnumerable<int> items = FluentArray.Of(1, 2, 3);
            Action<int> consumer = null!;

            var ex = Assert.Throws<ArgumentNullException>(() => items.ForEach(consumer));
            Assert.That(ex!.ParamName, Is.EqualTo("consumer"));
        }

        [Test]
        public void ForEach_CallsConsumerOnEachItem_WhenItemAndConsumerAreNonNull()
        {
            IEnumerable<int> items = FluentArray.Of(1, 2, 3);
            Action<int> consumer = Substitute.For<Action<int>>();

            items.ForEach(consumer);

            consumer.Received(items.Count()).Invoke(Arg.Any<int>());
        }
    }
}
