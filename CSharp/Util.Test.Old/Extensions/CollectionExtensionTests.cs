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
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Moreland.CSharp.Util.Test.Old.Extensions
{
    public class CollectionExtensionTests
    {
        [Fact]
        public void AddRangeParamsAddsExpectedValues()
        {
            Collection<int> collection = new Collection<int>();

            collection.AddRange(1, 2, 3);

            Assert.Equal(new[] { 1, 2, 3 }, collection);
        }

        [Fact]
        public void AddRangeEnumerableAddsExpectedValues()
        {
            Collection<int> collection = new Collection<int>();
            var items = new List<int> { 1, 2, 3 };

            collection.AddRange(items);

            Assert.Equal(new[] { 1, 2, 3 }, collection);
        }
        [Fact]
        public void AddRangeThrowsArgumentNullExceptionWhenCollectionNull()
        {
            Collection<int> collection = null!;
            var items = new List<int> { 1, 2, 3 };

            var exception = Assert.Throws<ArgumentNullException>(() => collection.AddRange(items));
            Assert.Contains($"'{nameof(collection)}'", exception.Message);
        }

        [Fact]
        public void AddRangeThrowsArgumentNullExceptionWhenItemsNull()
        {
            Collection<int> collection = new Collection<int>();
            List<int> items = null!;

            var exception = Assert.Throws<ArgumentNullException>(() => collection.AddRange(items));
            Assert.Contains($"'{nameof(items)}'", exception.Message);
        }
    }
}
