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

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD2_0 || NETCOREAPP2_1
using Moreland.CSharp.Util.Modernizer;
#endif

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
            new Either<TLeft, TRight>(value);

        /// <summary>
        /// Constructs an <see cref="Either{TLeft,TRight}"/> from <paramref name="value"/>
        /// </summary>
        /// <typeparam name="TLeft">Primary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <typeparam name="TRight">Secondary type of the <see cref="Either{TLeft,TRight}"/></typeparam>
        /// <param name="value"><typeparamref name="TRight"/> value contained within <see cref="Either{TLeft,TRight}"/></param>
        /// <returns>An <see cref="Either{TLeft,TRight}"/> constructed from <paramref name="value"/></returns>
        public static Either<TLeft, TRight> From<TLeft, TRight>(TRight value) =>
            new Either<TLeft, TRight>(value);

    }

    /// <summary>
    /// Value Object containing one of two possible values, either <typeparamref name="TLeft"/> or <typeparamref name="TRight"/>
    /// </summary>
    public readonly struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
    {
        private readonly Maybe<TLeft> _left;
        private readonly Maybe<TRight> _right;

        /// <summary>
        /// Instantiates a new instance of the <see cref="Either{TLeft,TRight}"/> class.
        /// </summary>
        /// <param name="value"><typeparamref name="TLeft"/> value</param>
        public Either(TLeft value)
        {
            _left = Maybe.Of(value);
            _right = Maybe.Empty<TRight>();
        }
        /// <summary>
        /// Instantiates a new instance of the <see cref="Either{TLeft,TRight}"/> class.
        /// </summary>
        /// <param name="value"><typeparamref name="TRight"/> value</param>
        public Either(TRight value)
        {
            _left = Maybe.Empty<TLeft>();
            _right = Maybe.Of(value);
        }

        internal Maybe<TLeft> MaybeLeftValue => _left;
        internal Maybe<TRight> MaybeRightValue => _right;

        /// <summary>
        /// Returns <c>true</c> if this instance contains no values,
        /// this can happen if the parameterless constructor is used
        /// and should be considered invalid.
        /// </summary>
        public bool IsEmpty => 
            _left.IsEmpty && _right.IsEmpty;

        /// <summary>
        /// Returns <c>true</c> if <see cref="Either{TLeft,TRight}"/> contains a <typeparamref name="TLeft"/> value.
        /// </summary>
        public bool HasLeftValue => _left.HasValue;

        /// <summary>
        /// Returns Left Value if present, otherwise throws exception
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// if this instance does not have a <typeparamref name="TLeft"/> value
        /// </exception>
        public TLeft LeftValue => _left.Value;

        /// <summary>
        /// Returns <c>true</c> if <see cref="Either{TLeft,TRight}"/> contains a <typeparamref name="TRight"/> value.
        /// </summary>
        public bool HasRightValue => _right.HasValue;

        /// <summary>
        /// Returns Left Value if present, otherwise throws exception
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// if this instance does not have a <typeparamref name="TRight"/> value
        /// </exception>
        public TRight RightValue => _right.Value;

        /// <summary>
        /// Implicitly constructs instance of <see cref="Either{TLeft,TRight}"/> from <paramref name="value"/>
        /// </summary>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static class Either")]
        public static implicit operator Either<TLeft, TRight>(TLeft value) =>
            new Either<TLeft, TRight>(value);

        /// <summary>
        /// Implicitly constructs instance of <see cref="Either{TLeft,TRight}"/> from <paramref name="value"/>
        /// </summary>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static class Either")]
        public static implicit operator Either<TLeft, TRight>(TRight value) =>
            new Either<TLeft, TRight>(value);

        /// <summary>
        /// Indicates whether <paramref name="first"/> and <paramref name="second"/> are equal
        /// </summary>
        public static bool operator ==(Either<TLeft, TRight> first, Either<TLeft, TRight> second) =>
            first.Equals(second);

        /// <summary>
        /// Indicates whether <paramref name="first"/> and <paramref name="second"/> are not equal
        /// </summary>
        public static bool operator !=(Either<TLeft, TRight> first, Either<TLeft, TRight> second) =>
            !(first == second);

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            obj is Either<TLeft, TRight> either && Equals(either);

        /// <inheritdoc/>
        public bool Equals([AllowNull] Either<TLeft, TRight> other) =>
            other != null! && _left.Equals(other._left) && _right.Equals(other._right);

        /// <summary>
        /// Returns <typeparamref name="TLeft"/> if present, otherwise default
        /// </summary>
        public TLeft LeftValueOrDefault() =>
            _left.ValueOr(default(TLeft)!);

        /// <summary>
        /// Returns <typeparamref name="TLeft"/> if present, otherwise default
        /// </summary>
        public TRight RightValueOrDefault() =>
            _right.ValueOr(default(TRight)!);

        /// <summary>
        /// Returns Maybe{TLeft} of the left value, this value will be
        /// <see cref="Maybe.Empty{TLeft}()"/> if this instance contains
        /// <typeparamref name="TRight"/>
        /// </summary>
        public static implicit operator Maybe<TLeft>(Either<TLeft, TRight> source) =>
            source._left;

        /// <summary>
        /// Returns Maybe{TLeft} of the left value, this value will be
        /// <see cref="Maybe.Empty{TLeft}()"/> if this instance contains
        /// <typeparamref name="TRight"/>
        /// </summary>
        public Maybe<TLeft> FromEither() => 
            _left;

        /// <inheritdoc />
        public override int GetHashCode() =>
            HashCodeBuilder.Create(_left, _right).ToHashCode();

    }
}
