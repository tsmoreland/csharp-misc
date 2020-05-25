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

using System.Util.Extensions;
using Xunit;

namespace System.Test.Util.Extensions
{
    public sealed class NullableTests
    {
        [Fact]
        public void ClassHasValueIsTrueWhenNonNull()
        {
            object value = new object();
            Assert.True(value.HasValue());
        }

        [Fact]
        public void ClassHasValueIsFalseWhenNull()
        {
            object value = null!;
            Assert.False(value.HasValue());
        }

        [Fact]
        public void ClassValueOrReturnsOrWhenValueIsNull()
        {
            // Arrange
            object value = null!;
            object or = new object();

            // Act
            var result = value.OrElse(or);

            // Assert
            Assert.Same(or, result);
        }

        [Fact]
        public void ClassValueOrReturnsValueWhenNonNull()
        {
            // Arrange
            object value = new object();
            object or = new object();

            // Act
            var result = value.OrElse(or);

            // Assert
            Assert.Same(value, result);
        }

        [Fact]
        public void ValueTypeHasValueIsTrueWhenNonNull()
        {
            Guid? value = Guid.NewGuid();
            Assert.True(value.HasValue());
        }

        [Fact]
        public void ValueTypeHasValueIsFalseWhenNull()
        {
            Guid? value = null;
            Assert.False(value.HasValue());
        }

        [Fact]
        public void ValueTypeValueOrReturnsOrWhenValueIsNull()
        {
            // Arrange
            Guid? value = null;
            Guid or = Guid.NewGuid();

            // Act
            var result = value.OrElse(or);

            // Assert
            Assert.Equal(or, result);
        }

        [Fact]
        public void ValueTypeValueOrReturnsValueWhenNonNull()
        {
            // Arrange
            Guid? value = Guid.NewGuid();
            Guid or = Guid.NewGuid();

            // Act
            var result = value.OrElse(or);

            // Assert
            Assert.Equal(value, result);
        }
    }
}
