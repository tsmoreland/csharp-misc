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
using System.Linq;
using System.Threading.Tasks;
using Moreland.CSharp.Util.Extensions;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util
{
    /// <summary>
    /// Maybe Factory methods
    /// </summary>
    public static class Maybe
    {
        /// <summary>Returns an empty Maybe instance.</summary>
        public static Maybe<TValue> Empty<TValue>() => new Maybe<TValue>();
        /// <summary>Returns a Maybe with the specified present value.</summary>
        public static Maybe<TValue> Of<TValue>(TValue value) => new Maybe<TValue>(value);
        /// <summary>Returns a Maybe describing the specified value, if non-null, otherwise returns an empty Maybe.</summary>
        public static Maybe<TValue> OfNullable<TValue>(TValue? value) where TValue : struct => value.HasValue ? new Maybe<TValue>(value.Value) : Empty<TValue>();
        /// <summary>Returns a Maybe describing the specified value, if non-null, otherwise returns an empty Maybe.</summary>
        public static Maybe<TValue> OfNullable<TValue>(TValue? value) where TValue : class => value != null ? new Maybe<TValue>(value) : Empty<TValue>();
    }

    /// <summary>Maybe class heavily influenced by java.util.Optional{T}</summary>
    public readonly struct Maybe<TValue> : IEquatable<Maybe<TValue>>
    {
        /// <summary>If a value is present in this Maybe, returns the value, otherwise throws <see cref="InvalidOperationException"/></summary>
        /// <exception cref="InvalidOperationException">when <see cref="HasValue"/> is false</exception>
        public TValue Value => 
            HasValue ? _value : throw new InvalidOperationException(ProjectResources.NoSuchValue);
        /// <summary>
        /// Returns <c>true</c> if <see cref="Maybe{TValue}"/> contains a value
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Returns <c>true</c> if <see cref="HasValue"/> is <c>false</c>
        /// </summary>
        public bool IsEmpty => !HasValue;

        /// <summary>
        /// If a value is present, performs the given action with the value, otherwise does nothing.
        /// </summary>
        /// <param name="action">the action to be performed, if a value is present</param>
        /// <exception cref="ArgumentNullException">if <paramref name="action"/> is null</exception>
        public void IfHasValue(Action<TValue> action)
        {
            GuardArgumentNull(action, nameof(action));

            if (HasValue)
                action.Invoke(Value);
        }

        /// <summary>
        /// If a value is present, performs the given action with the value, otherwise performs the given empty-based action.
        /// </summary>
        /// <param name="action">the action to be performed, if a value is present</param>
        /// <param name="emptyAction">the empty-based action to be performed, if no value is present</param>
        /// <exception cref="ArgumentNullException">if a value is present and the given action is null, or no value is present and the given empty-based action is null.</exception>
        public void IfHasValue(Action<TValue> action, Action emptyAction)
        {
            GuardArgumentNull(action, nameof(action));
            GuardArgumentNull(emptyAction, nameof(emptyAction));

            if (HasValue)
                action.Invoke(Value);
            else
                emptyAction.Invoke();
        }

#if !NET40
        /// <summary>
        /// If a value is present, performs the given action with the value, otherwise does nothing.
        /// </summary>
        /// <param name="action">the action to be performed, if a value is present</param>
        /// <exception cref="ArgumentNullException">if <paramref name="action"/> is null</exception>
        public async Task IfHasValueAsync(Func<TValue, Task> action)
        {
            GuardArgumentNull(action, nameof(action));

            if (HasValue)
                await action.Invoke(Value).ConfigureAwait(true);
        }

        /// <summary>
        /// If a value is present, performs the given action with the value, otherwise performs the given empty-based action.
        /// </summary>
        /// <param name="action">the action to be performed, if a value is present</param>
        /// <param name="emptyAction">the empty-based action to be performed, if no value is present</param>
        /// <exception cref="ArgumentNullException">if a value is present and the given action is null, or no value is present and the given empty-based action is null.</exception>
        public async Task IfHasValueAsync(Func<TValue, Task> action, Func<Task> emptyAction)
        {
            GuardArgumentNull(action, nameof(action));
            GuardArgumentNull(emptyAction, nameof(emptyAction));

            if (HasValue)
                await action.Invoke(Value).ConfigureAwait(true);
            else
                await emptyAction.Invoke().ConfigureAwait(true);
        }
#endif

        /// <summary>If a value is present and the value matches a given predicate, it returns a Maybe describing the value, otherwise returns an empty Maybe.</summary>
        // ReSharper disable once ConstantConditionalAccessQualifier
        public Maybe<TValue> Where(Predicate<TValue> predicate) => HasValue && predicate?.Invoke(Value) == true
            ? this 
            : Maybe.Empty<TValue>();

        /// <summary>
        /// If a value is present, it applies the provided Maybe-bearing
        /// mapping function to it, returns that result, otherwise returns an
        /// empty Maybe.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="selector"/> is <c>null</c></exception>
        public Maybe<TMappedValue> Select<TMappedValue>(Func<TValue, TMappedValue> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return HasValue
                ? Maybe.Of(selector.Invoke(Value))
                : Maybe.Empty<TMappedValue>();
        }

        /// <summary>
        /// If a value is present, it applies the provided Maybe-bearing
        /// mapping function to it, returns that result, otherwise returns an
        /// empty Maybe.
        /// </summary>
        /// <exception cref="ArgumentNullException">when <paramref name="selector"/> is null</exception>
        public Maybe<TMappedValue> Select<TMappedValue>(Func<TValue, Maybe<TMappedValue>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return !HasValue 
                ? Maybe.Empty<TMappedValue>() 
                : selector.Invoke(Value);
        }

        /// <summary>Returns the value if present, otherwise returns <paramref name="other"/>.</summary>
        /// <remarks>slightly awkward name due to OrElse being reserved keyword (VB)</remarks>
        public TValue ValueOr(TValue other) => HasValue 
            ? Value 
            : other;

        /// <summary>Returns the value if present, otherwise invokes other and returns the result of that invocation.</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="other"/> is <c>null</c></exception>
        public TValue ValueOr(Func<TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return HasValue ? _value : other.Invoke();
        }
	
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="HasValue"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue ValueOrThrow(Func<Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (HasValue)
                return _value;
            throw exceptionSupplier.Invoke();
        }

        /// <summary>
        /// if value is present, returns an <see cref="IEnumerable{TValue}"/> containing only that value;
        /// otherwise an empty <see cref="IEnumerable{TValue}"/>
        /// </summary>
        public IEnumerable<TValue> AsEnumerable() => HasValue
            ? ArrayExtensions.Of(Value)
            : Enumerable.Empty<TValue>();

        /// <summary>
        /// Returns <see cref="HasValue"/>
        /// </summary>
        public bool ToBoolean() => HasValue;

        /// <summary>
        /// Returns <c>true</c> if <paramref name="first"/> is equal to <paramref name="second"/>
        /// </summary>
        public static bool operator==(Maybe<TValue> first, Maybe<TValue> second) => 
            first.Equals(second);
        /// <summary>
        /// Returns <c>true</c> if <paramref name="first"/> is not equal to <paramref name="second"/>
        /// </summary>
        public static bool operator!=(Maybe<TValue> first, Maybe<TValue> second) => 
            !(first == second);
        /// <summary>
        /// Returns <see cref="ToBoolean"/>
        /// </summary>
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

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void GuardArgumentNull(object? argument, string parameterName)
        {
            if (argument == null)
                throw new ArgumentNullException(parameterName);
        }

        #region IEquatable{Maybe{TValue}}
        /// <summary>
        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        /// </summary>
        public bool Equals(Maybe<TValue> other) =>
            (!HasValue && !other.HasValue) || (HasValue && other.HasValue && Value?.Equals(other.Value) == true);

        #endregion
        #region Object
        /// <summary>
        /// <inheritdoc cref="ValueType.Equals(object?)"/>
        /// </summary>
        public override bool Equals(object? obj) => obj is Maybe<TValue> maybeObj && Equals(maybeObj);
        /// <summary>
        /// <inheritdoc cref="ValueType.GetHashCode"/>
        /// </summary>
        public override int GetHashCode() => HasValue
            ? Value?.GetHashCode() ?? 0
            : 0;
        /// <summary>
        /// <inheritdoc cref="ValueType.ToString"/>
        /// </summary>
        public override string ToString() =>
            HasValue ? _value?.ToString() ?? ProjectResources.NullValue : ProjectResources.NoSuchValue;
        #endregion
    }
}
