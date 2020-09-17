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
using Moreland.CSharp.Util.Results;
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test.Results
{
    [TestFixture]
    public sealed class CommandResultTests
    {
        private string _message = null!;
        private Exception _cause = null!;
        private CommandResult _ok = CommandResult.Ok();
        private CommandResult _okWithMessage = CommandResult.Ok();
        private CommandResult _failed = CommandResult.Ok();
        private CommandResult _failedWithCause = CommandResult.Ok();

        [SetUp]
        public void Setup()
        {
            _message = Guid.NewGuid().ToString("N");
            _cause = new Exception(_message);

            _ok = CommandResult.Ok();
            _okWithMessage = CommandResult.Ok(_message);
            _failed = CommandResult.Failed(_message);
            _failedWithCause = CommandResult.Failed(_message, _cause);
        }

        [Test]
        public void Failed_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = CommandResult.Failed(null!));
            Assert.That(ex.ParamName, Is.EqualTo("message"));
        }

        [Test]
        public void FailedWithCause_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = CommandResult.Failed(null!, _cause));
            Assert.That(ex.ParamName, Is.EqualTo("message"));
        }

        [Test]
        public void Success_ReturnsTrue_WhenOk()
        {
            Assert.That(_ok.Success, Is.True);
        }

        [Test]
        public void Success_ReturnsTrue_WhenOkWithMesssage()
        {
            Assert.That(_okWithMessage.Success, Is.True);
        }

        [Test]
        public void Success_ReturnsFalse_WhenFailed()
        {
            Assert.That(_failed.Success, Is.False);
        }

        [Test]
        public void Success_ReturnsFalse_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Success, Is.False);
        }

        [Test]
        public void Success_ReturnsFalse_WhenUnknownError()
        {
            Assert.That(CommandResult.UnknownError.Success, Is.False);
        }

        [Test]
        public void Message_ReturnsEmpty_WhenOk()
        {
            Assert.That(_ok.Message, Is.Empty);
        }

        [Test]
        public void Message_ReturnsMessage_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Message_ReturnsEmptyString_WhenOkWithMessageGivenNull()
        {
            Assert.That(CommandResult.Ok().Message, Is.EqualTo(""));
        }

        [Test]
        public void Message_ReturnsMessage_WhenFailed()
        {
            Assert.That(_failed.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Message_ReturnsMessage_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Message_IsEmpty_WhenOkWithNullMessage()
        {
            Assert.That(CommandResult.Ok(null!).Message, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Cause_ReturnsNull_WhenOk()
        {
            Assert.That(_ok.Cause, Is.Null);
        }

        [Test]
        public void Cause_ReturnsNull_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Cause, Is.Null);
        }

        [Test]
        public void Cause_ReturnsNull_WhenFailed()
        {
            Assert.That(_failed.Cause, Is.Null);
        }

        [Test]
        public void Cause_ReturnsException_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Cause, Is.Not.Null);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenOk()
        {
            Assert.That(_ok.Success, Is.True);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Success, Is.True);
        }

        [Test]
        public void ToBoolean_ReturnsFalse_WhenFailed()
        {
            Assert.That(_failed.Success, Is.False);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Success, Is.False);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenOk()
        {
            bool @bool = _ok;
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenOkWithMessage()
        {
            bool @bool = _okWithMessage;
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailed()
        {
            bool @bool = _failed;
            Assert.That(@bool, Is.False);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailedWithCause()
        {
            bool @bool = _failedWithCause;
            Assert.That(@bool, Is.False);
        }

        [TestCase(ResultType.Successful, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, ExpectedResult = true)]
        [TestCase(ResultType.Null, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenSuccessAndCauseAreEqual(ResultType resultType, bool includeException)
        {
            object? result = resultType switch
            {
                ResultType.Successful => CommandResult.Ok(_message),
                ResultType.Failure when includeException => CommandResult.Failed(_message, _cause),
                ResultType.Failure when !includeException => CommandResult.Failed(_message),
                ResultType.Null => null,
                _ => throw new InvalidOperationException("Invalid test case"),
            };

            return _failedWithCause.Equals(result);
        }

        [TestCase(ResultType.Successful, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, ExpectedResult = true)]
        public bool IEquatableEquals_ReturnsTrue_WhenSuccessAndCauseAreEqual(ResultType resultType,
            bool includeException)
        {
            CommandResult result = resultType switch
            {
                ResultType.Successful => CommandResult.Ok(_message),
                ResultType.Failure when includeException => CommandResult.Failed(_message, _cause),
                ResultType.Failure when !includeException => CommandResult.Failed(_message),
                _ => throw new InvalidOperationException("Invalid test case"),
            };
            return _failedWithCause.Equals(result);
        }

        [TestCase(ResultType.Successful, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, ExpectedResult = true)]
        public bool OperatorEquals_ReturnsTrue_WhenSuccessAndCauseAreEqual(ResultType resultType,
            bool includeException)
        {
            CommandResult result = resultType switch
            {
                ResultType.Successful => CommandResult.Ok(_message),
                ResultType.Failure when includeException => CommandResult.Failed(_message, _cause),
                ResultType.Failure when !includeException => CommandResult.Failed(_message),
                _ => throw new InvalidOperationException("Invalid test case"),
            };
            return _failedWithCause == result;
        }

        [TestCase(ResultType.Successful, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, ExpectedResult = false)]
        public bool OperatorNotEquals_ReturnsTrue_WhenSuccessAndCauseAreEqual(ResultType resultType,
            bool includeException)
        {
            CommandResult result = resultType switch
            {
                ResultType.Successful => CommandResult.Ok(_message),
                ResultType.Failure when includeException => CommandResult.Failed(_message, _cause),
                ResultType.Failure when !includeException => CommandResult.Failed(_message),
                _ => throw new InvalidOperationException("Invalid test case"),
            };
            return _failedWithCause != result;
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, false, true, ExpectedResult = true)]
        public bool GetHashCode_ReturnsEqualCodeForEqualParameters_Always(ResultType resultType,
            bool includeMessage, bool includeException)
        {
            string customMessage = BuildRandomString(_message);
            CommandResult result = resultType switch
            {
                ResultType.Successful when !includeMessage => CommandResult.Ok(_message),
                ResultType.Successful when includeMessage => CommandResult.Ok(customMessage),
                ResultType.Failure when includeException && includeMessage => CommandResult.Failed(customMessage, _cause),
                ResultType.Failure when includeException && !includeMessage => CommandResult.Failed(_message, _cause),
                ResultType.Failure when !includeException && includeMessage => CommandResult.Failed(customMessage),
                ResultType.Failure when !includeException && !includeMessage => CommandResult.Failed(_message),
                _ => throw new InvalidOperationException("Invalid test case"),
            };

            return _failedWithCause.Equals(result);
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToMessagenAndSuccess_ReturnsProvidedMessage_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            CommandResult result = BuildResultForDeconstruct(resultType, includeMessage, includeException);

            var (_, message) = result;

            Assert.That(message, Is.EqualTo(result.Message));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToMessagenAndSuccess_ReturnsProvidedSuccess_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            CommandResult result = BuildResultForDeconstruct(resultType, includeMessage, includeException);

            var (success, _) = result;

            Assert.That(success, Is.EqualTo(result.Success));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToMessagenSuccessAndCause_ReturnsProvidedMessage_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            CommandResult result = BuildResultForDeconstruct(resultType, includeMessage, includeException);

            var (_, message, _) = result;

            Assert.That(message, Is.EqualTo(result.Message));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToMessagenSuccessAndCause_ReturnsProvidedSuccess_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            CommandResult result = BuildResultForDeconstruct(resultType, includeMessage, includeException);

            var (success, _, _) = result;

            Assert.That(success, Is.EqualTo(result.Success));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToMessagenSuccessAndCause_ReturnsProvidedCause_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            CommandResult result = BuildResultForDeconstruct(resultType, includeMessage, includeException);

            var (_, _, cause) = result;

            Assert.That(cause, Is.EqualTo(result.Cause));
        }

        [Test]
        public void GetHashCode_ReturnsDifferentValues_WhenComparingSuccessAndFailure()
        {
            var successCode = CommandResult.Ok(_message).GetHashCode();
            var failureCode = CommandResult.Failed(_message).GetHashCode();

            Assert.That(successCode, Is.Not.EqualTo(failureCode));
        }

        private CommandResult BuildResultForDeconstruct(ResultType resultType, bool includeMessage, bool includeException) =>
            resultType switch
            {
                ResultType.Successful when !includeMessage => CommandResult.Ok(),
                ResultType.Successful when includeMessage => CommandResult.Ok(_message),
                ResultType.Failure when !includeException => CommandResult.Failed(_message),
                ResultType.Failure when includeException => CommandResult.Failed(_message, _cause),
                _ => throw new InvalidOperationException("Invalid test case"),
            };

    }

    [TestFixture]
    public sealed class ValueTypeCommandResultTests : ValueResultTests<int>
    {
        public ValueTypeCommandResultTests()
            : base(
                () => BuildRandomInt32(),
                new CommandResultTestHelper<int>())
        {
        }

    }

    [TestFixture]
    public sealed class EquatableReferenceTypeCommandResultTests : ValueResultTests<string>
    {
        public EquatableReferenceTypeCommandResultTests()
            : base(
                () => BuildRandomString(),
                new CommandResultTestHelper<string>())
        {
        }
    }

    [TestFixture]
    public sealed class ReferenceTypeCommandResultTests : ValueResultTests<List<string>>
    {
        public ReferenceTypeCommandResultTests()
            : base(
                () => BuildRandomListOfString(),
                new CommandResultTestHelper<List<string>>())
        {
        }
    }

}
