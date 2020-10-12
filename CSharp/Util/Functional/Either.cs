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
        /// Constructs an <see cref="Either{TLeft,TRight}"/> from <paramref name="value"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="value"><typeparamref name="TLeft"/> value contained within <see cref="Either{TLeft,TRight}"/></param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> constructed from <paramref name="value"/></returns>
        public static Either<TLeft, TRight> From<TLeft, TRight>(TLeft value) =>
            new LeftEither<TLeft, TRight>(value);

        /// <summary>
        /// Constructs an <see cref="Either{TLeft,TRight}"/> from <paramref name="value"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="value"><typeparamref name="TRight"/> value contained within <see cref="Either{TLeft,TRight}"/></param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> constructed from <paramref name="value"/></returns>
        public static Either<TLeft, TRight> From<TLeft, TRight>(TRight value) =>
            new RightEither<TLeft, TRight>(value);

        /// <summary>
        /// Returns <see cref="Maybe{TLeft}"/> if <paramref name="source"/> contains <typeparamref name="TLeft"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="source">source to return <typeparamref name="TLeft"/> for</param>
        /// <returns>
        /// Returns <see cref="Maybe{TLeft}"/> if <paramref name="source"/> contains <typeparamref name="TLeft"/>;
        /// <see cref="Maybe.Empty{TLeft}"/>
        /// </returns>
        public static Maybe<TLeft> ToLeftValue<TLeft, TRight>(this Either<TLeft, TRight> source) =>
            (source is LeftEither<TLeft, TRight> left)
                ? Maybe.Of(left.Value)
                : Maybe.Empty<TLeft>(); 

        /// <summary>
        /// Returns <see cref="Maybe{TRight}"/> if <paramref name="source"/> contains <typeparamref name="TLeft"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="source">source to return <typeparamref name="TLeft"/> for</param>
        /// <returns>
        /// Returns <see cref="Maybe{TRight}"/> if <paramref name="source"/> contains <typeparamref name="TLeft"/>;
        /// <see cref="Maybe.Empty{TRight}"/>
        /// </returns>
        public static Maybe<TRight> ToRightValue<TLeft, TRight>(this Either<TLeft, TRight> source) =>
            (source is RightEither<TLeft, TRight> right)
                ? Maybe.Of(right.Value)
                : Maybe.Empty<TRight>();
    }

    /// <summary>
    /// container storing one of two possible values
    /// </summary>
    /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
    /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
    /// <remarks>
    /// based on examples from https://app.pluralsight.com/library/courses/making-functional-csharp
    /// </remarks>
    public abstract class Either<TLeft, TRight>
    {
        /// <summary>
        /// implicit conversion operator converting from <typeparamref name="TLeft"/>
        /// </summary>
        /// <param name="value">value to contain</param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static class Either")]
        public static implicit operator Either<TLeft, TRight>(TLeft value) =>
            Either.From<TLeft, TRight>(value);

        /// <summary>
        /// implicit conversion operator converting from <typeparamref name="TRight"/>
        /// </summary>
        /// <param name="value">value to contain</param>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static class Either")]
        public static implicit operator Either<TLeft, TRight>(TRight value) =>
            Either.From<TLeft, TRight>(value);
    }
}
