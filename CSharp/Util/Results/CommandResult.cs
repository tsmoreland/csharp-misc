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

using System.Util.Internal;
using System.Diagnostics;
using ProjectResources = System.Util.Properties.Resources; 

namespace System.Util.Results
{
    /// <summary>Wrapper around the result of a command provided boolean result with additional message or cause on failure</summary>
    [DebuggerDisplay("{GetType().Name,nq}: {Success} {Message,nq}")]
    public struct CommandResult : IEquatable<CommandResult>
    {
        public static CommandResult<TValue> Ok<TValue>(TValue value) => new CommandResult<TValue>(value, true, string.Empty, null);
        public static CommandResult<TValue> Ok<TValue>(TValue value, string message) => new CommandResult<TValue>(value, true, message ?? string.Empty, null);
        public static CommandResult<TValue> Failed<TValue>(string message, Exception? cause = null) => 
            new CommandResult<TValue>(default!, false, message, cause); // allow default, which may be null in this case as it is a failure anyway and we shouldn't be accessing the value

        private CommandResult(bool success, string message, Exception? cause)
        {
            Result = new ResultCore(success, message, cause);
        }

        public static CommandResult Ok() => _ok;
        public static CommandResult Ok(string message) => new CommandResult(true, message ?? string.Empty, null);
        public static CommandResult Failed(string message, Exception? cause = null) => new CommandResult(false, message ?? throw new ArgumentNullException(nameof(message)), cause);
        public static CommandResult UnknownError { get; } = new CommandResult(false, "Unknown error occurred.", null);

        /// <summary>The result of the operation</summary>
        public bool Success => Result.Success;
        /// <summary>Optional message; should be non-null on failure but may contain a value on success</summary>
        public string Message => Result.Message;
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public Exception? Cause => Result.Cause;
        public bool ToBoolean() => Success;

        public static bool operator==(CommandResult leftHandSide, CommandResult rightHandSide) => leftHandSide.Equals(rightHandSide);
        public static bool operator!=(CommandResult leftHandSide, CommandResult rightHandSide) => !(leftHandSide == rightHandSide);
        public static implicit operator bool(CommandResult result) => result.ToBoolean();

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

        #region ValueType

        public override bool Equals(object? obj) => obj is CommandResult rightHandSide ? Equals(rightHandSide) : false;
        public override int GetHashCode() => Result.GetHashCode();
        #endregion
        #region IEquatable{CommandResult}
        public bool Equals(CommandResult other) =>
            Result.Equals(other.Result);
        #endregion
    }

    [DebuggerDisplay("{GetType().Name,nq}: {Value} {Success} {Message,nq}")]
    public struct CommandResult<TValue> : IValueResult<TValue>, IEquatable<CommandResult<TValue>>
    {
        internal CommandResult(TValue value, bool success, string message, Exception? cause)
        {
            ValueResult = new ValueResultCore<TValue>(value, success, message, cause);
        }

        /// <summary>Resulting Value of the Query</summary>
        /// <exception cref="InvalidOperationException">thrown if <see cref="Success"/> is <c>false</c></exception>
        public TValue Value => Success ? ValueResult.Value : throw new InvalidOperationException(ProjectResources.InvalidQueryResultValueAccess);
        /// <summary>The result of the operation</summary>
        public bool Success => ValueResult.Success;
        /// <summary>Optional message; should be non-empty of failure but may have a value on success</summary>
        public string Message => ValueResult.Message;
        /// <summary>Exceptional cause of the failure, only meaningful if <see cref="Success"/> is <c>false</c></summary>
        public Exception? Cause => ValueResult.Cause;
        public bool ToBoolean() => Success;

        public static bool operator==(CommandResult<TValue> leftHandSide, CommandResult<TValue> rightHandSide) => leftHandSide.Equals(rightHandSide);
        public static bool operator!=(CommandResult<TValue> leftHandSide, CommandResult<TValue> rightHandSide) => !(leftHandSide == rightHandSide);
        public static implicit operator bool(CommandResult<TValue> result) => result.ToBoolean();
        [Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by Value property")]
        public static explicit operator TValue(CommandResult<TValue> result) => result.Value;

        /// <summary>Deconstructs the components of <see cref="CommandResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out TValue value)
        {
            success = Success;
            value = Value;
        }
        /// <summary>Deconstructs the components of <see cref="CommandResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out TValue value, out string message)
        {
            success = Success;
            value = Value;
            message = Message;
        }
        /// <summary>Deconstructs the components of <see cref="CommandResult{TValue}"/> into seperate variables</summary>
        public void Deconstruct(out bool success, out TValue value, out string message, out Exception? cause)
        {
            success = Success;
            value = Value;
            message = Message;
            cause = Cause;
        }

        /// <summary>If a value is present, it applies the provided Maybe-bearing mapping function to it, returns that result, otherwise returns an empty Maybe. </summary>
        public CommandResult<TMappedValue> FlatMap<TMappedValue>(Func<TValue, TMappedValue> mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            if (!Success)
                return CommandResult.Failed<TMappedValue>(Message, Cause);
            // map type explicit to avoid any possible issues with TMappedValue being string
            return CommandResult.Ok<TMappedValue>(mapper.Invoke(Value));
        }

        /// <summary>Returns the value if present, otherwise returns <paramref name="other"/>.</summary>
        /// <remarks>slightly awkward name due to OrElse being reserved keyword (VB)</remarks>
        public TValue OrElseOther(TValue other) => Success ? Value : other;

        /// <summary>Returns the value if present, otherwise invokes other and returns the result of that invocation.</summary>
        /// <exception cref="ArgumentNullException">if <paramref name="other"/> is <c>null</c></exception>
        public TValue OrElseGet(Func<TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            return Success ? Value : other.Invoke();
        }
	
        /// <summary>Returns the contained value, if present, otherwise throws an exception to be created by the provided supplier. </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="exceptionSupplier"/> is null</exception>
        /// <exception cref="Exception">if <see cref="HasValue"/> is false then result of <paramref name="exceptionSupplier"/> is thrown</exception>
        public TValue OrElseThrow(Func<Exception> exceptionSupplier)
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            if (Success)
                return Value;
            throw exceptionSupplier.Invoke();
        }

        private ValueResultCore<TValue> ValueResult { get; }

        #region ValueType

        public override bool Equals(object? obj) => obj is CommandResult<TValue> rightHandSide ? Equals(rightHandSide) : false;
        public override int GetHashCode() => ValueResult.GetHashCode();

        #endregion
        #region IEquatable{QueryResult{TValue}}
        public bool Equals(CommandResult<TValue> other) =>
            ValueResult.Equals(other.ValueResult);

        #endregion
    }
}
