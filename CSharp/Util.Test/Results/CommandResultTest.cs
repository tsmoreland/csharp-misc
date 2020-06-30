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
using System.Collections.Generic;
using System.Security.Cryptography;
using Moreland.CSharp.Util.Internal;
using Moreland.CSharp.Util.Results;
using Xunit;

namespace Moreland.CSharp.Util.Test.Results
{
    public sealed class CommandResultTest
    {
        [Fact]
        public void Ok_SucccessReturnsTrue()
        {
            // Arrange
            var uid = CommandResult.Ok();

            // Act

            // Assert
            Assert.True(uid.Success);
        }

        [Fact]
        public void Ok_StoresMessageWhenProvided()
        {
            string message = Guid.NewGuid().ToString();
            // Arrange
            CommandResult uid = CommandResult.Ok(message);

            Assert.Equal(message, uid.Message);
        }

        [Fact]
        public void Ok_MessageEmptyWhenNotProvided()
        {
            // Arrange
            CommandResult uid = CommandResult.Ok();

            // Act

            // Assert
            Assert.Empty(uid.Message);
        }

        [Fact]
        public void Ok_ExceptionNull()
        {
            // Arrange
            CommandResult uid = CommandResult.Ok();

            // Act

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void Equals_OkResultsEqual()
        {
            // Arrange
            CommandResult leftHandSide = CommandResult.Ok();
            CommandResult rightHandSide = CommandResult.Ok();

            // Act
            bool equals = leftHandSide.Equals(rightHandSide);
            bool operatorEquals = leftHandSide == rightHandSide;
            bool operatorNotEquals = leftHandSide != rightHandSide;
            bool objEquals = leftHandSide.Equals((object)rightHandSide);
            bool objNotEquals = leftHandSide.Equals((object)new List<string>());

            // Assert
            Assert.True(equals);
            Assert.True(operatorEquals);
            Assert.False(operatorNotEquals);
            Assert.True(objEquals);
            Assert.False(objNotEquals);
        }

        [Fact]
        public void GetHashCode_MadeUpOfDeconstructedValue()
        {
            var result = CommandResult.Failed(Guid.NewGuid().ToString(), new InvalidCastException());
            var (success, message, cause) = result;

            Assert.Equal(HashProxy.Combine(success, message, cause), result.GetHashCode());
        }

        [Fact]
        public void GetHashCode_OfTMadeUpOfDeconstructedValue()
        {
            var result = CommandResult.Ok(Guid.NewGuid(), Guid.NewGuid().ToString());
            var (success, value, message, cause) = result;

            Assert.Equal(HashProxy.Combine(value, success, message, cause), result.GetHashCode());
        }

        [Fact]
        public void Ok_BooleanConversionIsTrue()
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
        public void DefaultConstructor_SuccessIsFalse()
        {
            // Arrange
            CommandResult result;

            // Act
            result = new CommandResult();

            // Assert
            Assert.False(result.Success);
            Assert.False(result);
            Assert.False(result.ToBoolean());
        }

        [Fact]
        public void Failed_ThrowsArgumentNullExceptionIfMessageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = CommandResult.Failed(null!));
        }

        [Fact]
        public void Failed_StoresProvidedException()
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
        public void Failed_SuccessIsFalse()
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
        public void OkWithResult_SuccessIsTrue()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = CommandResult.Ok(value);

            // Assert
            Assert.True(uid.Success);
            Assert.True(uid);
            Assert.True(uid.ToBoolean());
        }

        [Fact]
        public void Ok_StoresProvidedValueType()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            var uid = CommandResult.Ok(value);

            // Assert
            Assert.Equal(value, uid.Value); 
        }

        [Fact]
        public void Ok_StoresProvidedReferenceType()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();

            // Act
            var @string = CommandResult.Ok<string>(value);

            // Assert
            Assert.Equal(value, @string.Value); 
        }
        
        [Fact]
        public void Ok_EmptyMessageByDefault()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            CommandResult<Guid> uid = CommandResult.Ok(value);

            // Assert
            Assert.Empty(uid.Message);
        }

        [Fact]
        public void Ok_HasNullCause()
        {
            // Arrange
            Guid value = Guid.NewGuid();

            // Act
            CommandResult<Guid> uid = CommandResult.Ok(value);

            // Assert
            Assert.Null(uid.Cause);
        }

        [Fact]
        public void Equals_OkOfTEqualValueTypeIsTrue()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            CommandResult<Guid> leftHandSide = CommandResult.Ok(value);
            CommandResult<Guid> rightHandSide = CommandResult.Ok(value);

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
        public void Equals_OkWithEqualReferenceTypeIsTrue()
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
        public void Equals_OkWithEqualNonEquatableReferencTypeIsTrueWhenSameReference()
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
        public void Boolean_OkIsTrue()
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
        public void DefaultConstructorOfT_SuccessIsFalse()
        {
            // Arrange
            CommandResult<Guid> result;

            // Act
            result = new CommandResult<Guid>();

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Failed_ValueThrowsInvalidOperation()
        {
            // Arrange
            var failed = CommandResult.Failed<Guid>(Guid.NewGuid().ToString());

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = failed.Value);
        }

        [Fact]
        public void FailedOfT_ThrowsArgumentNullExceptionIfMessageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = CommandResult.Failed<Guid>(null!));
        }

        [Fact]
        public void FailedOfT_StoresProvidedException()
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
        public void FailedOfT_SuccessIsFalse()
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

        [Fact]
        public void FailedResult_OrElseThrowOverloadThrows() =>
            TestContext.FailedResult_OrElseThrowOverloadThrows(() => TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void FailedResult_OrElseThrowThrowsArgumentNullExceptionWhenSupplierIsNull() =>
            TestContext.FailedResult_OrElseThrowThrowsArgumentNullWhenSupplierIsNull(() => 
                TestContext.BuildCommandContext<Guid>());

        [Fact]
        public void ExplcitCast_ReturnsExpectedValueWhenSuccess()
        {
            var result = CommandResult.Ok<Guid>(Guid.NewGuid());

            var value = (Guid)result;

            Assert.Equal(result.Value, value);
        }
        [Fact]
        public void ExplcitCast_ThrowsInvalidOperationWhenFailed()
        {
            var result = CommandResult.Failed<Guid>(Guid.NewGuid().ToString());
            Assert.Throws<InvalidOperationException>(() => (Guid)result);
        }

        [Fact]
        public void DeconstructOfT_ValueTypeExportsProvidedValues()
        {
            Guid valueType = Guid.NewGuid();
            var valueResultSuccess = CommandResult.Ok(valueType);
            var valueResultFailure = CommandResult.Failed<Guid>(Guid.NewGuid().ToString(), new InvalidCastException());

            ActAndAssertDeconstruct(valueResultSuccess);
            ActAndAssertDeconstruct(valueResultFailure);

        }

        [Fact]
        public void DeconstructOfT_ReferenceTypeExportsProvidedValues()
        {
            object @object = new object();
            var refResultSuccess = CommandResult.Ok(@object);
            var refResultFailure = CommandResult.Failed<object>(Guid.NewGuid().ToString(), new NotSupportedException());

            ActAndAssertDeconstruct(refResultSuccess);
            ActAndAssertDeconstruct(refResultFailure);
        }

        [Fact]
        public void Deconstruct_ExportsProvidedValues()
        {
            var successResult = CommandResult.Ok();
            var failedResult = CommandResult.Failed(Guid.NewGuid().ToString(), new InvalidCastException());

            ActAndAssert(successResult);
            ActAndAssert(failedResult);

            static void ActAndAssert(CommandResult result)
            {
                var (success, message, cause) = result;
                Assert.Equal(result.Success, success);
                Assert.Equal(result.Message, message);
                Assert.Equal(result.Cause, cause);

                (success, message) = result;
                Assert.Equal(result.Success, success);
                Assert.Equal(result.Message, message);
            }
        }

        [Fact]
        public void UnknownError_IsFailure()
        {
            Assert.False(CommandResult.UnknownError.Success);
        }

        private static void ActAndAssertDeconstruct<T>(CommandResult<T> result)
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
