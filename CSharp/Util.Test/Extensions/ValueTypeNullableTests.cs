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
using Xunit;

namespace Moreland.CSharp.Util.Test.Extensions
{
    public sealed class ValueTypeNullableTests
    {
        [Fact]
        public void HasValueIsTrueWhenNonNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(Guid.NewGuid(), Guid.NewGuid())
                .AssertHasValue(true);

        [Fact]
        public void HasValueIsFalseWhenNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(null, Guid.NewGuid())
                .AssertHasValue(false);

        [Fact]
        public void OrElseReturnsOrWhenValueIsNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(null, Guid.NewGuid())
                .ActUsingOrElse()
                .AssertCorrectResult();

        [Fact]
        public void OrElseReturnsValueWhenNonNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(Guid.NewGuid(), Guid.NewGuid())
                .ActUsingOrElse()
                .AssertCorrectResult();

        [Fact]
        public void OrElseGetReturnsOrWhenValueIsNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(null, Guid.NewGuid())
                .ActUsingOrElseGet()
                .AssertCorrectResult();

        [Fact]
        public void OrElseGetReturnsValueWhenNonNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(Guid.NewGuid(), Guid.NewGuid())
                .ActUsingOrElseGet()
                .AssertCorrectResult();

        [Fact]
        public void OrElseGetThrowsWhenSupplierIsNull() =>
            ValueTypeNullableExtensionTestContext
            .ArrangeUsingSupplier(null, null!)
            .ActAndAssertThrowUsingOrElseGet<ArgumentNullException>();

        [Fact]
        public void OrElseThrowsExceptionWhenValueIsNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(null, Guid.NewGuid())
                .ActAndAssertThrowUsingOrElseThrow<InvalidOperationException>(() => new InvalidOperationException());

        [Fact]
        public void OrElseThrowsNoExceptionWhenValueIsNonNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(Guid.NewGuid(), Guid.NewGuid())
                .ActUsingOrElseThrow<InvalidOperationException>(() => new InvalidOperationException())
                .AssertCorrectResult();

        [Fact]
        public void OrElseThrowsArgumentNullExceptionWhenSupplierIsNull() =>
            ValueTypeNullableExtensionTestContext
                .Arrange(Guid.NewGuid(), Guid.NewGuid())
                .ActAndAssertThrowUsingOrElseThrow<ArgumentNullException>(null!);
    }
}
