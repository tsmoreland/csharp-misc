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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moreland.CSharp.Util.Collections;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Collections
{
    [TestFixture]
    public sealed class FluentListTests
    {

        [TestCase]
        [TestCase(1)]
        [TestCase(1, 2)]
        public void Of_Params_ReturnsList_Always(params int[] items)
        {
            var list = FluentList.Of(items);

            Assert.That(list, Is.EquivalentTo(items));
        }

        [TestCase]
        [TestCase(1)]
        [TestCase(1, 2)]
        public void Of_IEnumerable_ReturnsList_Always(params int[] items)
        {
            IEnumerable<int> enumerable = new Collection<int>(items);
            var list = FluentList.Of(enumerable);

            Assert.That(list, Is.EquivalentTo(items));
        }

        [Test]
        public void Empty_ReturnsEmptyList_Always()
        {
            var empty = FluentList.Empty<int>();
            Assert.That(empty, Is.Empty);
        }
    }
}
