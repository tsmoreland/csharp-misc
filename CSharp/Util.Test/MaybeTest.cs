//
// Copyright © 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYright HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Test
{
    public class MaybeTest
    {
        [Fact]
        public void DefaultConstructorProducesEmpty()
        {
            // Arrange / Act
            var maybe = new Maybe<Guid>();
            Assert.Equal(Maybe.Empty<Guid>(), maybe);
        }

        [Fact]
        public void Of_HasValueTrueWhenHasValue()
        {
            // Arrange
            var maybe = Maybe.Of<Guid>(Guid.NewGuid());

            // Act
            bool isPresent = maybe.HasValue;

            // Assert
            Assert.True(isPresent);
        }

        [Fact]
        public void Where_InvokedWhenHasValue()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Of(Guid.NewGuid());

            // Act / Assert
            _ = TryApplyWhere(maybe, true);

            // satisfy need for a call to assert, real assert is in TryApplyWhere
            Assert.True(true);
            Assert.True(true);
        }

        [Fact]
        public void Where_ReturnsValueWhenConditionMet()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Of(Guid.NewGuid());

            // Act
            var result = TryApplyWhere(maybe, true);

            // Assert
            Assert.True(result.HasValue);
        }

        [Fact]
        public void Where_NotCalledWhenEmpty()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Empty<Guid>();

            // Act
            var result = TryApplyWhere(maybe, false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public void Map_InvokedWhenHasValue()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var maybe = Maybe.Of(value);

            // Act / Assert
            _ = TryApplyMap(maybe, value);

            // satisfy need for a call to assert, real assert is in TryApplyMap
            Assert.True(true);
        }

        [Fact]
        public void Map_ResultHasValueWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var maybe = Maybe.Of(value);

            // Act
            var result = TryApplyMap(maybe, value);

            // Assert
            Assert.True(result.HasValue);
        }
        [Fact]
        public void Map_ReturnsExpectedMappedValueWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var maybe = Maybe.Of(value);

            // Act
            var result = TryApplyMap(maybe, value);

            // Assert
            Assert.Equal(value, result.Value);
        }
        [Fact]
        public void Map_ThrowsArgumentNullExceptionWhenMapperIsNull()
        {
            var maybe = Maybe.Of(Guid.NewGuid());
            Assert.Throws<ArgumentNullException>(() => _ = maybe.Map<string>(null!));
        }

        [Fact]
        public void Map_ReturnsEmptyWhenSourceIsEmpty()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var maybe = Maybe.Empty<Guid>();

            // Act
            var result = TryApplyMap(maybe, value);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public void FlatMap_InvokedWhenHasValue()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var mappedValue = Maybe.Of(value);
            var maybe = Maybe.Of(value);

            // Act / Assert
            _ = TryApplyFlatMap(maybe, mappedValue);

            // satisfy need for a call to assert, real assert is in TryApplyFlatMap
            Assert.True(true);
        }

        [Fact]
        public void FlatMap_ResultHasValueWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var mappedValue = Maybe.Of(value);
            var maybe = Maybe.Of(value);

            // Act
            var result = TryApplyFlatMap(maybe, mappedValue);

            // Assert
            Assert.True(result.HasValue);
        }
        [Fact]
        public void FlatMap_ReturnsExpectedMappedValueWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var mappedValue = Maybe.Of(value);
            var maybe = Maybe.Of(value);

            // Act
            var result = TryApplyFlatMap(maybe, mappedValue);

            // Assert
            Assert.Equal(mappedValue, result);
        }
        [Fact]
        public void FlatMap_ThrowsArgumentNullExceptionWhenMapperIsNull()
        {
            var maybe = Maybe.Of(Guid.NewGuid());
            Assert.Throws<ArgumentNullException>(() => _ = maybe.FlatMap<string>(null!));
        }

        [Fact]
        public void FlatMap_ReturnsEmptyWhenSourceIsEmpty()
        {
            // Arrange
            var value = Guid.NewGuid().ToString();
            var maybe = Maybe.Empty<Guid>();

            // Act
            var result = TryApplyFlatMap(maybe, Maybe.Of(value));

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public void OrElseOther_ReturnsSourceValueWhenHasValue()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            Guid @else = Guid.NewGuid();
            var maybe = Maybe.Of(value);

            // Act
            Guid result = maybe.OrElseOther(@else);

            // Assert
            Assert.NotEqual(value, @else); // sanity check
            Assert.Equal(value, result);

        }
        [Fact]
        public void OrElseThrow_NoThrowWhenHasValue()
        {
            // Arrange
            var value = Guid.NewGuid();
            var maybe = Maybe.Of(value);
            var ex = new Exception(Guid.NewGuid().ToString());

            // Act / Assert
            _ = OrElseThrow(maybe, ex);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void OrElseThrow_ReturnsValueWhenSourceHasValue_VerifyTestMethod()
        {
            // Arrange
            var value = Guid.NewGuid();
            var maybe = Maybe.Of(value);
            var ex = new Exception(Guid.NewGuid().ToString());

            // Act
            var (result, _) = OrElseThrow(maybe, ex);

            // Assert
            Assert.True(result.HasValue);
        }
        [Fact]
        public void OrElseThrow_ReturnsValueWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid();
            var maybe = Maybe.Of(value);
            var ex = new Exception(Guid.NewGuid().ToString());

            // Act
            var (result, _) = OrElseThrow(maybe, ex);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void OrElseOther_EmptyReturnsOther()
        {
            // Arrange
            Guid @else = Guid.NewGuid();
            var maybe = Maybe.Empty<Guid>();

            // Act
            Guid result = maybe.OrElseOther(@else);

            // Assert
            Assert.Equal(@else, result);
        }

        [Fact]
        public void OrElseGet_ThrowsArgumentNullWhenGetterIsNull()
        {
            var maybe = Maybe.Of(Guid.NewGuid());
            Assert.Throws<ArgumentNullException>(() => _ = maybe.OrElseGet(null!));
        }
        [Fact]
        public void OrElseGet_NotInvokedWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid();
            var @else = Guid.NewGuid();
            var maybe = Maybe.Of(value);

            // Act / Assert
            _ = OrElseGet(maybe, @else);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }

        [Fact]
        public void OrElseGet_ReturnsSourceValueWhenSourceHasValue()
        {
            // Arrange
            var value = Guid.NewGuid();
            var @else = Guid.NewGuid();
            var maybe = Maybe.Of(value);

            // Act
            var result = OrElseGet(maybe, @else);

            // Assert
            Assert.NotEqual(value, @else);
            Assert.Equal(@value, result);
        }
        [Fact]
        public void OrElseGet_EmptyInvokesProvidedGetter()
        {
            // Arrange
            var @else = Guid.NewGuid();
            var maybe = Maybe.Empty<Guid>();

            // Act / Assert
            _ = OrElseGet(maybe, @else);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void OrElseGet_EmptyReturnsValueFromGetter()
        {
            // Arrange
            var @else = Guid.NewGuid();
            var maybe = Maybe.Empty<Guid>();

            // Act
            var result = OrElseGet(maybe, @else);

            // Assert
            Assert.Equal(@else, result);
        }

        [Fact]
        public void OrElseThrowFromSupplier_ThrowsArgumentNullWhenSupplierIsNull()
        {
            var maybe = Maybe.Of(Guid.NewGuid());
            Assert.Throws<ArgumentNullException>(() => _ = maybe.OrElseThrow(null!));
        }
        [Fact]
        public void OrElseThrowFromSupplier_HasValueIsFalse()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();
            var ex = new Exception(Guid.NewGuid().ToString());

            // Act / Assert
            var (result, _) = OrElseThrow(maybe, ex);
            Assert.False(result.HasValue);
        }
        [Fact]
        public void OrElseThrowFromSupplier_ExpectedExceptionThrown()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();
            var ex = new Exception(Guid.NewGuid().ToString());

            // Act
            var (_, thrown) = OrElseThrow(maybe, ex);

            // Assert
            Assert.Equal(ex, thrown);
        }

        [Fact]
        public void ImplicitBool_TrueIfHasValue()
        {
            // Arrange
            var maybe = Maybe.Of<Guid>(Guid.NewGuid());

            // Act / Assert
            ImplicitBoolEqualsIsPresent(maybe);
            Assert.True(maybe.HasValue);
        }
        [Fact]
        public void ImplicitBool_FalseWhenEmpty()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act / Assert
            ImplicitBoolEqualsIsPresent(maybe);
            Assert.False(maybe);
        }

        [Fact]
        public void Value_ThrowsInvalidOperationWhenEmpty()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = maybe.Value);
        }

        [Fact]
        public void Equals_BaseEqualsTrueWhenValueEqual()
        {
            Guid value = Guid.NewGuid();
            object @ref = new object();
            object? valueObject = Maybe.Of(value);
            var maybeValue = Maybe.Of(value);
            object? refObject = Maybe.Of(@ref);
            var maybeRef = Maybe.Of(@ref);

            bool valueEqual = maybeValue.Equals(valueObject);
            bool refEqual = maybeRef.Equals(refObject);

            Assert.True(valueEqual);
            Assert.True(refEqual);
        }
        [Fact]
        public void Equals_BaseEqualsFalseWhenValueNotEqual()
        {
            object? valueObject = Maybe.Of(Guid.NewGuid());
            var maybeValue = Maybe.Of(Guid.NewGuid());
            object? refObject = Maybe.Of(new object());
            var maybeRef = Maybe.Of(new object());

            bool valueEqual = maybeValue.Equals(valueObject);
            bool refEqual = maybeRef.Equals(refObject);

            Assert.False(valueEqual);
            Assert.False(refEqual);
        }

        [Fact]
        public void Empty_HasValueFalse()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act
            bool isPresent = maybe.HasValue;

            // Assert
            Assert.False(isPresent);
        }
        [Fact]
        public void Empty_MaybeObjectsEqual()
        {
            // Arrange
            var emptyOne = Maybe.Empty<Guid>();
            var emptyTwo = Maybe.Empty<Guid>();

            // Act
            bool equal = emptyOne == emptyTwo;

            // Assert
            Assert.True(equal);
        }

        [Fact]
        public void OfNullable_HasValueTrueWhenNonNullProvided()
        {
            // Arrange
            var maybeValueType = Maybe.OfNullable<Guid>(Guid.NewGuid());
            var maybeRefType = Maybe.OfNullable<string>(Guid.NewGuid().ToString());

            // Act
            bool valueIsPresent = maybeValueType.HasValue;
            bool refIsPresent = maybeRefType.HasValue;

            // Assert
            Assert.True(valueIsPresent);
            Assert.True(refIsPresent);
        }
        [Fact]
        public void OfNullable_HasExpectedValueWhenProvided()
        {
            Guid id = Guid.NewGuid();

            var maybeValue = Maybe.OfNullable<Guid>(id);
            var maybeRef = Maybe.OfNullable<string>(id.ToString());

            Assert.Equal(id, maybeValue.Value);
            Assert.Equal(id.ToString(), maybeRef.Value);
        }
        [Fact]
        public void OfNullable_HasValueFalseWhenNullProvided()
        {
            // Arrange
            var maybeValue = Maybe.OfNullable<Guid>(null!);
            var maybeRef = Maybe.OfNullable<string>(null!);

            // Act
            bool valueIsPresent = maybeValue.HasValue;
            bool refIsPresent = maybeRef.HasValue;

            // Assert
            Assert.False(valueIsPresent);
            Assert.False(refIsPresent);
        }
        [Fact]
        public void OfNullable_IsEmptyWhenNullProvided()
        {
            var maybeValue = Maybe.OfNullable<Guid>(null!);
            var maybeRef = Maybe.OfNullable<string>(null!);

            Assert.Equal(Maybe.Empty<Guid>(), maybeValue);
            Assert.Equal(Maybe.Empty<string>(), maybeRef);
        }

        [Fact]
        public void Equals_FalseWhenComparedToEmpty()
        {
            var maybeValue = Maybe.Of(Guid.NewGuid());
            var maybeRef = Maybe.Of(new List<string>());
            var emptyValue = Maybe.Empty<Guid>();
            var emptyRef = Maybe.Empty<List<string>>();

            bool valueEqual = maybeValue.Equals(emptyValue);
            bool refEqual = maybeRef.Equals(emptyRef);
            bool valueOperatorEqual = maybeValue == emptyValue;
            bool refOperatorEqual = maybeRef ==  emptyRef;
            bool valueOperatorNotEqual = maybeValue != emptyValue;
            bool refOperatorNotEqual = maybeRef !=  emptyRef;

            Assert.False(valueEqual);
            Assert.False(valueOperatorEqual);
            Assert.True(valueOperatorNotEqual);

            Assert.False(refEqual);
            Assert.False(refOperatorEqual);
            Assert.True(refOperatorNotEqual);
        }

        [Fact]
        public void Equals_FalseWhenEmptyComparedTo()
        {
            var maybeValue = Maybe.Of(Guid.NewGuid());
            var maybeRef = Maybe.Of(new List<string>());
            var emptyValue = Maybe.Empty<Guid>();
            var emptyRef = Maybe.Empty<List<string>>();

            bool valueEqual = emptyValue.Equals(maybeValue);
            bool refEqual = emptyRef.Equals(maybeRef);
            bool valueOperatorEqual = emptyValue == maybeValue;
            bool refOperatorEqual = emptyRef ==  maybeRef;
            bool valueOperatorNotEqual = emptyValue != maybeValue;
            bool refOperatorNotEqual = emptyRef !=  maybeRef;

            Assert.False(valueEqual);
            Assert.False(valueOperatorEqual);
            Assert.True(valueOperatorNotEqual);

            Assert.False(refEqual);
            Assert.False(refOperatorEqual);
            Assert.True(refOperatorNotEqual);
        }

        [Fact]
        public void Equals_FalseWhenTypeDoesNotMatch()
        {
            var maybeValue = Maybe.Of(Guid.NewGuid());
            var maybeRef = Maybe.Of(new List<string>());
            var @object = new object();

            Assert.False(maybeValue.Equals(@object));
            Assert.False(maybeRef.Equals(@object));
        }

        [Fact]
        public void Equals_TrueWhenValuesEqual()
        {
            // Arrange
            object @object = new object();
            Guid value = Guid.NewGuid();
            var leftValue = Maybe.Of(value);
            var rightValue = Maybe.Of(value);
            var leftRef = Maybe.Of(@object);
            var rightRef = Maybe.Of(@object);

            // Act 
            bool valueEqual = leftValue.Equals(rightValue);
            bool refEqual = leftRef.Equals(rightRef);

            Assert.True(valueEqual);
            Assert.True(refEqual);
        }
        [Fact]
        public void Equals_FalseWhenValuesDoNotEqual()
        {
            // Arrange
            var leftValue = Maybe.Of(Guid.NewGuid());
            var rightValue = Maybe.Of(Guid.NewGuid());
            var leftRef = Maybe.Of(new object());
            var rightRef = Maybe.Of(new object());

            // Act
            bool valueEqual = leftValue.Equals(rightValue);
            bool refEqual = leftRef.Equals(rightRef);

            Assert.False(valueEqual);
            Assert.False(refEqual);
        }
        [Fact]
        public void Equals_OperatorTrueWhenValuesEqual()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            var leftValue = Maybe.Of(value);
            var rightValue = Maybe.Of(value);
            var @object = new object();
            var leftRef = Maybe.Of(@object);
            var rightRef = Maybe.Of(@object);

            // Act 
            bool valueEqual = leftValue == rightValue;
            bool refEqual = leftRef == rightRef;
            bool valueNotEqual = leftValue != rightValue;
            bool refNotEqual = leftRef != rightRef;
            
            Assert.True(valueEqual);
            Assert.True(refEqual);
            Assert.False(valueNotEqual);
            Assert.False(refNotEqual);
        }
        [Fact]
        public void Equals_OperatorEqualsFalseWhenValuesNotEqual()
        {
            // Arrange
            var leftValue = Maybe.Of(Guid.NewGuid());
            var rightValue = Maybe.Of(Guid.NewGuid());
            var leftRef = Maybe.Of(new object());
            var rightRef = Maybe.Of(new object());

            // Act / Assert
            bool valueEqual = leftValue == rightValue;
            bool refEqual = leftRef == rightRef;
            bool valueNotEqual = leftValue != rightValue;
            bool refNotEqual = leftRef != rightRef;

            Assert.False(valueEqual);
            Assert.False(refEqual);
            Assert.True(valueNotEqual);
            Assert.True(refNotEqual);
        }

        [Fact]
        public void ToString_ReturnsValueStringWhenHasValue()
        {
            Guid value = Guid.NewGuid();
            object @ref = new object();
            var maybeValue = Maybe.Of(value);
            var maybeRef = Maybe.Of(@ref);

            string valueString = maybeValue.ToString();
            string refString = maybeRef.ToString();

            Assert.Equal(value.ToString(), valueString);
            Assert.Equal(@ref.ToString(), refString);
        }
        [Fact]
        public void ToString_ReturnsEmptyWhenHasNoValue()
        {
            var maybeValue = Maybe.Empty<Guid>();
            var maybeRef = Maybe.Empty<object>();

            Assert.Equal(ProjectResources.NoSuchValue, maybeValue.ToString());
            Assert.Equal(ProjectResources.NoSuchValue, maybeRef.ToString());
        }

        [Fact]
        public void GetHashCode_ReturnsValueHashCodeWhenHasValue()
        {
            Guid value = Guid.NewGuid();
            object @ref = new object();
            var maybeValue = Maybe.Of(value);
            var maybeRef = Maybe.Of(@ref);

            int valueHashCode = maybeValue.GetHashCode();
            int refHashCode = maybeRef.GetHashCode();

            Assert.Equal(value.GetHashCode(), valueHashCode);
            Assert.Equal(@ref.GetHashCode(), refHashCode);

        }
        [Fact]
        public void GetHashCode_ReturnsZeroWhenEmpty()
        {
            var maybeValue = Maybe.Empty<Guid>();
            var maybeRef = Maybe.Empty<object>();

            int valueHashCode = maybeValue.GetHashCode();
            int refHashCode = maybeRef.GetHashCode();

            Assert.Equal(0, valueHashCode);
            Assert.Equal(0, refHashCode);
        }
        [Fact]
        public void GetHashCode_ReturnsZeroWhenValueIsNull()
        {
            var maybeRef = Maybe.Of<object>(null!);

            int refHashCode = maybeRef.GetHashCode();

            Assert.Equal(0, refHashCode);
        }

        [Fact]
        public void ExplicitCast_ReturnsValueWhenHasValue()
        {
            Guid value = Guid.NewGuid();
            List<string> @ref = new List<string>();
            var maybeValue = Maybe.Of(value);
            var maybeRef = Maybe.Of(@ref);

            Guid actualValue = (Guid)maybeValue;
            object actualRef = (List<string>)maybeRef;

            Assert.Equal(value, actualValue);
            Assert.Equal(@ref, actualRef);
        }
        [Fact]
        public void ExplicitCast_ThrowsInvalidOperationWhenEmpty()
        {
            var maybeValue = Maybe.Empty<Guid>();
            var maybeRef = Maybe.Empty<List<string>>();

            Assert.Throws<InvalidOperationException>(() => _ = (Guid)maybeValue);
            Assert.Throws<InvalidOperationException>(() => _ = (List<string>)maybeRef);
        }

        #region Private
        private void ImplicitBoolEqualsIsPresent<T>(Maybe<T> maybe)
        {
            // Arrange
            // handled by caller

            // Act
            bool @implicit = maybe;
            bool @explicit = maybe.ToBoolean();
            bool isPresent = maybe.HasValue;

            // Assert
            Assert.Equal(@implicit, isPresent);
            Assert.Equal(@explicit, isPresent);
        }
        private Maybe<T> TryApplyWhere<T>(Maybe<T> maybe, bool @return)
        {
            // Arrange
            Mock<Predicate<T>> predicate = new Mock<Predicate<T>>();
            predicate.Setup(p => p.Invoke(It.IsAny<T>())).Returns(@return);

            // Act
            Maybe<T> maybeResult = maybe.Where(predicate.Object);

            // Assert 
            if (@return)
                predicate.Verify(p => p.Invoke(It.IsAny<T>()), Times.Once);
            else
                predicate.Verify(p => p.Invoke(It.IsAny<T>()), Times.Never);
            return maybeResult;
        }
        private Maybe<U> TryApplyMap<T, U>(Maybe<T> maybe, U mappedValue)
        {
            // Arrange
            var flatMap = new Mock<Func<T, U>>();
            flatMap
                .Setup(mapper => mapper.Invoke(It.IsAny<T>()))
                .Returns(mappedValue);

            // Act
            var mapped = maybe.Map(flatMap.Object);
            if (maybe.HasValue)
                flatMap.Verify(m => m.Invoke(It.IsAny<T>()), Times.Once);
            else
                flatMap.Verify(m => m.Invoke(It.IsAny<T>()), Times.Never);
            return mapped;
        }
        private Maybe<U> TryApplyFlatMap<T, U>(Maybe<T> maybe, Maybe<U> mappedValue)
        {
            // Arrange
            var flatMap = new Mock<Func<T, Maybe<U>>>();
            flatMap
                .Setup(mapper => mapper.Invoke(It.IsAny<T>()))
                .Returns(mappedValue);

            // Act
            var mapped = maybe.FlatMap(flatMap.Object);
            if (maybe.HasValue)
                flatMap.Verify(m => m.Invoke(It.IsAny<T>()), Times.Once);
            else
                flatMap.Verify(m => m.Invoke(It.IsAny<T>()), Times.Never);
            return mapped;
        }
        private T OrElseGet<T>(Maybe<T> maybe, T @else)
        {
            // Arrange
            var other = new Mock<Func<T>>();
            other
                .Setup(o => o.Invoke())
                .Returns(@else);

            // Act
            T result = maybe.OrElseGet(other.Object);

            // Assert
            if (!maybe.HasValue)
                other.Verify(o => o.Invoke(), Times.Once);
            else
                other.Verify(o => o.Invoke(), Times.Never);

            return result;
        }
        private (Maybe<T> result, Exception? thrown) OrElseThrow<T, TException>(Maybe<T> maybe, TException exceptionToThrow) where TException : Exception
        {
            // Arrange
            var supplier = new Mock<Func<Exception>>();
            supplier
                .Setup(s => s.Invoke())
                .Returns(exceptionToThrow);

            Maybe<T> result = Maybe.Empty<T>();
            Exception? thrown = null;

            // Act
            if (!maybe.HasValue)
            {
                thrown = Assert.Throws<TException>(() => maybe.OrElseThrow(supplier.Object));
                supplier.Verify(s => s.Invoke(), Times.Once);
            }
            else
            {
                result = Maybe.Of(maybe.OrElseThrow(supplier.Object));
                supplier.Verify(s => s.Invoke(), Times.Never);
            }

            return (result, thrown);
        }

        #endregion
    }
}
