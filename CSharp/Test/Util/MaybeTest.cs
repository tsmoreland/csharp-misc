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
        public void IsPresentTrueOrElseNotUsed()
        {
            // Arrange

            // Act

            // Assert
        }
        public void IsPresentTrueOrElseGetNotUsed()
        {
            // Arrange

            // Act

            // Assert
        }
        public void IsPresentTrueOrElseThrowDoesNotThrow()
        {
            // Arrange

            // Act

            // Assert
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

        public void IsPresentFalseFlatMapNotApplied()
        {
        }

        public void IsPresentFalseOrElseUsed()
        {
        }

        public void IsPresentFalseOrElseGetUsed()
        {
        }

        public void IsPresentFalseOrElseThrowThrows()
        {
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
            return mapped;
        }

        #endregion
    }
}
