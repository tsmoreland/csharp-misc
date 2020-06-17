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
using System.Security.Cryptography;
using CSharp.Util.Results;
using Xunit;

namespace CSharp.Test.Util.Results
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

        [Fact]
        public void SuccessfulCommandResultOfTReportsSuccess()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = CommandResult.Ok(value);

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        public void SucessfulQueryContainsCorrectValueForValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = CommandResult.Ok(value);

            // Assert
            Assert.Equal(value, uid.Value); 
        }

        [Fact]
        public void SucessfulQueryContainsCorrectValueForReferenceType()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();

            // Act
            var @string = CommandResult.Ok<string>(value);

            // Assert
            Assert.Equal(value, @string.Value); 
        }
        
        [Fact]
        public void SuccessfulCommandResultOfTHasEmptyMessageIfNoneProvided()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            CommandResult<Guid> uid = CommandResult.Ok(value);

            // Assert
            Assert.Empty(uid.Message);
        }

        [Fact]
        public void SuccessfulCommandResultOfTHasNullCause()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            CommandResult<Guid> uid = CommandResult.Ok(value);

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void SuccessfulCommandResultOfTEqualsReturnsTrueForEqualResultsWithValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            CommandResult<Guid> leftHandSide = CommandResult.Ok(value);
            CommandResult<Guid> rightHandSide = CommandResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void SuccessfulCommandResultOfTEqualsReturnsTrueForEqualResultsWithSameReference()
        {
            // Arrange
            var generator = RNGCryptoServiceProvider.Create();
            byte[] data = new byte[sizeof(int)];
            generator.GetBytes(data);
            int randomNumber = BitConverter.ToInt32(data);

            CommandResult<EquatableReferenceType> leftHandSide = CommandResult.Ok(new EquatableReferenceType(randomNumber));
            CommandResult<EquatableReferenceType> rightHandSide = CommandResult.Ok(new EquatableReferenceType(randomNumber));

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }
        [Fact]
        public void SuccessfulCommandResultOfTEqualsReturnsTrueForEqualResultsForEquatableReferenceType()
        {
            // Arrange
            Exception value = new Exception("ERROR");
            CommandResult<Exception> leftHandSide = CommandResult.Ok(value);
            CommandResult<Exception> rightHandSide = CommandResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void SuccessfulCommandResultOfTImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            CommandResult<Guid> result = CommandResult.Ok<Guid>(Guid.NewGuid());

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
        public void FailureCommandResultOfTByDefault()
        {
            // Arrange
            CommandResult<Guid> result;

            // Act
            result = new CommandResult<Guid>();

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void FailedCommandResultOfTThrowsOnValueAccess()
        {
            // Arrange
            var failed = CommandResult.Failed<Guid>(Guid.NewGuid().ToString());

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = failed.Value);
        }

        [Fact]
        public void FailureQueryStoresExceptionCause()
        {
            // Arrange
            var exception = new Exception(Guid.NewGuid().ToString(), new Exception($"Inner {Guid.NewGuid()}"));
            var message = Guid.NewGuid().ToString();

            // Act
            var result = CommandResult.Failed<Guid>(message, exception);

            // Assert
            Assert.Equal(exception, result.Cause);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void FailureCommandResultOfTImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            CommandResult<Guid> result = CommandResult.Failed<Guid>(Guid.NewGuid().ToString());

            // Act
            bool @implicit = result;
            bool @explicit = result.ToBoolean();
            bool success = result.Success;

            // Assert
            Assert.Equal(success, @implicit);
            Assert.Equal(@explicit, @implicit);
            Assert.False(success);
        }

        [Fact]
        public void SucessfulResult_FlatMapInvoked() => 
            TestContext.SucessfulResult_FlatMapInvoked(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void SucessfulResult_FlatMapAppliedResultSuccess() =>
            TestContext.SucessfulResult_FlatMapAppliedResultSuccess(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseNotUsed() =>
            TestContext.SucessfulResult_OrElseNotUsed(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseGetOtherNotInvoked() =>
            TestContext.SucessfulResult_OrElseGetOtherNotInvoked(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseGetNotUsedValueMatches() =>
            TestContext.SucessfulResult_OrElseGetNotUsedValueMatches(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseThrowDoesNotThrow() =>
            TestContext.SucessfulResult_OrElseThrowDoesNotThrow(() => TestContext.BuildCommandContext<Guid>());
        [Fact]
        public void SucessfulResult_OrElseThrowOverloadDoesNotThrow() =>
            TestContext.SucessfulResult_OrElseThrowOverloadDoesNotThrow(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseThrowDoesValueMatches() =>
            TestContext.SucessfulResult_OrElseThrowDoesValueMatches(() => TestContext.BuildCommandContext<Guid>());
        [Fact]
        public void SucessfulResult_OrElseThrowOverloadDoesValueMatches() =>
            TestContext.SucessfulResult_OrElseThrowOverloadDoesValueMatches(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void FailedResult_FlatMapNotApplied() =>
            TestContext.FailedResult_FlatMapNotApplied(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void FailedResult_OrElseUsed() =>
            TestContext.FailedResult_OrElseUsed(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void FailedResult_OrElseGetUsedOtherInvoked() =>
            TestContext.FailedResult_OrElseGetUsedOtherInvoked(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void FailedResult_OrElseGetUsedValueMatches() =>
            TestContext.FailedResult_OrElseGetUsedValueMatches(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void FailedResult_OrElseThrowThrows() =>
            TestContext.FailedResult_OrElseThrowThrows(() => TestContext.BuildCommandContext<Guid>());
    }
}
