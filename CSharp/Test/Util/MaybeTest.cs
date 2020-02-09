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
using SystemEx.Util;
using System;
using Xunit;

namespace SystemEx.Test.Util
{
    public class MaybeTest
    {
        [Fact]
        public void IsPresentTrueWhenValueProvided()
        {
            // Arrange
            var maybe = Maybe.Of<Guid>(Guid.NewGuid());

            // Act
            bool isPresent = maybe.IsPresent;

            // Assert
            Assert.True(isPresent);
        }

        [Fact]
        public void IsPresentTrueFilterPredicateInvoked()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Of(Guid.NewGuid());

            // Act / Assert
            var _ = TryApplyFilter(maybe, true); 
        }

        [Fact]
        public void IsPresentTrueFilterApplied()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Of(Guid.NewGuid());

            // Act
            var result = TryApplyFilter(maybe, true);

            // Assert
            Assert.True(result.IsPresent);
        }

        [Fact]
        public void IsPresentTrueFlatMapInvoked()
        {
            // Arrange
            Guid value = Guid.NewGuid();
            string mappedValue = value.ToString();
            var maybe = Maybe.Of(value);

            // Act / Assert
            _ = TryApplyFlatMap(maybe, mappedValue);
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
            Assert.True(result.IsPresent);
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
            Guid result = maybe.OrElse(@else);

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
            Assert.True(result.IsPresent);
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
            Assert.True(result.IsPresent);
            Assert.Equal(value, result.Value);
        }
        [Fact]
        public void IsPresentTrueImplicitBoolEqualsIsPresent()
        {
            // Arrange
            var maybe = Maybe.Of<Guid>(Guid.NewGuid());

            // Act / Assert
            IsPresentImplicitBoolEqualsIsPresent(maybe);
            Assert.True(maybe.IsPresent);
        }

        [Fact]
        public void IsPresentFalseWhenEmpty()
        {
            // Arrange
            var maybe = Maybe.Empty<Guid>();

            // Act
            bool isPresent = maybe.IsPresent;

            // Assert
            Assert.False(isPresent);
        }

        [Fact]
        public void IsPresentFalseFilterNotApplied()
        {
            // Arrange
            Maybe<Guid> maybe = Maybe.Empty<Guid>();

            // Act
            var result = TryApplyFilter(maybe, false);

            // Assert
            Assert.False(result.IsPresent);
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
            Assert.False(result.IsPresent);
        }

        [Fact]
        public void IsPresentFalseOrElseUsed()
        {
            // Arrange
            Guid @else = Guid.NewGuid();
            var maybe = Maybe.Empty<Guid>();

            // Act
            Guid result = maybe.OrElse(@else);

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
            var _ = OrElseGet(maybe, @else);
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
            Assert.False(result.IsPresent);
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

        #region Private
        private void IsPresentImplicitBoolEqualsIsPresent<T>(Maybe<T> maybe)
        {
            // Arrange
            // handled by caller

            // Act
            bool @implicit = maybe;
            bool @explicit = maybe.ToBoolean();
            bool isPresent = maybe.IsPresent;

            // Assert
            Assert.Equal(@implicit, isPresent);
            Assert.Equal(@explicit, isPresent);
        }
        private Maybe<T> TryApplyFilter<T>(Maybe<T> maybe, bool @return)
        {
            // Arrange
            Mock<Predicate<T>> predicate = new Mock<Predicate<T>>();
            predicate.Setup(p => p.Invoke(It.IsAny<T>())).Returns(@return);

            // Act
            Maybe<T> maybeResult = maybe.Filter(predicate.Object);

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
            if (maybe.IsPresent)
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
            if (!maybe.IsPresent)
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
            if (!maybe.IsPresent)
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
