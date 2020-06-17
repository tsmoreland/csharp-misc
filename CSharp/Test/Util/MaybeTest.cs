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

using Moq;
using CSharp.Util;
using System;
using Xunit;

namespace CSharp.Test.Util
{
    public class MaybeTest
    {
        [Fact]
        public void IsPresentTrueWhenValueProvided()
        {
            // Arrange
            var maybe = Maybe.Of<Guid>(Guid.NewGuid());

            // Act
            bool isPresent = maybe.HasValue;

            // Assert
            Assert.True(isPresent);
        }

        [Fact]
        public void IsPresentTrueWherePredicateInvoked()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Of(Guid.NewGuid());

            // Act / Assert
            var result = TryApplyWhere(maybe, true);

            Assert.NotNull(result);
        }

        [Fact]
        public void IsPresentTrueWhereApplied()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Of(Guid.NewGuid());

            // Act
            var result = TryApplyWhere(maybe, true);

            // Assert
            Assert.True(result.HasValue);
        }

        [Fact]
        public void IsPresentTrueFlatMapInvoked()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            string mappedValue = value.ToString();
            var maybe = Maybe.Of(value);

            // Act / Assert
            var result = TryApplyFlatMap(maybe, mappedValue);

            Assert.NotNull(result);
        }

        [Fact]
        public void IsPresentTrueFlatMapAppliedResultHasValue()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            string mappedValue = value.ToString();
            var maybe = Maybe.Of(value);

            // Act
            var result = TryApplyFlatMap(maybe, mappedValue);

            // Assert
            Assert.True(result.HasValue);
        }
        [Fact]
        public void IsPresentTrueFlatMapAppliedResultValueMatchesExcpected()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            string mappedValue = value.ToString();
            var maybe = Maybe.Of(value);

            // Act
            var result = TryApplyFlatMap(maybe, mappedValue);

            // Assert
            Assert.Equal(mappedValue, result.Value);
        }
        [Fact]
        public void IsPresentTrueOrElseNotUsed()
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
        public void IsPresentTrueOrElseGetOtherNotInvoked()
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
        public void IsPresentTrueOrElseGetNotUsedValueMatches()
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
        public void IsPresentTrueOrElseThrowDoesNotThrow()
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
        public void IsPresentTrueOrElseThrowDoesIsPresent()
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
        public void IsPresentTrueOrElseThrowDoesValueMatches()
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
        public void IsPresentTrueImplicitBoolEqualsIsPresent()
        {
            // Arrange
            var maybe = Maybe.Of<Guid>(Guid.NewGuid());

            // Act / Assert
            IsPresentImplicitBoolEqualsIsPresent(maybe);
            Assert.True(maybe.HasValue);
        }

        [Fact]
        public void IsPresentTrueEqualsReturnsTrueForEqualValueTypeValues()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            var left = Maybe.Of(value);
            var right = Maybe.Of(value);

            // Act / Assert
            IsPresentEqualsReturnsTrueForEqualValue(left, right);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentTrueEqualsOpertorReturnsTrueForEqualValueTypeValues()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            var left = Maybe.Of(value);
            var right = Maybe.Of(value);

            // Act / Assert
            IsPresentEqualsOpertorReturnsTrueForEqualValue(left, right);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentTrueNotEqualsOpertorReturnsFalseForEqualValueTypeValues()
        {
            // Arrange
            Guid leftValue = Guid.NewGuid();
            Guid rightValue = Guid.NewGuid();
            var left = Maybe.Of(leftValue);
            var right = Maybe.Of(rightValue);

            // Act / Assert
            IsPresentNotEqualsOpertorReturnsFalseForEqualValue(left, right);
            Assert.NotEqual(leftValue, rightValue);
        }

        [Fact]
        public void IsPresentTrueEqualsReturnsTrueForEqualEquatableReferenceTypeValues()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            var left = Maybe.Of(value.ToString());
            var right = Maybe.Of(value.ToString());

            // Act / Assert
            IsPresentEqualsReturnsTrueForEqualValue(left, right);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentTrueEqualsOpertorReturnsTrueForEqualEquatableReferenceTypeValues()
        {
            // Arrange
            string value = Guid.NewGuid().ToString();
            var left = Maybe.Of(value.ToString());
            var right = Maybe.Of(value.ToString());

            // Act / Assert
            IsPresentEqualsOpertorReturnsTrueForEqualValue(left, right);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentTrueNotEqualsOpertorReturnsFalseForEqualEquatableReferenceTypeValues()
        {
            // Arrange
            string leftValue = Guid.NewGuid().ToString();
            string rightValue = Guid.NewGuid().ToString();
            var left = Maybe.Of(leftValue);
            var right = Maybe.Of(rightValue);

            // Act / Assert
            IsPresentNotEqualsOpertorReturnsFalseForEqualValue(left, right);
            Assert.NotEqual(leftValue, rightValue);
        }
        [Fact]
        public void IsPresentTrueEqualsReturnsTrueForEqualNonEquatableReferenceTypeValues()
        {
            // Arrange
            object @object = new object();
            var left = Maybe.Of(@object);
            var right = Maybe.Of(@object);

            // Act / Assert
            IsPresentEqualsReturnsTrueForEqualValue(left, right);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentTrueEqualsOpertorReturnsTrueForEqualNonEquatableReferenceTypeValues()
        {
            // Arrange
            object @object = new object();
            var left = Maybe.Of(@object);
            var right = Maybe.Of(@object);

            // Act / Assert
            IsPresentEqualsOpertorReturnsTrueForEqualValue(left, right);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentTrueNotEqualsOpertorReturnsFalseForEqualNonEquatableReferenceTypeValues()
        {
            // Arrange
            object leftValue = new object();
            object rightValue = new object();
            var left = Maybe.Of(leftValue);
            var right = Maybe.Of(rightValue);

            // Act / Assert
            IsPresentNotEqualsOpertorReturnsFalseForEqualValue(left, right);
            Assert.NotEqual(leftValue, rightValue);
        }

        [Fact]
        public void IsPresentFalseWhenEmpty()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act
            bool isPresent = maybe.HasValue;

            // Assert
            Assert.False(isPresent);
        }

        [Fact]
        public void IsPresentFalseFilterNotApplied()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Empty<Guid>();

            // Act
            var result = TryApplyWhere(maybe, false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public void IsPresentFalseFlatMapNotApplied()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            string mappedValue = value.ToString();
            var maybe = Maybe.Empty<Guid>();

            // Act
            var result = TryApplyFlatMap(maybe, mappedValue);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public void IsPresentFalseOrElseUsed()
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
        public void IsPresentFalseOrElseGetUsedOtherInvoked()
        {
            // Arrange
            var @else = Guid.NewGuid();
            var maybe = Maybe.Empty<Guid>();

            // Act / Assert
            _ = OrElseGet(maybe, @else);

            Assert.True(true, "assert is handled by or else get, this prevents warning");
        }
        [Fact]
        public void IsPresentFalseOrElseGetUsedValueMatches()
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
        public void IsPresentFalseOrElseThrowThrows()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();
            var ex = new Exception(Guid.NewGuid().ToString());

            // Act / Assert
            var (result, _) = OrElseThrow(maybe, ex);
            Assert.False(result.HasValue);
        }
        [Fact]
        public void IsPresentFalseOrElseThrowExceptionThrownProvidedBySupplier()
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
        public void IsPresentFalseImplicitBoolEqualsIsPresent()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act / Assert
            IsPresentImplicitBoolEqualsIsPresent(maybe);
            Assert.False(maybe);
        }

        [Fact]
        public void IsPresentFalseValueThrows()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => _ = maybe.Value);
        }

        [Fact]
        public void EmptyMaybeObjectsEqual()
        {
            // Arrange
            var empty_one = Maybe.Empty<Guid>();
            var empty_two = Maybe.Empty<Guid>();

            // Act
            bool equal = empty_one == empty_two;

            // Assert
            Assert.True(equal);
        }

        #region Private
        private void IsPresentImplicitBoolEqualsIsPresent<T>(Maybe<T> maybe)
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
        private Maybe<U> TryApplyFlatMap<T, U>(Maybe<T> maybe, U mappedValue)
        {
            // Arrange
            var flatMap = new Mock<Func<T, U>>();
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

        private void IsPresentEqualsReturnsTrueForEqualValue<T>(Maybe<T> left, Maybe<T> right)
        {
            // Arrange

            // Act
            bool equal = left.Equals(right);

            // Assert
            Assert.True(equal);
        }
        private void IsPresentEqualsOpertorReturnsTrueForEqualValue<T>(Maybe<T> left, Maybe<T> right)
        {
            // Arrange

            // Act
            bool equal =  left == right;

            // Assert
            Assert.True(equal);
        }
        private void IsPresentNotEqualsOpertorReturnsFalseForEqualValue<T>(Maybe<T> left, Maybe<T> right)
        {
            // Arrange

            // Act
            bool notEqual = left != right;

            // Assert
            Assert.True(notEqual);
        }
        #endregion
    }
}
