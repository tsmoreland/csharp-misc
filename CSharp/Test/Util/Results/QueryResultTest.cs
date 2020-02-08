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
using Xunit;
using Util.Results;
using System.Security.Cryptography;

namespace Test.Util.Results
{
    public class QueryResultTest
    {
        [Fact]
        public void SuccessfulQueryResultReportsSuccess()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = QueryResult.Ok(value);

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        public void SucessfulQueryContainsCorrectValueForValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = QueryResult.Ok(value);

            // Assert
            Assert.Equal(value, uid.Value); 
        }

        [Fact]
        public void SucessfulQueryContainsCorrectValueForReferenceType()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();

            // Act
            var @string = QueryResult.Ok(value);

            // Assert
            Assert.Equal(value, @string.Value); 
        }
        
        [Fact]
        public void SuccessfulQueryResultHasEmptyReason()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult.Ok(value);

            // Assert
            Assert.Empty(uid.Reason);
        }

        [Fact]
        public void SuccessfulQueryResultHasNullCause()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult.Ok(value);

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void SuccessfulQueryResultEqualsReturnsTrueForEqualResultsWithValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            QueryResult<Guid> leftHandSide = QueryResult.Ok(value);
            QueryResult<Guid> rightHandSide = QueryResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void SuccessfulQueryResultEqualsReturnsTrueForEqualResultsWithSameReference()
        {
            // Arrange
            var generator = RNGCryptoServiceProvider.Create();
            byte[] data = new byte[sizeof(int)];
            generator.GetBytes(data);
            int randomNumber = BitConverter.ToInt32(data);

            QueryResult<EquatableReferenceType> leftHandSide = QueryResult.Ok(new EquatableReferenceType(randomNumber));
            QueryResult<EquatableReferenceType> rightHandSide = QueryResult.Ok(new EquatableReferenceType(randomNumber));

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }
        [Fact]
        public void SuccessfulQueryResultEqualsReturnsTrueForEqualResultsForEquatableReferenceType()
        {
            // Arrange
            Exception value = new Exception("ERROR");
            QueryResult<Exception> leftHandSide = QueryResult.Ok(value);
            QueryResult<Exception> rightHandSide = QueryResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void SuccessfulQueryResultImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            QueryResult<Guid> result = QueryResult.Ok<Guid>(Guid.NewGuid());

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
        public void FailureQueryResultByDefault()
        {
            // Arrange
            QueryResult<Guid> result;

            // Act
            result = new QueryResult<Guid>();

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void FailedQueryResultThrowsOnValueAccess()
        {
            // Arrange
            var failed = QueryResult.Failed<Guid>(Guid.NewGuid().ToString());

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
            var result = QueryResult.Failed<Guid>(message, exception);

            // Assert
            Assert.Equal(exception, result.Cause);
            Assert.Equal(message, result.Reason);
        }

        [Fact]
        public void FailureQueryResultImplicitBoolEqualsSuccessFailureState()
        {
            // Arrange
            QueryResult<Guid> result = QueryResult.Failed<Guid>(Guid.NewGuid().ToString());

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
