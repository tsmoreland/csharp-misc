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

using Xunit;
using Moreland.CSharp.Util.Results;
using System;
using System.Security.Cryptography;
using Moreland.CSharp.Util.Internal;

namespace Moreland.CSharp.Util.Test.Results
{
    public class QueryResultTest
    {
        [Fact]
        public void SuccessfulResult_ReportsSuccess()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = QueryResult.Ok(value);

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        public void SuccessfulResult_ReturnsStoredMessage()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            var message = Guid.NewGuid().ToString();

            // Act
            var result = QueryResult.Ok(value, message);

            // Assert
            Assert.Equal(message, result.Message);
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
        public void SuccessfulResult_HasEmptyMessageIfNoneProvided()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult.Ok(value);

            // Assert
            Assert.Empty(uid.Message);
        }

        [Fact]
        public void SuccessfulResult_HasNullCause()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            QueryResult<Guid> uid = QueryResult.Ok(value);

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void SuccessfulResult_EqualsReturnsTrueForEqualResultsWithValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            QueryResult<Guid> leftHandSide = QueryResult.Ok(value);
            QueryResult<Guid> rightHandSide = QueryResult.Ok(value);

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);
            bool operatorEquals = leftHandSide == rightHandSide;
            bool operatorNotEquals = leftHandSide != rightHandSide;
            bool objEquals = leftHandSide.Equals((object)rightHandSide);
            bool nullObjEquals = leftHandSide.Equals((object)null!);

            // Assert
            Assert.True(equals);
            Assert.True(operatorEquals);
            Assert.False(operatorNotEquals);
            Assert.True(objEquals);
            Assert.False(nullObjEquals);
        }

        [Fact]
        public void SuccessfulResult_EqualsReturnsTrueForEqualResultsWithSameReference()
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
        public void SuccessfulResult_EqualsReturnsTrueForEqualResultsForEquatableReferenceType()
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
        public void SuccessfulResult_ImplicitBoolEqualsSuccessFailureState()
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
        public void FailureResult_ByDefault()
        {
            // Arrange
            QueryResult<Guid> result;

            // Act
            result = new QueryResult<Guid>();

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void FailedResultResultThrowsOnValueAccess()
        {
            // Arrange
            var failed = QueryResult.Failed<Guid>(Guid.NewGuid().ToString());

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = failed.Value);
        }

        [Fact]
        public void FailureQuery_StoresExceptionCause()
        {
            // Arrange
            var exception = new Exception(Guid.NewGuid().ToString(), new Exception($"Inner {Guid.NewGuid()}"));
            var message = Guid.NewGuid().ToString();

            // Act
            var result = QueryResult.Failed<Guid>(message, exception);

            // Assert
            Assert.Equal(exception, result.Cause);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void FailureResult_ImplicitBoolEqualsSuccessFailureState()
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

        [Fact]
        public void SucessfulResult_FlatMapInvoked() => 
            TestContext.SucessfulResult_FlatMapInvoked(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_FlatMapAppliedResultSuccess() =>
            TestContext.SucessfulResult_FlatMapAppliedResultSuccess(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_MapInvoked() => 
            TestContext.SucessfulResult_MapInvoked(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_MapAppliedResultSuccess() =>
            TestContext.SucessfulResult_MapAppliedResultSuccess(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseNotUsed() =>
            TestContext.SucessfulResult_OrElseNotUsed(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseGetOtherNotInvoked() =>
            TestContext.SucessfulResult_OrElseGetOtherNotInvoked(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseGetNotUsedValueMatches() =>
            TestContext.SucessfulResult_OrElseGetNotUsedValueMatches(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseThrowDoesNotThrow() =>
            TestContext.SucessfulResult_OrElseThrowDoesNotThrow(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseThrowValueMatches() =>
            TestContext.SucessfulResult_OrElseThrowDoesValueMatches(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void SucessfulResult_OrElseFlatMapReturnsInitialResult()
        {
            // Arrange
            var ctx = QueryResult.Ok(Guid.NewGuid());
            Guid alternate = Guid.NewGuid();

            // Act
            var result = ctx.OrElseFlatMap((msg, ex) => QueryResult.Ok(alternate));

            // Assert
            Assert.NotEqual(alternate, result.Value);
        }

        [Fact]
        public void FailedResult_OrElseThrowsArgumentNullWhenSupplierIsNull() =>
            TestContext.FailedResult_OrElseThrowThrowsArgumentNullWhenSupplierIsNull(() => TestContext.BuildQueryContext<Guid>());
        
        [Fact]
        public void FailedResult_FlatMapThrowsArgumentNullWhenMapperIsNull() =>
            TestContext.FailedResult_FlatMapThrowsArgumentNullWhenMapperIsNull(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseFlatMapReturnsMappedValue() 
        {
            // Arrange
            var ctx = QueryResult.Failed<Guid>("error", new NotImplementedException("not implemented"));
            Guid alternate = Guid.NewGuid();

            // Act
            var result = ctx.OrElseFlatMap((msg, ex) => QueryResult.Ok(alternate));

            // Assert
            Assert.Equal(alternate, result.Value);
        }

        [Fact]
        public void FailedResult_FlatMapNotApplied() =>
            TestContext.FailedResult_FlatMapNotApplied(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_MapThrowsArgumentNullWhenMaperIsNull() =>
            TestContext.FailedResult_MapThrowsArgumentNullWhenMapperIsNull(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_MapNotApplied() =>
            TestContext.FailedResult_MapNotApplied(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseUsed() =>
            TestContext.FailedResult_OrElseUsed(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseGetUsedOtherInvoked() =>
            TestContext.FailedResult_OrElseGetUsedOtherInvoked(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseGetUsedValueMatches() =>
            TestContext.FailedResult_OrElseGetUsedValueMatches(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseThrowThrows() =>
            TestContext.FailedResult_OrElseThrowThrows(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseThrowOverloadThrows() =>
            TestContext.FailedResult_OrElseThrowOverloadThrows(() => TestContext.BuildQueryContext<Guid>());

        [Fact]
        public void FailedResult_OrElseThrowThrowsArgumentNullExceptionWhenSupplierIsNull() =>
            TestContext.FailedResult_OrElseThrowThrowsArgumentNullWhenSupplierIsNull(() => 
                TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void GetHashCode_MadeUpOfDeconstructedValue()
        {
            var result = QueryResult.Ok<Guid>(Guid.NewGuid(), Guid.NewGuid().ToString());
            var (success, value, message, cause) = result;

            Assert.Equal(HashProxy.Combine(value, success, message, cause), result.GetHashCode());
        }

        [Fact]
        public void Deconstruct_ValueTypeExportsProvidedValues()
        {
            Guid valueType = Guid.NewGuid();
            var valueResultSuccess = QueryResult.Ok(valueType);
            var valueResultFailure = QueryResult.Failed<Guid>(Guid.NewGuid().ToString(), new InvalidCastException());

            ActAndAssertDeconstruct(valueResultSuccess);
            ActAndAssertDeconstruct(valueResultFailure);

        }

        [Fact]
        public void Deconstruct_ReferenceTypeExportsProvidedValues()
        {
            object @object = new object();
            var refResultSuccess = QueryResult.Ok(@object);
            var refResultFailure = QueryResult.Failed<object>(Guid.NewGuid().ToString(), new NotSupportedException());

            ActAndAssertDeconstruct(refResultSuccess);
            ActAndAssertDeconstruct(refResultFailure);
        }

        private static void ActAndAssertDeconstruct<T>(QueryResult<T> result)
        {
            if (result)
            {
                var (success, value, message, cause) = result;
                Assert.Equal(result.Success, success);
                Assert.Equal(result.Value, value);
                Assert.Equal(result.Message, message);
                Assert.Equal(result.Cause, cause);

                (success, value, message) = result;
                Assert.Equal(result.Success, success);
                Assert.Equal(result.Value, value);
                Assert.Equal(result.Message, message);

                (success, value) = result;
                Assert.Equal(result.Success, success);
                Assert.Equal(result.Value, value);
            }
            else
                Assert.Throws<InvalidOperationException>(() => (_, _, _, _) = result);
        }
    }
}
