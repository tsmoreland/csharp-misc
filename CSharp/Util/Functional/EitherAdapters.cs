﻿//
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
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Functional
{
    /// <remarks>
    /// based on examples from https://app.pluralsight.com/library/courses/making-functional-csharp
    /// the idea at least, the Either class more so than the adapters these are more based on the
    /// features of Maybe{T}
    /// </remarks>
    public static class EitherAdapters
    {
        /// <summary>
        /// Projects source into a new form
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TNewRight">Secondary type of the transformed <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="source">source to invoke transform function on</param>
        /// <param name="selector">transform function to apply</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> resulting from invoking the transform function </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selector"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <see cref="Either{TLeft,TRight}.IsEmpty"/> for <paramref name="source"/> returns true
        /// </exception>
        public static Either<TLeft, TNewRight> Select<TLeft, TRight, TNewRight>(this Either<TLeft, TRight> source,
            Func<TRight, TNewRight> selector)
        {
            GuardAgainst.ArgumentIsEmpty(source);
            GuardAgainst.ArgumentBeingNull(selector, nameof(selector));
            
            return source.HasLeftValue
                ? Either.From<TLeft, TNewRight>(source.LeftValue)
                : selector(source.RightValue);
        }

        /// <summary>
        /// Projects source into a new form
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TNewRight">Secondary type of the transformed <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="source">source to invoke transform function on</param>
        /// <param name="selector">transform function to apply</param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> resulting from invoking the transform function </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="selector"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <see cref="Either{TLeft,TRight}.IsEmpty"/> for <paramref name="source"/> returns true
        /// </exception>
        public static Either<TLeft, TNewRight> Select<TLeft, TRight, TNewRight>(this Either<TLeft, TRight> source,
            Func<TRight, Either<TLeft, TNewRight>> selector)
        {
            GuardAgainst.ArgumentIsEmpty(source);
            GuardAgainst.ArgumentBeingNull(selector, nameof(selector));

            return source.HasLeftValue
                ? Either.From<TLeft, TNewRight>(source.LeftValue)
                : selector(source.RightValue);
        }
        /*

        /// <summary>
        /// Reducers <paramref name="source"/> to <typeparamref name="TLeft"/> either
        /// directly or using <paramref name="reducer"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="source">source to reduce</param>
        /// <param name="reducer">reduce function to convert <typeparamref name="TRight"/> to <typeparamref name="TLeft"/></param>
        /// <returns><typeparamref name="TLeft"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="reducer"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <paramref name="source"/> is not a <see cref="LeftEither{TLeft,TRight}"/> or <see cref="RightEither{TLeft,TRight}"/>
        /// </exception>
        public static TLeft Reduce<TLeft, TRight>(this Either<TLeft, TRight> source, Func<TRight, TLeft> reducer)
        {
            GuardAgainst.ArgumentBeingNull(source, nameof(source));
            GuardAgainst.ArgumentBeingNull(reducer, nameof(reducer));

            return source switch
            {
                LeftEither<TLeft, TRight> left => left.Value,
                RightEither<TLeft, TRight> right => reducer(right.Value),
                _ => throw new ArgumentException(ProjectResources.UnknownEitherAccess, nameof(source))
            };
        }

        /// <summary>
        /// Reducers <paramref name="source"/> to <typeparamref name="TLeft"/> either
        /// directly or using <paramref name="reducer"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="source">source to reduce</param>
        /// <param name="reducer">reduce function to convert <typeparamref name="TLeft"/> to <typeparamref name="TRight"/></param>
        /// <returns><typeparamref name="TRight"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> or <paramref name="reducer"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <paramref name="source"/> is not a <see cref="LeftEither{TLeft,TRight}"/> or <see cref="RightEither{TLeft,TRight}"/>
        /// </exception>
        public static TRight Reduce<TLeft, TRight>(this Either<TLeft, TRight> source, Func<TLeft, TRight> reducer)
        {
            GuardAgainst.ArgumentBeingNull(source, nameof(source));
            GuardAgainst.ArgumentBeingNull(reducer, nameof(reducer));

            return source switch
            {
                LeftEither<TLeft, TRight> left => reducer(left.Value),
                RightEither<TLeft, TRight> right => right.Value,
                _ => throw new ArgumentException(ProjectResources.UnknownEitherAccess, nameof(source))
            };
        }

        /// <summary>
        /// Transform <see cref="Either{TLeft,TRight}"/> to <typeparamref name="TResult"/> 
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TResult">transform result type</typeparam>
        /// <param name="source">source to transform</param>
        /// <param name="fromLeft">transform function if <paramref name="source"/> contains <typeparamref name="TLeft"/></param>
        /// <param name="fromRight">transform function if <paramref name="source"/> contains <typeparamref name="TRight"/></param>
        /// <returns><typeparamref name="TResult"/> resulting from <paramref name="fromLeft"/> or <paramref name="fromRight"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>, <paramref name="fromLeft"/> or <paramref name="fromRight"/> are <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <paramref name="source"/> is not a <see cref="LeftEither{TLeft,TRight}"/> or <see cref="RightEither{TLeft,TRight}"/>
        /// </exception>
        public static TResult Case<TLeft, TRight, TResult>(this Either<TLeft, TRight> source, Func<TLeft, TResult> fromLeft,
            Func<TRight, TResult> fromRight)
        {
            GuardAgainst.ArgumentBeingNull(source, nameof(source));
            GuardAgainst.ArgumentBeingNull(fromLeft, nameof(fromLeft));
            GuardAgainst.ArgumentBeingNull(fromRight, nameof(fromRight));

            return source switch
            {
                LeftEither<TLeft, TRight> left => fromLeft(left.Value),
                RightEither<TLeft, TRight> right => fromRight(right.Value),
                _ => throw new ArgumentException(ProjectResources.UnknownEitherAccess, nameof(source))
            };
        }
        */
    }
}
