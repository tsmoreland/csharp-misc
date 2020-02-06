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

using Maple.Util;
using System;
using Xunit;

namespace Maple.Tests.Util
{
    public class QueryResult
    {
        [Fact]
        public void SuccessfulQueryResultReportsSuccess()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult<Guid>.Ok(value);

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        public void SucessfulQueryContainsCorrectValueForValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult<Guid>.Ok(value);

            // Assert
            Assert.Equal(value, uid.Value); 
        }

        [Fact]
        public void SucessfulQueryContainsCorrectValueForReferenceType()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();

            // Act
            QueryResult<string> @string = QueryResult<string>.Ok(value);

            // Assert
            Assert.Equal(value, @string.Value); 
        }
        
        [Fact]
        public void SuccessfulQueryResultHasEmptyReason()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult<Guid>.Ok(value);

            // Assert
            Assert.Empty(uid.Reason);
        }

        [Fact]
        public void SuccessfulQueryResultHasNullCause()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult<Guid>.Ok(value);

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void SuccessfulQueryResultEqualsReturnsTrueForEqualResultsWithValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            QueryResult<Guid> leftHandSide = QueryResult<Guid>.Ok(value);
            QueryResult<Guid> rightHandSide = QueryResult<Guid>.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void SuccessfulQueryResultEqualsReturnsTrueForEqualResultsWithSameReference()
        {
            // Arrange
            Exception value = new Exception("ERROR");
            QueryResult<Exception> leftHandSide = QueryResult<Exception>.Ok(value);
            QueryResult<Exception> rightHandSide = QueryResult<Exception>.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);

            // Assert
            Assert.True(equals);
        }

        [Fact]
        public void FailedQueryResultThrowsOnValueAccess()
        {
            // Arrange
            var failed = QueryResult<Guid>.Failed(Guid.NewGuid().ToString());

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = failed.Value);
        }
    }
}
