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
using Moreland.CSharp.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Extensions
{
    [TestFixture]
    public sealed class DictionaryExtensionsTests
    {
        private readonly Dictionary<int, int> _nullDictionary = null!;
        private Dictionary<int, int> _first = null!;
        private Dictionary<int, int> _second = null!;

        [SetUp]
        public void Setup()
        {
#if !NET461
            _first = From((1, 1), (2, 2), (3, 3));
            _second = From((4, 4), (5, 5), (6, 6));
#else
            _first = From(new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(2, 2), new KeyValuePair<int, int>(3, 3));
            _second = From(new KeyValuePair<int, int>(4, 4), new KeyValuePair<int, int>(5, 5), new KeyValuePair<int, int>(6, 6));
#endif
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.Union(_second, MergeByAddition));
            Assert.That(ex!.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Union(_nullDictionary, MergeByAddition));
            Assert.That(ex!.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenMergeHandlerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Union(_second, null!));
            Assert.That(ex!.ParamName, Is.EqualTo("mergeHandler"));
        }

        [Test]
        public void UnionMergeUsingFirst_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.UnionMergeUsingFirst(_second));
            Assert.That(ex!.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void UnionMergeUsingFirst_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.UnionMergeUsingFirst(_nullDictionary));
            Assert.That(ex!.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void UnionMergeUsingSecond_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.UnionMergeUsingSecond(_second));
            Assert.That(ex!.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void UnionMergeUsingSecond_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.UnionMergeUsingSecond(_nullDictionary));
            Assert.That(ex!.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void Intersect_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.Intersect(_second, MergeByAddition));
            Assert.That(ex!.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void Intersect_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Intersect(_nullDictionary, MergeByAddition));
            Assert.That(ex!.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void Intersect_ThrowsArgumentNullException_WhenMergeHandlerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Intersect(_second, null!));
            Assert.That(ex!.ParamName, Is.EqualTo("mergeHandler"));
        }

        [Test]
        public void Union_MergesUsingHandler_WhenDuplicateKeysPresent()
        {
            Func<int, int, int> mergeHandler = Substitute.For<Func<int, int, int>>();
            MergeHandler<int> handler = new MergeHandler<int>(mergeHandler);
            var other = _first.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            other.Remove(other.Keys.First());
            other[15] = 15;
 
            _ = _first.Union(other, handler);

            mergeHandler.Received(_first.Count - 1).Invoke(Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void UnionMergeUsesFirst_SelectsValueFromFirst_WhenDuplicateKeysFound()
        {
            var firstDoubled = _first.ToDictionary(kvp => kvp.Key, kvp => kvp.Value * 2);

            var merged = firstDoubled.UnionMergeUsingFirst(_first);

            Assert.That(merged, Is.EquivalentTo(firstDoubled));
        }

        [Test]
        public void UnionMergeUsesSecond_SelectsValueFromSecond_WhenDuplicateKeysFound()
        {
            var firstDoubled = _first.ToDictionary(kvp => kvp.Key, kvp => kvp.Value * 2);

            var merged = firstDoubled.UnionMergeUsingSecond(_first);

            Assert.That(merged, Is.EquivalentTo(_first));
        }

        [Test]
        public void Intersect_MergesUsingHandler_WhenDuplicateKeysPresent()
        {
            Func<int, int, int> mergeHandler = Substitute.For<Func<int, int, int>>();
            MergeHandler<int> handler = new MergeHandler<int>(mergeHandler);

            _ = _first.Intersect(_first, handler);

            mergeHandler.Received(_first.Count).Invoke(Arg.Any<int>(), Arg.Any<int>());
        }

#if !NET461
        private static Dictionary<int, int> From(params (int key, int value)[] items)
        {
            var dictionary = new Dictionary<int, int>();
            foreach (var (key, value) in items)
            {
                dictionary[key] = value;
            }
            return dictionary;
        }
#else
        private static Dictionary<int, int> From(params KeyValuePair<int, int>[] items)
        {
            var dictionary = new Dictionary<int, int>();
            foreach (var pair in items)
            {
                dictionary[pair.Key] = pair.Value;
            }
            return dictionary;
        }
#endif

        private static int MergeByAddition(int first, int second) => first + second;
    }
}
