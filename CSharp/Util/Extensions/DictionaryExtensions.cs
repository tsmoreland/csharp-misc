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

namespace Moreland.CSharp.Util.Extensions
{
    public static class DictionaryExtensions
    {
        public delegate TValue MergeConfict<TValue>(TValue first, TValue second);

        /// <summary>
        /// Performs a union of two dictionarys using <paramref name="conflictHandler"/> to handle duplicate entries
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
        /// <param name="conflictHandler">
        /// <see cref="MergeUsingFirst{TValue}(TValue, TValue)"/> used to merge conflicting entries 
        /// between <paramref name="first"/> and <paramref name="second"/>
        /// </param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> that contains the elements from both
        /// input sequences, where duplicates are merged using <paramref name="conflictHandler"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/>, <paramref name="second"/>, or <paramref name="conflictHandler"/> are <c>null</c></exception>
        public static IDictionary<TKey, TValue> Union<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second, MergeConfict<TValue> conflictHandler)
            where TKey : notnull
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (conflictHandler == null)
                throw new ArgumentNullException(nameof(conflictHandler));

            var superset = new Dictionary<TKey, TValue>(first);

#           if NETSTANDARD2_0 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48
            foreach (var keyValuePair in second)
            {
                var key = keyValuePair.Key;
                var value = keyValuePair.Value;
                if (!superset.ContainsKey(key))
                    superset[key] = value;
                else
                    superset[key] = conflictHandler(superset[key], value);
            }
#           else
            foreach (var (key, value) in second)
                if (!superset.ContainsKey(key))
                    superset[key] = value;
                else
                    superset[key] = conflictHandler(superset[key], value);
#           endif

            return superset;
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
    }
}
