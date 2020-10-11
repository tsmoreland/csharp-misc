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
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TNewRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Either<TLeft, TNewRight> Select<TLeft, TRight, TNewRight>(this Either<TLeft, TRight> either,
            Func<TRight, TNewRight> selector)
        {
            GuardAgainst.ArgumentBeingNull(either, nameof(either));
            GuardAgainst.ArgumentBeingNull(selector, nameof(selector));

            return either switch
            {
                RightEither<TLeft, TRight> right => Either.From<TLeft, TNewRight>(selector(right)),
                LeftEither<TLeft, TRight> left => Either.From<TLeft, TNewRight>(left),
                _ => throw new ArgumentException(ProjectResources.UnknownEitherAccess, nameof(either))
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TNewRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Either<TLeft, TNewRight> Select<TLeft, TRight, TNewRight>(this Either<TRight, TRight> either,
            Func<TRight, Either<TLeft, TNewRight>> selector)
        {
            GuardAgainst.ArgumentBeingNull(either, nameof(either));
            GuardAgainst.ArgumentBeingNull(selector, nameof(selector));

            return either switch
            {
                RightEither<TLeft, TRight> right => selector(right),
                LeftEither<TLeft, TRight> left => Either.From<TLeft, TNewRight>(left),
                _ => throw new ArgumentException(ProjectResources.UnknownEitherAccess, nameof(either))
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="either"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static Either<TLeft, TRight> Select<TLeft, TRight>(this Either<TLeft, TRight> either,
            Func<TRight, TLeft> selector)
        {
            GuardAgainst.ArgumentBeingNull(either, nameof(either));
            GuardAgainst.ArgumentBeingNull(selector, nameof(selector));

            return either switch
            {
                RightEither<TLeft, TRight> right => Either.From<TLeft, TRight>(selector(right)),
                LeftEither<TLeft, TRight> left => Either.From<TLeft, TRight>(left),
                _ => throw new ArgumentException(ProjectResources.UnknownEitherAccess, nameof(either))
            };
        }
    }
}
