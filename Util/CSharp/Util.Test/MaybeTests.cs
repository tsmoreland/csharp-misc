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
using System.Linq;
using System.Threading.Tasks;
using Moreland.CSharp.Util.Collections;
using NSubstitute;
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class MaybeValueTypeTests : MaybeTests<Guid>
    {
        public MaybeValueTypeTests()
            : base(() => BuildRandomGuid())
        {
        }
    }

    [TestFixture]
    public sealed class MaybeEquatableRefTypeTests : MaybeTests<string>
    {
        public MaybeEquatableRefTypeTests()
            : base(() => BuildRandomString())
        {
        }
    }

    [TestFixture]
    public sealed class MaybeRefTypeTests : MaybeTests<List<string>>
    {
        public MaybeRefTypeTests()
            : base(() => BuildRandomListOfString())
        {
        }
    }

    public abstract class MaybeTests<T>
    {
        private readonly Func<T> _builder;
        private Maybe<T> _maybeWithValue;
        private Func<T, object> _selector = null!;
        private Func<T, Maybe<object>> _maybeSelector = null!;
        private Action<T> _action = null!;
        private Action _emptyAction = null!;
        private Func<T, Task> _actionAsync = null!;
        private Func<Task> _emptyActionAsync = null!;

        protected MaybeTests(Func<T> builder)
        {
            _builder = builder;
        }

        [SetUp]
        public void Setup()
        {
            _maybeWithValue = Maybe.Of(_builder());
            _selector = Substitute.For<Func<T, object>>();
            _maybeSelector = Substitute.For<Func<T, Maybe<object>>>();
            _action = Substitute.For<Action<T>>();
            _emptyAction = Substitute.For<Action>();
            _actionAsync = Substitute.For<Func<T, Task>>();
            _emptyActionAsync = Substitute.For<Func<Task>>();

            _actionAsync.Invoke(Arg.Any<T>()).Returns(Task.CompletedTask);
            _emptyActionAsync.Invoke().Returns(Task.CompletedTask);
        }


        [Test]
        public void Constructor_ReturnsEmpty_WhenInvoked()
        {
            var maybe = new Maybe<T>();

            Assert.That(maybe, Is.EqualTo(Maybe.Empty<T>()));
        }

        [Test]
        public void HasValue_ReturnsTrue_WhenNotEmpty()
        {
            bool hasValue = _maybeWithValue.HasValue;

            Assert.That(hasValue, Is.True);
        }

        [Test]
        public void HasValue_ReturnsFalse_WhenEmpty()
        {
            Assert.That(Maybe.Empty<T>().HasValue, Is.False);
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => _ = Maybe.Empty<T>().Value);
        }

        [Test]
        public void IsEmpty_ReturnsFalse_WhenNotEmpty()
        {
            bool hasValue = _maybeWithValue.IsEmpty;

            Assert.That(hasValue, Is.False);
        }

        [Test]
        public void IsEmpty_ReturnsTrue_WhenEmpty()
        {
            Assert.That(Maybe.Empty<T>().IsEmpty, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IfHasValue_ThrowsArgumentNullException_WhenActionIsNull(bool includeEmptyAction)
        {
            var ex = includeEmptyAction
                ? Assert.Throws<ArgumentNullException>(() => _maybeWithValue.IfHasValue(null!, _emptyAction))
                : Assert.Throws<ArgumentNullException>(() => _maybeWithValue.IfHasValue(null!));

            Assert.That(ex!.ParamName, Is.EqualTo("action"));
        }

        [Test]
        public void IfHasValue_ThrowsArgumentNullException_WhenEmptyActionIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _maybeWithValue.IfHasValue(_action, null!));
            Assert.That(ex!.ParamName, Is.EqualTo("emptyAction"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IfHasValue_InvokesAction_WhenNotEmpty(bool includeEmptyAction)
        {
            if (includeEmptyAction)
                _maybeWithValue.IfHasValue(_action, _emptyAction);
            else
                _maybeWithValue.IfHasValue(_action);

            _action.Received(1).Invoke(Arg.Any<T>());
        }

        [Test]
        public void IfHasValue_EmptyActionNotInvoked_WhenNotEmpty()
        {
            _maybeWithValue.IfHasValue(_action, _emptyAction);

            _emptyAction.Received(0).Invoke();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IfHasValueAsync_ThrowsArgumentNullException_WhenActionIsNull(bool includeEmptyAction)
        {
            var ex = includeEmptyAction
                ? Assert.ThrowsAsync<ArgumentNullException>(async () => await _maybeWithValue.IfHasValueAsync(null!, _emptyActionAsync))
                : Assert.ThrowsAsync<ArgumentNullException>(async () => await _maybeWithValue.IfHasValueAsync(null!));

            Assert.That(ex!.ParamName, Is.EqualTo("action"));
        }        

        [Test]
        public void IfHasValue_EmptyActionInvoked_WhenEmpty()
        {
            Maybe.Empty<T>().IfHasValue(_action, _emptyAction);

            _emptyAction.Received(1).Invoke();
        }

        [Test]
        public void IfHasValue_ActionNotInvoked_WhenEmpty()
        {
            Maybe.Empty<T>().IfHasValue(_action, _emptyAction);

            _action.Received(0).Invoke(Arg.Any<T>());
        }

        [Test]
        public void IfHasValueAsync_ThrowsArgumentNullException_WhenEmptyActionIsNull()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _maybeWithValue.IfHasValueAsync(_actionAsync, null!));
            Assert.That(ex!.ParamName, Is.EqualTo("emptyAction"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task IfHasValueAsync_InvokesAction_WhenNotEmpty(bool includeEmptyAction)
        {

            if (includeEmptyAction)
                await _maybeWithValue.IfHasValueAsync(_actionAsync, _emptyActionAsync);
            else
                await _maybeWithValue.IfHasValueAsync(_actionAsync);

            await _actionAsync.Received(1).Invoke(Arg.Any<T>());
        }

        [Test]
        public async Task IfHasValueAsync_EmptyActionNotInvoked_WhenNotEmpty()
        {
            _actionAsync.Invoke(Arg.Any<T>()).Returns(Task.CompletedTask);
            _emptyActionAsync.Invoke().Returns(Task.CompletedTask);

            await _maybeWithValue.IfHasValueAsync(_actionAsync, _emptyActionAsync);

            await _emptyActionAsync.Received(0).Invoke();
        }

        [Test]
        public async Task IfHasValueAsync_EmptyActionInvoked_WhenEmpty()
        {
            _actionAsync.Invoke(Arg.Any<T>()).Returns(Task.CompletedTask);
            _emptyActionAsync.Invoke().Returns(Task.CompletedTask);

            await Maybe.Empty<T>().IfHasValueAsync(_actionAsync, _emptyActionAsync);

            await _emptyActionAsync.Received(1).Invoke();
        }

        [Test]
        public async Task IfHasValueAsync_ActionNotInvoked_WhenEmpty()
        {
            _actionAsync.Invoke(Arg.Any<T>()).Returns(Task.CompletedTask);
            _emptyActionAsync.Invoke().Returns(Task.CompletedTask);

            await Maybe.Empty<T>().IfHasValueAsync(_actionAsync, _emptyActionAsync);

            await _actionAsync.Received(0).Invoke(Arg.Any<T>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Where_ReturnsEmpty_WhenIsEmpty(bool filterResult)
        {
            var predicate = Substitute.For<Predicate<T>>();
            predicate.Invoke(Arg.Any<T>()).Returns(filterResult);

            var maybe = Maybe.Empty<T>();

            var actual = maybe.Where(predicate);
            
            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void Where_ReturnsEmpty_WhenHasValueButPredicateReturnsFalse()
        {
            var predicate = Substitute.For<Predicate<T>>();
            predicate.Invoke(Arg.Any<T>()).Returns(false);

            var actual = _maybeWithValue.Where(predicate);

            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void Where_ReturnsWithValue_WhenHasValueAndPredicateReturnsTrue()
        {
            var predicate = Substitute.For<Predicate<T>>();
            predicate.Invoke(Arg.Any<T>()).Returns(true);

            var actual = _maybeWithValue.Where(predicate);

            Assert.That(actual.Value, Is.EqualTo(_maybeWithValue.Value));
        }

        [Test]
        public void Select_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _maybeWithValue.Select((Func<T, object>)null!));
            Assert.That(ex!.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void Select_ReturnsUpdatedMaybe_WhenHasValueAndSelectorIsNonNull()
        {
            var expected = new object();
            _selector.Invoke(Arg.Any<T>()).Returns(expected);

            var actual = _maybeWithValue.Select(_selector);

            Assert.That(actual.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Select_ReturnsEmpty_WhenDoesNotHasValue()
        {
            _selector.Invoke(Arg.Any<T>()).Returns(new object());

            var actual = Maybe.Empty<T>().Select(_selector);

            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void SelectMaybe_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _maybeWithValue.Select((Func<T, Maybe<object>>)null!));
            Assert.That(ex!.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void SelectMaybe_ReturnsUpdatedMaybe_WhenHasValueAndSelectorIsNonNull()
        {
            var expected = Maybe.Of(new object());
            _maybeSelector.Invoke(Arg.Any<T>()).Returns(expected);

            var actual = _maybeWithValue.Select(_maybeSelector);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SelectMaybe_ReturnsEmpty_WhenDoesNotHasValue()
        {
            _maybeSelector.Invoke(Arg.Any<T>()).Returns(Maybe.Of(new object()));

            var actual = Maybe.Empty<T>().Select(_maybeSelector);

            Assert.That(actual.IsEmpty, Is.True);
        }

        [Test]
        public void ValueOr_ReturnsValue_WhenHasValue()
        {
            var @else = _builder();

            var actual = _maybeWithValue.ValueOr(@else);

            Assert.That(actual, Is.EqualTo(_maybeWithValue.Value));
        }

        [Test]
        public void ValueOr_ReturnsElseValue_WhenIsEmpty()
        {
            var @else = _builder();

            var actual = Maybe.Empty<T>().ValueOr(@else);

            Assert.That(actual, Is.EqualTo(@else));
        }

        [Test]
        public void ValueOr_Supplier_ThrowsArgumentNullException_WhenSupplierIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _maybeWithValue.ValueOr(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("supplier"));
        }

        [Test]
        public void ValueOr_Supplier_ReturnsValue_WhenHasValue()
        {
            var supplier = Substitute.For<Func<T>>();

            var actual = _maybeWithValue.ValueOr(supplier);

            Assert.That(actual, Is.EqualTo(_maybeWithValue.Value));
        }

        [Test]
        public void ValueOr_Supplier_ReturnsSupplierValue_WhenIsEmpty()
        {
            var @else = _builder();
            var supplier = Substitute.For<Func<T>>();
            supplier.Invoke().Returns(@else);

            var actual = Maybe.Empty<T>().ValueOr(supplier);

            Assert.That(actual, Is.EqualTo(@else));
        }

        [Test]
        public void ValueOrThrow_ThrowsArgumentNullException_WhenExceptionSupplierIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _maybeWithValue.ValueOrThrow(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("exceptionSupplier"));
        }

        [Test]
        public void ValueOrThrow_ReturnsValue_WhenHasValue()
        {
            var exceptionSupplier = Substitute.For<Func<Exception>>();

            var actual = _maybeWithValue.ValueOrThrow(exceptionSupplier);

            Assert.That(actual, Is.EqualTo(_maybeWithValue.Value));
        }

        [Test]
        public void ValueOrThrows_ThrowsSuppliedException_WhenIsEmpty()
        {
            var expected = new NotImplementedException(BuildRandomString());
            var exceptionSupplier = Substitute.For<Func<Exception>>();
            exceptionSupplier.Invoke().Returns(expected);

            var actual = Assert.Catch<Exception>(() => _ = Maybe.Empty<T>().ValueOrThrow(exceptionSupplier));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AsEnumerable_ReturnsEnumerableContainingValue_WhenHasvalue()
        {
            var expected = FluentArray.Of(_maybeWithValue.Value);

            var actual = _maybeWithValue.AsEnumerable();

            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void AsEnumerable_ReturnsEmptyEnumerable_WhenIsEmpty()
        {
            var actual = Maybe.Empty<T>().AsEnumerable();

            Assert.That(actual.Any(), Is.False);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenHasValue()
        {
            var actual = _maybeWithValue.ToBoolean();

            Assert.That(actual, Is.True);
        }

        [Test]
        public void ToBoolean_ReturnsFalse_WhenIsEmpty()
        {
            var actual = Maybe.Empty<T>().ToBoolean();

            Assert.That(actual, Is.False);
        }

        [Test]
        public void OperatorEquals_ReturnsTrue_WhenValuesEqual()
        {
            var value = _builder();
            var first = Maybe.Of(value);
            var second = Maybe.Of(value);

            Assert.That(first == second, Is.True);
        }

        [Test]
        public void OperatorEquals_ReturnsFalse_WhenValuesNotEqual()
        {
            var firstValue = _builder();
            var first = Maybe.Of(firstValue);
            var second = Maybe.Of(GetValueNotEqualTo(_builder, firstValue));

            Assert.That(first == second, Is.False);
        }

        [Test]
        public void OperatorNotEquals_ReturnsFalse_WhenValuesEqual()
        {
            var value = _builder();
            var first = Maybe.Of(value);
            var second = Maybe.Of(value);

            Assert.That(first != second, Is.False);
        }

        [Test]
        public void OperatorNotEquals_ReturnsTrue_WhenValuesNotEqual()
        {
            var firstValue = _builder();
            var first = Maybe.Of(firstValue);
            var second = Maybe.Of(GetValueNotEqualTo(_builder, firstValue));

            Assert.That(first != second, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenHasValue()
        {
            bool hasValue = _maybeWithValue;
            Assert.That(hasValue, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenIsEmpty()
        {
            bool hasValue = Maybe.Empty<T>();
            Assert.That(hasValue, Is.False);
        }

        [Test]
        public void ExplicitCastToValueType_ReturnsValue_WHenHasValue()
        {
            var actual = (T)_maybeWithValue;
            Assert.That(actual, Is.EqualTo(_maybeWithValue.Value));
        }

        [Test]
        public void ExplicitCast_ThrowsInvalidOperationException_WhenIsEmpty() =>
            Assert.Throws<InvalidOperationException>(() => _ = (T)Maybe.Empty<T>());

        [Test]
        public void IEquatableEquals_ReturnsTrue_WhenValuesEqual()
        {
            var value = _builder();
            var first = Maybe.Of(value);
            var second = Maybe.Of(value);

            Assert.That(first.Equals(second), Is.True);
        }
        [Test]
        public void IEquatableEquals_ReturnsFalse_WhenValuesNotEqual()
        {
            var firstValue = _builder();
            var first = Maybe.Of(firstValue);
            var second = Maybe.Of(GetValueNotEqualTo(_builder, firstValue));

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void IEquatableEquals_ReturnsTrue_WhenBothAreEmpty()
        {
            var first = Maybe.Empty<T>();
            var second = Maybe.Empty<T>();

            Assert.That(first.Equals(second), Is.True);
        }

        [Test]
        public void IEquatableEquals_ReturnsFalse_WhenFirstMaybeIsEmpty()
        {
            var first = Maybe.Empty<T>();
            var second = _maybeWithValue;

            Assert.That(first.Equals(second), Is.False);
        }
        [Test]
        public void IEquatableEquals_ReturnsFalse_WhenSecondMaybeIsEmpty()
        {
            var first = _maybeWithValue;
            var second = Maybe.Empty<T>();

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void Equals_ReturnsTrue_WhenValuesEqual()
        {
            var value = _builder();
            var first = Maybe.Of(value);
            object second = Maybe.Of(value);

            Assert.That(first.Equals(second), Is.True);
        }
        [Test]
        public void Equals_ReturnsFalse_WhenValuesNotEqual()
        {
            var firstValue = _builder();
            var first = Maybe.Of(firstValue);
            object second = Maybe.Of(GetValueNotEqualTo(_builder, firstValue));

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void Equals_ReturnsFalse_WhenComparedValueIsWrongType()
        {
            var first = Maybe.Of(_builder());
            object second = new InaccessableClass();

            Assert.That(first.Equals(second), Is.False);
        }

        [Test]
        public void ToString_ReturnsValueAsString_WhenHasValue()
        {
            var acutal = _maybeWithValue.ToString();
            Assert.That(acutal, Is.EqualTo(_maybeWithValue.Value!.ToString()));
        }

        [Test]
        public void HashCode_ReturnsValueHashCode_WhenHasValue()
        {
            int expected = _maybeWithValue.Value!.GetHashCode();

            int actual = _maybeWithValue.GetHashCode();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void HashCode_ReturnsZero_WhenIsEmpty()
        {
            Assert.That(Maybe.Empty<T>().GetHashCode(), Is.EqualTo(0));
        }

        [Test]
        public void ToString_ReturnsNoSuchValueString_WhenIsEmpty()
        {
            var actual = Maybe.Empty<T>().ToString();

            Assert.That(actual, Is.EqualTo(Properties.Resources.NoSuchValue));
        }

        private class InaccessableClass
        {
            /// <summary>Arbitrary value so class isn't empty</summary>
            public Guid Id { get; } = Guid.NewGuid();

            /// <summary><inheritdoc cref="object.Equals(object?)"/></summary>
            public override bool Equals(object? obj) =>
                obj is InaccessableClass inaccessable && inaccessable.Id == Id;

            /// <summary><inheritdoc cref="object.GetHashCode"/></summary>
            public override int GetHashCode() => Id.GetHashCode();
        }
    }
}
