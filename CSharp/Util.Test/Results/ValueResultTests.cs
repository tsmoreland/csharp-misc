using System;
using System.Collections.Generic;
using System.Linq;
using Moreland.CSharp.Util.Properties;
using Moreland.CSharp.Util.Results;
using NSubstitute;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Results
{
    public abstract class ValueResultTests<T>
    {
        private readonly Func<T> _builder;
        private readonly IValueResultTestHelper<T> _testHelper;
        private string _message = null!;
        private Exception _cause = null!;
        private T _value = default!;
        private T _elseValue = default!;
        private IValueResult<T> _ok;
        private IValueResult<T> _okWithMessage;
        private IValueResult<T> _failed;
        private IValueResult<T> _failedWithCause;
        private readonly DateTime _selectedValue;
        private Func<T, DateTime> _selector = null!;
        private readonly Func<T, IValueResult<DateTime>> _resultSelector;

        protected ValueResultTests(Func<T> builder, IValueResultTestHelper<T> testHelper)
        {
            _selectedValue = DateTime.Now;
            _resultSelector = Substitute.For<Func<T, IValueResult<DateTime>>>();

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
            _elseValue = _builder();
            while (_elseValue!.Equals(_value))
                _elseValue = _builder();

            _selector = Substitute.For<Func<T, DateTime>>();
            _selector.Invoke(_value).Returns(_selectedValue);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
            IValueResult<T> result = BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
                BuildValueResult(resultType, expectedValue, includeMessage, includeException);

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
                BuildValueResult(resultType, expectedValue, includeMessage, includeException);

            var (_, _, _, cause) = result;

            Assert.That(cause, Is.EqualTo(result.Cause));
        }

        public static IEnumerable<TestCaseData> SelectThrowsArgumentNullTestData()
        {
            foreach (var selectorType in Enum.GetValues(typeof(SelectorType)).OfType<SelectorType>())
            {
                yield return new TestCaseData(ResultType.Successful, false, false, selectorType);
                yield return new TestCaseData(ResultType.Successful, true, false, selectorType);
                yield return new TestCaseData(ResultType.Failure, true, false, selectorType);
                yield return new TestCaseData(ResultType.Failure, true, true, selectorType);
            }
        }
    
        [Test, TestCaseSource(nameof(SelectThrowsArgumentNullTestData))]
        public void Select_ThrowsArgumentNullException_WhenSelectorIsNull(ResultType resultType, bool includeMessage, bool includeException, SelectorType selectorType)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);

            var ex = selectorType == SelectorType.DirectValue
                ? Assert.Throws<ArgumentNullException>(() =>
                    _ = _testHelper.Select(result, (Func<T, List<int>>)null!))
                : Assert.Throws<ArgumentNullException>(() =>
                    _ = _testHelper.Select(result, (Func<T, IValueResult<List<int>>>)null!));

            Assert.That(ex.ParamName, Is.EqualTo("selector"));
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Ok)]
        [TestCase(SelectorType.OkWithMessage)]
        public void Select_ReturnsSuccess_WhenOk(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_ok, _selector) 
                : _testHelper.Select(_ok, _resultSelector);
            
            Assert.That(actual.Success, Is.True);
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Ok)]
        [TestCase(SelectorType.OkWithMessage)]
        public void Select_ReturnsResultOfSelector_WhenOk(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_ok, _selector) 
                : _testHelper.Select(_ok, _resultSelector);

            Assert.That(actual.Value, Is.EqualTo(_selectedValue));
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Ok)]
        [TestCase(SelectorType.OkWithMessage)]
        public void Select_ReturnsSuccess_WhenOkWithMessage(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_okWithMessage, _selector) 
                : _testHelper.Select(_okWithMessage, _resultSelector);

            Assert.That(actual.Success, Is.True);
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Ok)]
        [TestCase(SelectorType.OkWithMessage)]
        public void Select_ReturnsResultOfSelector_WhenOkWithMessage(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_okWithMessage, _selector) 
                : _testHelper.Select(_okWithMessage, _resultSelector);

            Assert.That(actual.Value, Is.EqualTo(_selectedValue));
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Failed)]
        [TestCase(SelectorType.FailedWithCause)]
        public void Select_ReturnsSuccessFalse_WhenFailed(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_failed, _selector) 
                : _testHelper.Select(_failed, _resultSelector);
            Assert.That(actual.Success, Is.False);
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Failed)]
        [TestCase(SelectorType.FailedWithCause)]
        public void Select_ReturnsNullCause_WhenFailed(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_failed, _selector) 
                : _testHelper.Select(_failed, _resultSelector);
            Assert.That(actual.Cause, Is.Null);
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Failed)]
        [TestCase(SelectorType.FailedWithCause)]
        public void Select_ReturnsSuccessFalse_WhenFailedWithCause(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_failedWithCause, _selector) 
                : _testHelper.Select(_failedWithCause, _resultSelector);
            Assert.That(_testHelper.ObjectEquals(_failedWithCause, actual), Is.False);
        }

        [TestCase(SelectorType.DirectValue)]
        [TestCase(SelectorType.Failed)]
        [TestCase(SelectorType.FailedWithCause)]
        public void Select_ReturnsNonNullCause_WhenFailedWithCause(SelectorType type)
        {
            ArrangeSelector(type);
            var actual = type == SelectorType.DirectValue 
                ? _testHelper.Select(_failedWithCause, _selector) 
                : _testHelper.Select(_failedWithCause, _resultSelector);
            Assert.That(actual.Cause, Is.Not.Null);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ValueOr_ReturnsValue_WhenOk(bool useSupplierOverride)
        {
            var actual = useSupplierOverride 
                ? _ok.ValueOr(() => _elseValue) 
                : _ok.ValueOr(_elseValue);
            Assert.That(actual, Is.EqualTo(_value));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ValueOr_ReturnsValue_WhenOkWithMessage(bool useSupplierOverride)
        {
            var actual = useSupplierOverride 
                ? _okWithMessage.ValueOr(() => _elseValue) 
                : _okWithMessage.ValueOr(_elseValue);
            Assert.That(actual, Is.EqualTo(_value));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ValueOr_ReturnsOrValue_WhenFailed(bool useSupplierOverride)
        {
            var actual = useSupplierOverride 
                ? _failed.ValueOr(() => _elseValue) 
                : _failed.ValueOr(_elseValue);
            Assert.That(actual, Is.EqualTo(_elseValue));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ValueOr_ReturnsOrValue_WhenFailedWithCause(bool useSupplierOverride)
        {
            var actual = useSupplierOverride 
                ? _failedWithCause.ValueOr(() => _elseValue) 
                : _failedWithCause.ValueOr(_elseValue);
            Assert.That(actual, Is.EqualTo(_elseValue));
        }

        [TestCase(ResultType.Successful, false, false)]
        [TestCase(ResultType.Successful, true, false)]
        [TestCase(ResultType.Failure, true, false)]
        [TestCase(ResultType.Failure, true, true)]
        public void ValueOr_ThrowsArgumentNullException_WhenSupplierIsNull(ResultType resultType, bool includeMessage, bool includeException)
        {
            var result = BuildValueResult(resultType, _value, includeMessage, includeException);
            var ex = Assert.Throws<ArgumentNullException>(() => _ = result.ValueOr((Func<T>)null!));
            Assert.That(ex.ParamName, Is.EqualTo("supplier"));
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        [TestCase(ResultType.Null, false, false, ExpectedResult = false)]
        public bool ObjectEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            object? result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.ObjectEquals(_failedWithCause, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool EquatableEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool EquatableEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool EquatableEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool EquatableEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.EquatableEquals(_failedWithCause, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorEquals(_failedWithCause, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorNotEquals_ReturnsTrue_WhenOk(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_ok, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = false)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorNotEquals_ReturnsTrue_WhenOkWithMessage(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_okWithMessage, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = false)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = true)]
        public bool OperatorNotEquals_ReturnsTrue_WhenFailed(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_failed, result);
        }

        [TestCase(ResultType.Successful, false, false, ExpectedResult = true)]
        [TestCase(ResultType.Successful, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, false, ExpectedResult = true)]
        [TestCase(ResultType.Failure, true, true, ExpectedResult = false)]
        public bool OperatorNotEquals_ReturnsTrue_WhenFailedWithCause(ResultType resultType, bool includeMessage, bool includeException)
        {
            IValueResult<T> result = BuildValueResult(resultType, _value, includeMessage, includeException);
            return _testHelper.OperatorNotEquals(_failedWithCause, result);
        }

        [Test]
        public void ExplicitOperator_ReturnsValue_WhenOk()
        {
            var actual = _testHelper.ExplicitValue(_ok);
            Assert.That(actual, Is.EqualTo(_value));
        }
        [Test]
        public void ExplicitOperator_ReturnsValue_WhenOkWithMessage()
        {
            var actual = _testHelper.ExplicitValue(_okWithMessage);
            Assert.That(actual, Is.EqualTo(_value));
        }

        [Test]
        public void ExplicitOperator_ThrowsInvalidOperationException_WhenFailed()
        {
            Assert.Throws<InvalidOperationException>(() => _testHelper.ExplicitValue(_failed));
        }
        [Test]
        public void ExplicitOperator_ThrowsInvalidOperationException_WhenFailedWithCause()
        {
            Assert.Throws<InvalidOperationException>(() => _testHelper.ExplicitValue(_failedWithCause));
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

        private void ArrangeSelector(SelectorType type)
        {
            switch (type)
            {
                case SelectorType.DirectValue:
                    break;
                case SelectorType.Ok:
                    _resultSelector.Invoke(_value)
                        .Returns(_testHelper.OkBuilder(_selectedValue));
                    break;
                case SelectorType.OkWithMessage:
                    _resultSelector.Invoke(_value)
                        .Returns(_testHelper.OkWithMessageBuilder(_selectedValue, _message + "2"));
                    break;
                case SelectorType.Failed:
                    _resultSelector.Invoke(_value)
                        .Returns(_testHelper
                            .FailedBuilder<DateTime>(_message + "2"));
                    break;
                case SelectorType.FailedWithCause:
                    _resultSelector.Invoke(_value)
                        .Returns(_testHelper
                            .FailedWithCauseBuilder<DateTime>(_message + "2", new Exception(_message + "2")));
                    break;
                default:
                    throw new InvalidOperationException("unexpected value");
            }
        }

        private IValueResult<T> BuildValueResult(ResultType resultType, T value, bool includeMessage, bool includeException) =>
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
