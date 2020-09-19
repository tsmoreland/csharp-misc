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
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Extensions
{
    [TestFixture]
    public sealed class DictionaryExtensionsTests
    {
        private readonly Dictionary<int, int> _nullDictionary = null!;
        private Dictionary<int, int> _first = null!;
        private Dictionary<int, int> _second = null!;
        private Dictionary<int, int> _mixOfFirstAndSecond = null!;

        [SetUp]
        public void Setup()
        {
            _first = From((1, 1), (2, 2), (3, 3));
            _second = From((4, 4), (5, 5), (6, 6));
            _mixOfFirstAndSecond = From((1, 1), (2, 2), (5, 5), (6, 6));
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.Union(_second, MergeByAddition));
            Assert.That(ex.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Union(_nullDictionary, MergeByAddition));
            Assert.That(ex.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenMergeHandlerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Union(_second, null!));
            Assert.That(ex.ParamName, Is.EqualTo("mergeHandler"));
        }

        [Test]
        public void UnionMergeUsingFirst_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.UnionMergeUsingFirst(_second));
            Assert.That(ex.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void UnionMergeUsingFirst_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.UnionMergeUsingFirst(_nullDictionary));
            Assert.That(ex.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void UnionMergeUsingSecond_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.UnionMergeUsingSecond(_second));
            Assert.That(ex.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void UnionMergeUsingSecond_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.UnionMergeUsingSecond(_nullDictionary));
            Assert.That(ex.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void Intersect_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _nullDictionary.Intersect(_second, MergeByAddition));
            Assert.That(ex.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void Intersect_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Intersect(_nullDictionary, MergeByAddition));
            Assert.That(ex.ParamName, Is.EqualTo("second"));
        }

        [Test]
        public void Intersect_ThrowsArgumentNullException_WhenMergeHandlerIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _first.Intersect(_second, null!));
            Assert.That(ex.ParamName, Is.EqualTo("mergeHandler"));
        }



        private static Dictionary<int, int> From(params (int key, int value)[] items)
        {
            var dictionary = new Dictionary<int, int>();
            foreach (var (key, value) in items)
                dictionary[key] = value;
            return dictionary;
        }

        private static int MergeByAddition(int first, int second) => first + second;
    }
}
