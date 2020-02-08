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

using Util.Results;
using System;
using Xunit;
using System.Security.Cryptography;

namespace Test.Util.Results
{
    public class CommandAndQueryResultTest
    {
        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultReportsSuccess()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = CommandAndQueryResult.Ok(value);

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        [Obsolete]
        public void SucessfulQueryContainsCorrectValueForValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = CommandAndQueryResult.Ok(value);

            // Assert
            Assert.Equal(value, uid.Value); 
        }

        [Fact]
        [Obsolete]
        public void SucessfulQueryContainsCorrectValueForReferenceType()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();

            // Act
            var @string = CommandAndQueryResult.Ok(value);

            // Assert
            Assert.Equal(value, @string.Value); 
        }
        
        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultHasEmptyReason()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            CommandAndQueryResult<Guid> uid = CommandAndQueryResult.Ok(value);

            // Assert
            Assert.Empty(uid.Reason);
        }

        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultHasNullCause()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            CommandAndQueryResult<Guid> uid = CommandAndQueryResult.Ok(value);

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultEqualsReturnsTrueForEqualResultsWithValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            CommandAndQueryResult<Guid> leftHandSide = CommandAndQueryResult.Ok(value);
            CommandAndQueryResult<Guid> rightHandSide = CommandAndQueryResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultEqualsReturnsTrueForEqualResultsWithSameReference()
        {
            // Arrange
            var generator = RNGCryptoServiceProvider.Create();
            byte[] data = new byte[sizeof(int)];
            generator.GetBytes(data);
            int randomNumber = BitConverter.ToInt32(data);

            CommandAndQueryResult<EquatableReferenceType> leftHandSide = CommandAndQueryResult.Ok(new EquatableReferenceType(randomNumber));
            CommandAndQueryResult<EquatableReferenceType> rightHandSide = CommandAndQueryResult.Ok(new EquatableReferenceType(randomNumber));

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }
        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultEqualsReturnsTrueForEqualResultsForEquatableReferenceType()
        {
            // Arrange
            Exception value = new Exception("ERROR");
            CommandAndQueryResult<Exception> leftHandSide = CommandAndQueryResult.Ok(value);
            CommandAndQueryResult<Exception> rightHandSide = CommandAndQueryResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        [Obsolete]
        public void SuccessfulCommandAndQueryResultImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            CommandAndQueryResult<Guid> result = CommandAndQueryResult.Ok<Guid>(Guid.NewGuid());

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
        [Obsolete]
        public void FailureCommandAndQueryResultByDefault()
        {
            // Arrange
            CommandAndQueryResult<Guid> result;

            // Act
            result = new CommandAndQueryResult<Guid>();

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        [Obsolete]
        public void FailedCommandAndQueryResultThrowsOnValueAccess()
        {
            // Arrange
            var failed = CommandAndQueryResult.Failed<Guid>(Guid.NewGuid().ToString());

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = failed.Value);
        }

        [Fact]
        [Obsolete]
        public void FailureQueryStoresExceptionCause()
        {
            // Arrange
            var exception = new Exception(Guid.NewGuid().ToString(), new Exception($"Inner {Guid.NewGuid()}"));
            var message = Guid.NewGuid().ToString();

            // Act
            var result = CommandAndQueryResult.Failed<Guid>(message, exception);

            // Assert
            Assert.Equal(exception, result.Cause);
            Assert.Equal(message, result.Reason);
        }

        [Fact]
        [Obsolete]
        public void FailureCommandAndQueryResultImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            CommandAndQueryResult<Guid> result = CommandAndQueryResult.Failed<Guid>(Guid.NewGuid().ToString());

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
