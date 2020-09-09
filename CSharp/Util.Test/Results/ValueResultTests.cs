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

        private readonly Func<T> _builder;
        private readonly OkDelegate _okBuilder;
        private readonly OkWithMessageDelegate _okWithMessageBuilder;
        private readonly FailedDelegate _failedBuilder;
        private readonly FailedWithCauseDelegate _failedWithCauseBuilder;
        private readonly GetImplicitBoolDelegate _getImplicitBool;
        private string _message = null!;
        private Exception _cause = null!;
        private T _value = default!;
        private IValueResult<T> _ok;
        private IValueResult<T> _okWithMessage;
        private IValueResult<T> _failed;
        private IValueResult<T> _failedWithCause;

        protected ValueResultTests(
            Func<T> builder, 
            OkDelegate okBuilder, 
            OkWithMessageDelegate okWithMessageBuilder, 
            FailedDelegate failedBuilder,
            FailedWithCauseDelegate failedWithCauseBuilder,
            GetImplicitBoolDelegate getImplicitBool)
        {
            _builder = builder;
            _okBuilder = okBuilder;
            _okWithMessageBuilder = okWithMessageBuilder;
            _failedBuilder = failedBuilder;
            _failedWithCauseBuilder = failedWithCauseBuilder;
            _getImplicitBool = getImplicitBool;
            _ok = _failedBuilder("invalid");
            _okWithMessage = _failedBuilder("invalid");
            _failed = _failedBuilder("invalid");
            _failedWithCause = _failedBuilder("invalid");
        }

        [SetUp]
        public void Setup()
        {
            _message = Guid.NewGuid().ToString("N");
            _cause = new Exception(_message);
            _value = _builder();

            _ok = _okBuilder(_value);
            _okWithMessage = _okWithMessageBuilder(_value, _message);
            _failed = _failedBuilder(_message);
            _failedWithCause = _failedWithCauseBuilder(_message, _cause);
        }

        [Test]
        public void Failed_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _failedBuilder(null!));
            Assert.That(ex.ParamName, Is.EqualTo("message"));
        }

        [Test]
        public void FailedWithCause_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _failedWithCauseBuilder(null!, _cause));
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
            Assert.That(_okBuilder(_value).Message, Is.EqualTo(""));
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
            bool @bool = _getImplicitBool(_ok);
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenOkWithMessage()
        {
            bool @bool = _getImplicitBool(_okWithMessage);
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailed()
        {
            bool @bool = _getImplicitBool(_failed);
            Assert.That(@bool, Is.False);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailedWithCause()
        {
            bool @bool = _getImplicitBool(_failedWithCause);
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

        private IValueResult<T> BuildResultForDeconstruct(ResultType resultType, T value, bool includeMessage, bool includeException) =>
            resultType switch
            {
                ResultType.Successful when !includeMessage => _okBuilder(value),
                ResultType.Successful when includeMessage => _okWithMessageBuilder(value, _message),
                ResultType.Failure when !includeException => _failedBuilder(_message),
                ResultType.Failure when includeException => _failedWithCauseBuilder(_message, _cause),
                _ => throw new InvalidOperationException("Invalid test case"),
            };
    }
}
