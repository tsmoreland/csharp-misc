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

using Moreland.CSharp.Util.Internal;
using System;
using System.Diagnostics;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Results
{
    /// <summary>Wrapper around the result of a command provided boolean result with additional message or cause on failure</summary>
    [DebuggerDisplay("{GetType().Name,nq}: {Success} {Message,nq}")]
    public readonly struct CommandResult : IEquatable<CommandResult>
    {
        private CommandResult(bool success, string message, Exception? cause)
        {
            Result = new ResultCore(success, message, cause);
        }

        /// <summary>
        /// Builds a successful <see cref="CommandResult{TValue}"/> with value 
        /// </summary>
        public static CommandResult<TValue> Ok<TValue>(TValue value) => 
            new CommandResult<TValue>(value, true, string.Empty, null);
        /// <summary>
        /// Builds a successful <see cref="CommandResult{TValue}"/> with value and optional message
        /// </summary>
        public static CommandResult<TValue> Ok<TValue>(TValue value, string message) => 
            new CommandResult<TValue>(value, true, message ?? string.Empty, null);
        /// <summary>
        /// Builds a successful <see cref="CommandResult"/> 
        /// </summary>
        public static CommandResult Ok() => _ok;
        /// <summary>
        /// Builds a successful <see cref="CommandResult"/> with optional message
        /// </summary>
        public static CommandResult Ok(string message) => 
            new CommandResult(true, message ?? string.Empty, null);

        /// <summary>
        /// Builds a <see cref="CommandResult"/> with a failed result
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cause"></param>
        /// <returns></returns>
        public static CommandResult Failed(string message, Exception? cause = null) => 
            new CommandResult(false, message ?? throw new ArgumentNullException(nameof(message)), cause);
        /// <summary>
        /// Builds a <see cref="CommandResult{TValue}"/> with a failed result
        /// </summary>
        public static CommandResult<TValue> Failed<TValue>(string message, Exception? cause = null) => 
            new CommandResult<TValue>(default!, false, message ?? throw new ArgumentNullException(nameof(message)), cause); // allow default, which may be null in this case as it is a failure anyway and we shouldn't be accessing the value

        /// <summary>
        /// represents an unknown error result
        /// </summary>
        public static CommandResult UnknownError { get; } = new CommandResult(false, "Unknown error occurred.", null);

        /// <summary>The result of the operation</summary>
        public bool Success => Result.Success;
        /// <summary>Optional message; should be non-null on failure but may contain a value on success</summary>
        public string Message => Result.Message;
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public Exception? Cause => Result.Cause;
        /// <summary>
        /// Returns <see cref="Success"/>
        /// </summary>
        /// <returns></returns>
        public bool ToBoolean() => Success;

        /// <summary>
        /// Returns <c>true</c> if <paramref name="leftHandSide"/> is equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator==(CommandResult leftHandSide, CommandResult rightHandSide) => 
            leftHandSide.Equals(rightHandSide);
        /// <summary>
        /// Returns <c>true</c> if <paramref name="leftHandSide"/> is not equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator!=(CommandResult leftHandSide, CommandResult rightHandSide) => 
            !(leftHandSide == rightHandSide);
        /// <summary>
        /// Returns <see cref="ToBoolean"/>
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(CommandResult result) => 
            result.ToBoolean();

        /// <summary>Deconstructs the components of <see cref="CommandResult"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out string message)
        {
            success = Success;
            message = Message;
        }
        /// <summary>Deconstructs the components of <see cref="CommandResult"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out string message, out Exception? cause)
        {
            success = Success;
            message = Message;
            cause = Cause;
        }

        private ResultCore Result { get; }
        private static readonly CommandResult _ok = new CommandResult(true, string.Empty, null);

        #region IEquatable{CommandResult}
        /// <summary>
        /// <inheritdoc cref="IEquatable{CommandResult}.Equals(CommandResult)"/>
        /// </summary>
        public bool Equals(CommandResult other) =>
            Result.Equals(other.Result);
        #endregion
        #region ValueType

        /// <summary>
        /// <inheritdoc cref="Object.Equals(Object?)"/>
        /// </summary>
        public override bool Equals(object? obj) => obj is CommandResult rightHandSide && Equals(rightHandSide);
        /// <summary>
        /// <inheritdoc cref="Object.GetHashCode"/>
        /// </summary>
        public override int GetHashCode() => Result.GetHashCode();
        #endregion
    }

    /// <summary>
    /// A Command Result storing a result value of <typeparamref name="TValue"/>
    /// </summary>
    /// <typeparam name="TValue">type of the Value stored on success</typeparam>
    [DebuggerDisplay("{GetType().Name,nq}: {Value} {Success} {Message,nq}")]
    public readonly struct CommandResult<TValue> : IValueResult<TValue>, IEquatable<CommandResult<TValue>>
    {
        internal CommandResult(TValue value, bool success, string message, Exception? cause)
        {
            ValueResult = new ValueResultCore<TValue>(value, success, message, cause);
        }

        /// <summary>Resulting Value of the Query</summary>
        /// <exception cref="InvalidOperationException">thrown if <see cref="Success"/> is <c>false</c></exception>
        public TValue Value => Success 
            ? ValueResult.Value 
            : throw new InvalidOperationException(ProjectResources.InvalidResultValueAccess);

        /// <summary>The result of the operation</summary>
        public bool Success => ValueResult.Success;
        /// <summary>Optional message; should be non-empty of failure but may have a value on success</summary>
        public string Message => ValueResult.Message;
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public Exception? Cause => ValueResult.Cause;
        /// <summary>
        /// Returns <see cref="Success"/>
        /// </summary>
        public bool ToBoolean() => Success;

        /// <summary>
        /// Returns <c>true</c> if <paramref name="leftHandSide"/> is equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator==(CommandResult<TValue> leftHandSide, CommandResult<TValue> rightHandSide) => leftHandSide.Equals(rightHandSide);
        /// <summary>
        /// Returns <c>true</c> if <paramref name="leftHandSide"/> is not equal to <paramref name="rightHandSide"/>
        /// </summary>
        public static bool operator!=(CommandResult<TValue> leftHandSide, CommandResult<TValue> rightHandSide) => !(leftHandSide == rightHandSide);
        /// <summary>
        /// Returns <see cref="ToBoolean"/>
        /// </summary>
        public static implicit operator bool(CommandResult<TValue> result) => result.ToBoolean();
        /// <summary>
        /// Returns <see cref="Value"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">if <see cref="Success"/> is not <c>true</c></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by Value property")]
        public static explicit operator TValue(CommandResult<TValue> result) => result.Value;

        /// <summary>Deconstructs the components of <see cref="CommandResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out Maybe<TValue> value)
        {
            success = Success;
            value = Success ? Maybe.Of(Value) : Maybe.Empty<TValue>();
        }
        /// <summary>Deconstructs the components of <see cref="CommandResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out Maybe<TValue> value, out string message)
        {
            success = Success;
            value = Success ? Maybe.Of(Value) : Maybe.Empty<TValue>();
            message = Message;
        }
        /// <summary>Deconstructs the components of <see cref="CommandResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out Maybe<TValue> value, out string message, out Exception? cause)
        {
            success = Success;
            value = Success ? Maybe.Of(Value) : Maybe.Empty<TValue>();
            message = Message;
            cause = Cause;
        }
        /// <summary>
        /// If a value is present, apply the provided Optional-bearing mapping function to it, 
        /// return that result, otherwise return an empty Optional.
        /// </summary>
        public CommandResult<TMappedValue> Select<TMappedValue>(Func<TValue, CommandResult<TMappedValue>> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (!Success)
                return CommandResult.Failed<TMappedValue>(Message, Cause);
            return selector.Invoke(Value);
        }

        /// <summary>If a value is present, apply the provided mapping function to it, and if the result is non-null, return a Maybe describing the result.</summary>
        public CommandResult<TMappedValue> Select<TMappedValue>(Func<TValue, TMappedValue> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (!Success)
                return CommandResult.Failed<TMappedValue>(Message, Cause);
            // map type explicit to avoid any possible issues with TMappedValue being string
            return CommandResult.Ok(selector.Invoke(Value));
        }

        /// <summary>Returns the value if present, otherwise returns <paramref name="other"/>.</summary>
        /// <remarks>slightly awkward name due to OrElse being reserved keyword (VB)</remarks>
        public TValue ValueOr(TValue other) => Success ? Value : other;

        /// <summary>Returns the value if present, otherwise invokes other and returns the result of that invocation.</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="other"/> is <c>null</c></exception>
        public TValue ValueOr(Func<TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return Success ? Value : other.Invoke();
        }
	
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="Success"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue ValueOrThrow(Func<Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (Success)
                return Value;
            throw exceptionSupplier.Invoke();
        }
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <param name="exceptionSupplier">exception builder taking the message and optional Exception that caused the failiure</param>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="Success"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue ValueOrThrow(Func<string, Exception?, Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (Success)
                return Value;
            throw exceptionSupplier.Invoke(Message, Cause);
        }

        /// <summary>
        /// Returns the current result if successful or uses <paramref name="mapper"/> to handle the error
        /// allow the result to be converted to an alternate successful result
        /// </summary>
        /// <param name="mapper">
        /// Func  given message and exception cause and returns a
        /// <see cref="CommandResult{TValue}"/> to an alternate result
        /// </param>
        /// <returns>
        /// current result if successful or uses <paramref name="mapper"/> to handle the error
        /// allow the result to be converted to an alternate successful result
        /// </returns>
        public CommandResult<TValue> ValueOr(Func<string, Exception?, CommandResult<TValue>> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            return Success
                ? this
                : mapper.Invoke(Message, Cause);
        }

        private ValueResultCore<TValue> ValueResult { get; }

        #region IEquatable{QueryResult{TValue}}
        /// <summary>
        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        /// </summary>
        public bool Equals(CommandResult<TValue> other) =>
            ValueResult.Equals(other.ValueResult);

        #endregion
        #region ValueType

        /// <summary>
        /// <inheritdoc cref="Object.Equals(Object?)"/>
        /// </summary>
        public override bool Equals(object? obj) => obj is CommandResult<TValue> rightHandSide && Equals(rightHandSide);
        /// <summary>
        /// <inheritdoc cref="Object.GetHashCode"/>
        /// </summary>
        public override int GetHashCode() => ValueResult.GetHashCode();

        #endregion
    }
}
