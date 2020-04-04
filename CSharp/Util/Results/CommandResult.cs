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
using System;
using System.Diagnostics;

namespace System.Util.Results
{
    /// <summary>Wrapper around the result of a command provided boolean result with additional message or cause on failure</summary>
    [DebuggerDisplay("{GetType().Name,nq}: {Success} {Message,nq}")]
    public struct CommandResult : IEquatable<CommandResult>
    {
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
}
