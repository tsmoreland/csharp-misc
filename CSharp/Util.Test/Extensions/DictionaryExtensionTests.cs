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


using Moreland.CSharp.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Moreland.CSharp.Util.Test.Extensions
{
    public class DictionaryExtensionTests
    {
        [Fact]
        public void Union_ThrowsArgumentNullExceptionForNullFirstValue()
        {
            Dictionary<int, string> first = null!;
            Dictionary<int, string> second = ArrayExtensions.Of(1, 2, 3).ToDictionary(value => value, value => value.ToString());

            var exception = Assert.Throws<ArgumentNullException>(() => first.Union(second, UnexpectedMerge));
            Assert.Contains($"'{nameof(first)}'", exception.Message);
        }
        [Fact]
        public void Union_ThrowsArgumentNullExceptionForNullSecondValue()
        {
            Dictionary<int, string> first = ArrayExtensions.Of(1, 2, 3).ToDictionary(value => value, value => value.ToString());
            Dictionary<int, string> second = null!;

            var exception = Assert.Throws<ArgumentNullException>(() => first.Union(second, UnexpectedMerge));
            Assert.Contains($"'{nameof(second)}'", exception.Message);
        }
        [Fact]
        public void Union_ThrowsArgumentNullExceptionForNullMergeHandler()
        {
            Dictionary<int, string> first = ArrayExtensions.Of(3, 4, 5).ToDictionary(value => value, value => value.ToString());
            Dictionary<int, string> second = ArrayExtensions.Of(1, 2, 3).ToDictionary(value => value, value => value.ToString());

            var exception = Assert.Throws<ArgumentNullException>(() => first.Union(second, null!));
            Assert.Contains("'mergeHandler'", exception.Message);
        }

        private static string UnexpectedMerge(string first, string second)
        {
            Assert.True(false, "unexpected call to merge");
            return string.Empty;
        }
    }
}
