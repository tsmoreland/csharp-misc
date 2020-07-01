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

namespace Moreland.CSharp.Util
{
    public static class Maybe
    {
        /// <summary>Returns an empty Maybe instance.</summary>
        public static Maybe<TValue> Empty<TValue>() => new Maybe<TValue>();
        /// <summary>Returns a Maybe with the specified present value.</summary>
        public static Maybe<TValue> Of<TValue>(TValue value) => new Maybe<TValue>(value);
        /// <summary>Returns a Maybe describing the specified value, if non-null, otherwise returns an empty Maybe.</summary>
        public static Maybe<TValue> OfNullable<TValue>(TValue? value) where TValue : struct => value.HasValue ? new Maybe<TValue>(value.Value) : Empty<TValue>();
        /// <summary>Returns a Maybe describing the specified value, if non-null, otherwise returns an empty Maybe.</summary>
        public static Maybe<TValue> OfNullable<TValue>(TValue value) where TValue : class => value != null ? new Maybe<TValue>(value) : Empty<TValue>();
    }

    /// <summary>Maybe class heavily influenced by java.util.Optional{T}</summary>
    public readonly struct Maybe<TValue> : IEquatable<Maybe<TValue>>
    {
        /// <summary>If a value is present in this Maybe, returns the value, otherwise throws <see cref="InvalidOperationException"/></summary>
        /// <exception cref="InvalidOperationException">when <see cref="HasValue"/> is false</exception>
        public TValue Value => 
            HasValue ? _value : throw new InvalidOperationException(ProjectResources.NoSuchValue);
        public bool HasValue { get; }

        /// <summary>If a value is present and the value matches a given predicate, it returns a Maybe describing the value, otherwise returns an empty Maybe.</summary>
        public Maybe<TValue> Filter(Predicate<TValue> predicate) => 
            Where(predicate);

        /// <summary>If a value is present and the value matches a given predicate, it returns a Maybe describing the value, otherwise returns an empty Maybe.</summary>
        public Maybe<TValue> Where(Predicate<TValue> predicate) => HasValue && predicate?.Invoke(Value) == true 
            ? this 
            : Maybe.Empty<TValue>();

        /// <summary>If a value is present, it applies the provided Maybe-bearing mapping function to it, returns that result, otherwise returns an empty Maybe. </summary>
        public Maybe<TMappedValue> Select<TMappedValue>(Func<TValue, TMappedValue> selector) =>
            Map(selector);

        /// <summary>If a value is present, it applies the provided Maybe-bearing mapping function to it, returns that result, otherwise returns an empty Maybe. </summary>
        public Maybe<TMappedValue> Map<TMappedValue>(Func<TValue, TMappedValue> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            if (!HasValue)
                return Maybe.Empty<TMappedValue>();
            return Maybe.Of(mapper.Invoke(Value));
        }

        /// <summary>If a value is present, it applies the provided Maybe-bearing mapping function to it, returns that result, otherwise returns an empty Maybe. </summary>
        public Maybe<TMappedValue> FlatMap<TMappedValue>(Func<TValue, Maybe<TMappedValue>> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            if (!HasValue)
                return Maybe.Empty<TMappedValue>();
            return mapper.Invoke(Value);
        }

        /// <summary>Returns the value if present, otherwise returns <paramref name="other"/>.</summary>
        /// <remarks>slightly awkward name due to OrElse being reserved keyword (VB)</remarks>
        public TValue OrElseOther(TValue other) => HasValue 
            ? Value 
            : other;

        /// <summary>Returns the value if present, otherwise invokes other and returns the result of that invocation.</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="other"/> is <c>null</c></exception>
        public TValue OrElseGet(Func<TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return HasValue ? _value : other.Invoke();
        }
	
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="HasValue"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue OrElseThrow(Func<Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (HasValue)
                return _value;
            throw exceptionSupplier.Invoke();
        }

        public bool ToBoolean() => HasValue;

        public static bool operator==(Maybe<TValue> leftHandSide, Maybe<TValue> rightHandSide) => 
            leftHandSide.Equals(rightHandSide);
        public static bool operator!=(Maybe<TValue> leftHandSide, Maybe<TValue> rightHandSide) => 
            !(leftHandSide == rightHandSide);
        public static implicit operator bool(Maybe<TValue> maybe) => maybe.ToBoolean();

        /// <summary>Returns the Value</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="maybe"/> is <c>null</c></exception>
        /// <exception cref="InvalidOperationException">if <see cref="HasValue"/> is <c>false</c></exception>
        /// <remarks>
        /// the following cast won't work if <typeparamref name="TValue"/> is <see cref="object"/> as that will bypass this method and instead
        /// return the <see cref="Maybe{TValue}"/> as an <see cref="object"/> instead
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by Value property")]
        public static explicit operator TValue(Maybe<TValue> maybe) => 
            maybe.Value;

        internal Maybe(TValue value)
        {
            _value = value;
            HasValue = value != null;
        }
        private readonly TValue _value;

        #region IEquatable{Maybe{TValue}}
        public bool Equals(Maybe<TValue> other) =>
            (!HasValue && !other.HasValue) || (HasValue && other.HasValue && Value?.Equals(other.Value) == true);

        #endregion
        #region Object
        public override bool Equals(object? obj) => obj is Maybe<TValue> maybeObj && Equals(maybeObj);
        public override int GetHashCode() => HasValue
            ? Value?.GetHashCode() ?? 0
            : 0;
        public override string ToString() =>
            HasValue ? _value?.ToString() ?? ProjectResources.NullValue : ProjectResources.NoSuchValue;
        #endregion
    }
}
