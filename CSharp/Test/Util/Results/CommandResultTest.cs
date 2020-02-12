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
using SystemEx.Util.Results;
using Xunit;

namespace SystemEx.Test.Util.Results
{
    public class CommandResultTest
    {
        [Fact]
        public void SuccessfulCommandResultReportsSuccess()
        {
            // Arrange
            var uid = CommandResult.Ok();

            // Act

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        public void SuccessfulCommandResultHasEmptyMessageIfNoneProvided()
        {
            // Arrange
            CommandResult uid = CommandResult.Ok();

            // Act

            // Assert
            Assert.Empty(uid.Message);
        }

        [Fact]
        public void SuccessfulCommandResultHasNullCause()
        {
            // Arrange
            CommandResult uid = CommandResult.Ok();

            // Act

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void SuccessfulCommandResultEqualsReturnsTrueForEqualResults()
        {
            // Arrange
            CommandResult leftHandSide = CommandResult.Ok();
            CommandResult rightHandSide = CommandResult.Ok();

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void SuccessfulQueryResultImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            CommandResult result = CommandResult.Ok();

            // Act
            bool @implicit = result;
            bool @explicit = result.ToBoolean();
            bool success = result.Success;

            // Assert
            Assert.Equal(success, @implicit);
            Assert.Equal(@explicit, @implicit);
            Assert.True(success);
        }

        [Fact]
        public void FailureCommandResultByDefault()
        {
            // Arrange
            CommandResult result;

            // Act
            result = new CommandResult();

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void FailureCommandStoresExceptionCause()
        {
            // Arrange
            var exception = new Exception(Guid.NewGuid().ToString(), new Exception($"Inner {Guid.NewGuid()}"));
            var message = Guid.NewGuid().ToString();

            // Act
            var result = CommandResult.Failed(message, exception);

            // Assert
            Assert.Equal(exception, result.Cause);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void FailureCommandResultImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            CommandResult result = CommandResult.Failed(Guid.NewGuid().ToString());

            // Act
            bool @implicit = result;
            bool @explicit = result.ToBoolean();
            bool success = result.Success;

            // Assert
            Assert.Equal(success, @implicit);
            Assert.Equal(@explicit, @implicit);
            Assert.False(success);
        }
    }
}
