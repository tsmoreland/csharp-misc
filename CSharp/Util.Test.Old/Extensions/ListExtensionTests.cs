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
using System.Linq;
using Xunit;

namespace Moreland.CSharp.Util.Test.Old.Extensions
{
    public class ListExtensionTests
    {
        [Fact]
        public void ListOfArrayContainsProvidedItems()
        {
            var list = List.Of(1, 2, 3);
            Assert.Equal(new List<int>() { 1, 2, 3 }, list);
        }

        [Fact]
        public void ListOfEnumerableContainsProvidedItems()
        {
            var enumerable = (new List<int>() { 1, 2, 3 }).AsEnumerable();
            var list = List.Of(enumerable);
            Assert.Equal(new List<int>() { 1, 2, 3 }, list);
        }

        [Fact]
        public void EmptyHasNoValues()
        {
            var empty = List.Empty<int>();

            Assert.Equal(0, empty.Count);
        }
    }
}
