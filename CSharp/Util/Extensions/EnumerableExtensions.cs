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
using System.Util;
using System.Util.Extensions;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        /// <summary>Returns the first item in the collection wrapped in a <see cref="Maybe{TValue}"/> or <see cref="Maybe.Empty{TValue}"/> if <paramref name="enumerable"/> is empty. </summary>
        /// <exception cref="NullReferenceException">if <paramref name="enumerable"/> is <c>null</c></exception>
        public static Maybe<TValue> MaybeFirst<TValue>(this IEnumerable<TValue> enumerable) => enumerable
            .Select(item => Maybe.Of(item))
            .DefaultIfEmpty(Maybe.Empty<TValue>())
            .FirstOrDefault();

        /// <summary>Returns <paramref name="enumerable"/> as array of <typeparamref name="TValue"/> without building a new array if it already is one and an empty array if <paramref name="enumerable"/> is <c>null</c></summary>
        public static TValue[] AsOrToArray<TValue>(this IEnumerable<TValue> enumerable) => enumerable is TValue[] array 
            ? array 
            : enumerable?.ToArray() ?? ArrayExtensions.Empty<TValue>();

        /// <summary>
        /// Returns <paramref name="enumerable"/> as <see cref="IList{TValue}"/> of <typeparamref name="TValue"/> without building a new list if it already is one 
        /// and an empty list if <paramref name="enumerable"/> is <c>null</c><
        /// /summary>
        public static IList<TValue> AsOrToList<TValue>(this IEnumerable<TValue> enumerable) => enumerable is IList<TValue> list
            ? list
            : enumerable?.ToList() ?? new List<TValue>();
    }
}
