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
using System.Diagnostics.CodeAnalysis;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Functional
{
    /// <summary>
    /// Static Factory methods for <see cref="Either{TLeft,TRight}"/>
    /// </summary>
    public static class Either
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Either<TLeft, TRight> From<TLeft, TRight>(TLeft value) =>
            new LeftEither<TLeft, TRight>(value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Either<TLeft, TRight> From<TLeft, TRight>(TRight value) =>
            new RightEither<TLeft, TRight>(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="either"></param>
        /// <returns></returns>
        public static TLeft ToLeftValue<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            if (either is LeftEither<TLeft, TRight> left)
                return left.Value;
            throw new ArgumentException(ProjectResources.BadEitherAccessLeft, nameof(either));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="either"></param>
        /// <returns></returns>
        public static TRight ToRightValue<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            if (either is RightEither<TLeft, TRight> right)
                return right.Value;
            throw new ArgumentException(ProjectResources.BadEitherAccessRight, nameof(either));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <remarks>
    /// based on examples from https://app.pluralsight.com/library/courses/making-functional-csharp
    /// </remarks>
    public abstract class Either<TLeft, TRight>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static class Either")]
        public static implicit operator Either<TLeft, TRight>(TLeft value) =>
            Either.From<TLeft, TRight>(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static class Either")]
        public static implicit operator Either<TLeft, TRight>(TRight value) =>
            Either.From<TLeft, TRight>(value);
    }
}
