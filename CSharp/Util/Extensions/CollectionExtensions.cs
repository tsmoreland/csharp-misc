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
namespace System.Linq
{
    /// <summary>
    /// Extension methods to generic Collection types
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds <paramref name="items"/> to the collection <paramref name="collection"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="collection"/> is <c>null</c></exception>
        public static void AddRange<T>(this ICollection<T> collection, params T[] items) =>
            AddRange(collection, items.AsEnumerable());

        /// <summary>
        /// Adds <paramref name="items"/> to the collection <paramref name="collection"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="collection"/> or <paramref name="items"/> is <c>null</c></exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                collection.Add(item);
        }
    }
}
