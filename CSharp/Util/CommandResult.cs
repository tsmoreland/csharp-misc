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

using Maple.Util.Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Maple.Util
{
    [DebuggerDisplay("{Success} {Reason}")]
    public struct CommandResult : IEquatable<CommandResult>
    {
        private CommandResult(bool success, string reason, Exception? cause)
        {
            Result = new ResultCore(success, reason, cause);
        }

        public static CommandResult Ok { get; } = new CommandResult(true, string.Empty, null);
        public static CommandResult Failure(string reason) => new CommandResult(false, reason ?? throw new ArgumentNullException(nameof(reason)), null);
        public static CommandResult UnkownError { get; } = new CommandResult(false, "Unknown error occurred.", null);

        public bool Success => Result.Success;
        public string Reason => Result.Reason;
        public Exception? Cause => Result.Cause;

        private ResultCore Result { get; }

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
