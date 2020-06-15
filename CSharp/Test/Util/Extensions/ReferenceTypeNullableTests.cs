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

namespace CSharp.Test.Util.Extensions
{

    public sealed class ReferenceTypeNullableTests
    {
        [Fact]
        public void HasValueIsTrueWhenNonNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(new object(), new object())
                .AssertHasValue(true);

        [Fact]
        public void HasValueIsFalseWhenNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(null, new object())
                .AssertHasValue(false);

        [Fact]
        public void OrElseReturnsOrWhenValueIsNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(null, new object())
                .ActUsingOrElse()
                .AssertCorrectResult();

        [Fact]
        public void OrElseReturnsValueWhenNonNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(new object(), new object())
                .ActUsingOrElse()
                .AssertCorrectResult();

        [Fact]
        public void OrElseGetReturnsOrWhenValueIsNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(null, new object())
                .ActUsingOrElseGet()
                .AssertCorrectResult();

        [Fact]
        public void OrElseGetReturnsValueWhenNonNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(new object(), new object())
                .ActUsingOrElseGet()
                .AssertCorrectResult();

        [Fact]
        public void OrElseThrowsExceptionWhenValueIsNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(null, new object())
                .ActAndAssertThrowUsingOrElseThrow<InvalidOperationException>(() => new InvalidOperationException());

        [Fact]
        public void OrElseThrowsNoExceptionWhenValueIsNonNull() =>
            ReferenceTypeNullableExtensionTestContext
                .Arrange(new object(), new object())
                .ActUsingOrElseThrow<InvalidOperationException>(() => new InvalidOperationException())
                .AssertCorrectResult();
    }
}
