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

// ReSharper disable once CheckNamespace
namespace Moreland.CSharp.Extensions
{
    /// <summary>
    /// <see cref="IList{T}"/> extension methods
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns new list containing <paramref name="items"/>
        /// </summary>
        public static List<T> Of<T>(params T[] items) => new List<T>(items);
        /// <summary>
        /// Returns new list containing <paramref name="items"/>
        /// </summary>
        public static List<T> Of<T>(IEnumerable<T> items) => new List<T>(items);
        /// <summary>
        /// Returns new empty list
        /// </summary>
        public static List<T> Empty<T>() => new List<T>();
    }
}
