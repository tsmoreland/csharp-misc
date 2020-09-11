using System;
using Moreland.CSharp.Util.Properties;
using Moreland.CSharp.Util.Results;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Results
{
    public abstract class ValueResultTests<T>
    {
        protected delegate IValueResult<T> OkDelegate(T value);
        protected delegate IValueResult<T> OkWithMessageDelegate(T value, string messsage);
        protected delegate IValueResult<T> FailedDelegate(string message);
        protected delegate IValueResult<T> FailedWithCauseDelegate(string messsage, Exception? cause);
        protected delegate bool GetImplicitBoolDelegate(IValueResult<T> result);
        protected delegate bool ObjectEqualsDelegate(IValueResult<T> first, IValueResult<T> second);
        protected delegate bool EquatableEqualsDelegate(IValueResult<T> first, IValueResult<T> second);
        protected delegate bool OperatorEqualsDelegate(IValueResult<T> first, IValueResult<T> second);
        protected delegate bool OperatorNotEqualsDelegate(IValueResult<T> first, IValueResult<T> second);

        private readonly Func<T> _builder;
        private readonly IValueResultTestHelper<T> _testHelper;
        private string _message = null!;
        private Exception _cause = null!;
        private T _value = default!;
        private IValueResult<T> _ok;
        private IValueResult<T> _okWithMessage;
        private IValueResult<T> _failed;
        private IValueResult<T> _failedWithCause;


        protected ValueResultTests(Func<T> builder, IValueResultTestHelper<T> testHelper)
        {
            _builder = builder;
            _testHelper = testHelper;
            _ok = _testHelper.FailedBuilder("invalid");
            _okWithMessage = _testHelper.FailedBuilder("invalid");
            _failed = _testHelper.FailedBuilder("invalid");
            _failedWithCause = _testHelper.FailedBuilder("invalid");
        }

        [SetUp]
        public void Setup()
        {
            _message = Guid.NewGuid().ToString("N");
            _cause = new Exception(_message);
            _value = _builder();

            _ok = _testHelper.OkBuilder(_value);
            _okWithMessage = _testHelper.OkWithMessageBuilder(_value, _message);
            _failed = _testHelper.FailedBuilder(_message);
            _failedWithCause = _testHelper.FailedWithCauseBuilder(_message, _cause);
        }

        [Test]
        public void Failed_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _testHelper.FailedBuilder(null!));
            Assert.That(ex.ParamName, Is.EqualTo("message"));
        }

        [Test]
        public void FailedWithCause_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _testHelper.FailedWithCauseBuilder(null!, _cause));
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
        public void Value_ReturnsGivenValue_WhenOk()
        {
            Assert.That(_ok.Value, Is.EqualTo(_value));
        }

        [Test]
        public void Value_ReturnsGivenValue_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Value, Is.EqualTo(_value));
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenFailed()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _ = _failed.Value);
            Assert.That(ex.Message, Is.EqualTo(Resources.InvalidResultValueAccess));
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenFailedWithCause()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _ = _failedWithCause.Value);
            Assert.That(ex.Message, Is.EqualTo(Resources.InvalidResultValueAccess));
        }

        [Test]
        public void Message_ReturnsMessage_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Message_ReturnsEmptyString_WhenOkWithMessageGivenNull()
        {
            Assert.That(_testHelper.OkBuilder(_value).Message, Is.EqualTo(""));
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
            bool @bool = _testHelper.ImplicitBool(_ok);
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenOkWithMessage()
        {
            bool @bool = _testHelper.ImplicitBool(_okWithMessage);
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailed()
        {
            bool @bool = _testHelper.ImplicitBool(_failed);
            Assert.That(@bool, Is.False);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailedWithCause()
        {
            bool @bool = _testHelper.ImplicitBool(_failedWithCause);
            Assert.That(@bool, Is.False);
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToSuccessAndValue_ReturnsProvidedValue_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (_, actualValue) = result;

            if (resultType == ResultType.Successful)
                Assert.That(actualValue.Value, Is.EqualTo(result.Value).And.EqualTo(expectedValue));
            else
                Assert.That(actualValue.IsEmpty, Is.True);
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToSuccessAndValue_ReturnsProvidedSuccess_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (success, _) = result;

            Assert.That(success, Is.EqualTo(result.Success));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToSuccessValueAndMessage_ReturnsProvidedValue_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (_, actualValue, _) = result;

            if (resultType == ResultType.Successful)
                Assert.That(actualValue.Value, Is.EqualTo(result.Value).And.EqualTo(expectedValue));
            else
                Assert.That(actualValue.IsEmpty, Is.True);
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToSuccessValueAndMessage_ReturnsProvidedSuccess_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (success, _, _) = result;

            Assert.That(success, Is.EqualTo(result.Success));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ToSuccessValueAndMessage_ReturnsProvidedMessage_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (_, _, message) = result;

            Assert.That(message, Is.EqualTo(result.Message));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ReturnsProvidedValue_WhenSuccessful(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (_, actualValue, _, _) = result;

            if (resultType == ResultType.Successful)
                Assert.That(actualValue.Value, Is.EqualTo(result.Value).And.EqualTo(expectedValue));
            else
                Assert.That(actualValue.IsEmpty, Is.True);
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ReturnsProvidedSuccess_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result = BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (success, _, _, _) = result;

            Assert.That(success, Is.EqualTo(result.Success));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ReturnsProvidedMessage_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result =
                BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (_, _, message, _) = result;

            Assert.That(message, Is.EqualTo(result.Message));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void Deconstruct_ReturnsProvidedCause_Always(ResultType resultType, bool includeMessage, bool includeException)
        {
            T expectedValue = _builder();
            IValueResult<T> result =
                BuildResultForDeconstruct(resultType, expectedValue, includeMessage, includeException);

            var (_, _, _, cause) = result;

            Assert.That(cause, Is.EqualTo(result.Cause));
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_failedWithCause, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool EquatableEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool EquatableEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool EquatableEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool EquatableEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_failedWithCause, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_failedWithCause, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorNotEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorNotEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorNotEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorNotEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildResultForDeconstruct(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_failedWithCause, result);
        }

        [Test]
        public void GetHashCode_ReturnsNonZero_WhenOk()
        {
            int hashCode = _ok.GetHashCode();
            Assert.That(hashCode, Is.Not.Zero);
        }

        [Test]
        public void GetHashCode_ReturnsNonZero_WhenOkWithMessage()
        {
            int hashCode = _okWithMessage.GetHashCode();
            Assert.That(hashCode, Is.Not.Zero);
        }

        [Test]
        public void GetHashCode_ReturnsSameValue_ForOkWithOrWithoutMessage()
        {
            int withMessage = _testHelper.OkBuilder(_value).GetHashCode();
            int withoutMessage = _testHelper.OkWithMessageBuilder(_value, _message).GetHashCode();

            Assert.That(withoutMessage, Is.EqualTo(withMessage).And.Not.Zero);
        }

        [Test]
        public void GetHashCode_ReturnsNonZero_WhenFailed()
        {
            int hashCode = _failed.GetHashCode();
            Assert.That(hashCode, Is.Not.Zero);
        }

        [Test]
        public void GetHashCode_ReturnsNonZero_WhenFailedWithCause()
        {
            int hashCode = _failedWithCause.GetHashCode();
            Assert.That(hashCode, Is.Not.Zero);
        }

        [Test]
        public void GetHashCode_ReturnsDifferentValue_ForFailedWithOrWithoutCause()
        {
            int withCause = _testHelper.FailedBuilder(_message).GetHashCode();
            int withoutCause = _testHelper.FailedWithCauseBuilder(_message, _cause).GetHashCode();

            Assert.That(withoutCause, Is.Not.EqualTo(withCause).And.Not.Zero);
        }

        private IValueResult<T> BuildResultForDeconstruct(ResultType resultType, T value, bool includeMessage, bool includeException) =>
            resultType switch
            {
                ResultType.Successful when !includeMessage => _testHelper.OkBuilder(value),
                ResultType.Successful when includeMessage => _testHelper.OkWithMessageBuilder(value, _message),
                ResultType.Failure when !includeException => _testHelper.FailedBuilder(_message),
                ResultType.Failure when includeException => _testHelper.FailedWithCauseBuilder(_message, _cause),
                ResultType.Null => null!,
                _ => throw new InvalidOperationException("Invalid test case"),
            };
    }
}
