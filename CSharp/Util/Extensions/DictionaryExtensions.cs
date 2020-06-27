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

namespace System.Linq
{
    public static class DictionaryExtensions
    {
        public delegate TValue MergeConfict<TValue>(TValue first, TValue second);

        /// <summary>
        /// Performs a union of two dictionarys using <paramref name="mergeHandler"/> to handle duplicate entries
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="first">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements form the 
        /// first set for the union
        /// </param>
        /// <param name="second">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements form the 
        /// second set for the union
        /// </param>
        /// <param name="mergeHandler">
        /// <see cref="MergeUsingFirst{TValue}(TValue, TValue)"/> used to merge conflicting entries 
        /// between <paramref name="first"/> and <paramref name="second"/>
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> that contains the elements from both
        /// input sequences, where duplicates are merged using <paramref name="mergeHandler"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="mergeHandler"/> are <c>null</c></exception>
        public static IDictionary<TKey, TValue> Union<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second, MergeConfict<TValue> mergeHandler)
            where TKey : notnull
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (mergeHandler == null)
                throw new ArgumentNullException(nameof(mergeHandler));

            return first.Keys.Union(second.Keys)
                .ToDictionary(key => key, key =>
                {
                    bool firstHasKey = first.ContainsKey(key);
                    bool secondHasKey = second.ContainsKey(key);
                    if (firstHasKey && secondHasKey)
                        return mergeHandler(first[key], second[key]);
                    else if (firstHasKey)
                        return first[key];
                    else
                        return second[key];
                });
        }

        /// <summary>
        /// Performs a union of two dictionarys preferring items from <paramref name="first"/> in the case of conflict
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="first">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements form the 
        /// first set for the union
        /// </param>
        /// <param name="second">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements form the 
        /// second set for the union
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> that contains the elements from both
        /// input sequences, where duplicates are merged by using value from <paramref name="first"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="conflictHandler"/> are <c>null</c></exception>
        public static IDictionary<TKey, TValue> UnionMergeUsingFirst<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
            where TKey : notnull =>
            Union(first, second, (firstValue, secondValue) => firstValue);

        /// <summary>
        /// Performs a union of two dictionarys preferring items from <paramref name="second"/> in the case of conflict
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="first">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements form the 
        /// first set for the union
        /// </param>
        /// <param name="second">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements form the 
        /// second set for the union
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> that contains the elements from both
        /// input sequences, where duplicates are merged by using value from <paramref name="second"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="conflictHandler"/> are <c>null</c></exception>
        public static IDictionary<TKey, TValue> UnionMergeUsingSecond<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
            where TKey : notnull =>
            Union(first, second, (firstValue, secondValue) => secondValue);

        /// <summary>
        /// Produces the set intersection of two sequences by using the default equality
        /// comparer to compare keys.  Values are then merged using <paramref name="mergeHandler"/>
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="first">
        /// An <see cref="IDictionary{TKey, TValue}"/> whose distinct elements that also
        /// </param>
        /// <param name="second">
        /// </param>
        /// <param name="mergeHandler">
        /// <see cref="MergeUsingFirst{TValue}(TValue, TValue)"/> used to merge conflicting entries 
        /// between <paramref name="first"/> and <paramref name="second"/>
        /// </param>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> that contains the elements that form the set intersection of two sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="conflictHandler"/> are <c>null</c></exception>
        public static IDictionary<TKey, TValue> Intersect<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second, MergeConfict<TValue> mergeHandler)
            where TKey : notnull
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (mergeHandler == null)
                throw new ArgumentNullException(nameof(mergeHandler));

            return first.Keys.Intersect(second.Keys)
                .ToDictionary(key => key, key => mergeHandler(first[key], second[key]));
        }
    }
}
