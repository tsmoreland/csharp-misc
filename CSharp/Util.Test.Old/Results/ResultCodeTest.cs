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
using Moreland.CSharp.Util.Extensions;
using Moreland.CSharp.Util.Internal;
using Xunit;

namespace Moreland.CSharp.Util.Test.Old.Results
{
    public sealed class ResultCodeTest
    {
        [Theory]
        [MemberData(nameof(ResultConstructorValues))]
        public void Constructor_StoresProvidedValues(bool success, string message, Exception? cause)
        {
            var result = new ResultCore(success, message, cause);

            Assert.Equal(success, result.Success);
            Assert.Equal(message, result.Message);
            Assert.Equal(cause, result.Cause);
        }

        [Fact]
        public void Equals_EqualValuesReturnTrue()
        {
            var message = Guid.NewGuid().ToString();
            var cause = new InvalidOperationException();
            var first = new ResultCore(true, message, cause);
            var second = new ResultCore(true, message, cause);

            bool equals = first.Equals(second);
            bool operatorEquals = first == second;
            bool operatorNotEquals = first != second;
            bool objEquals = first.Equals((object)second);
            bool objNotEquals = first.Equals((object)null!);

            Assert.True(equals);
            Assert.True(operatorEquals);
            Assert.False(operatorNotEquals);
            Assert.True(objEquals);
            Assert.False(objNotEquals);

        }

        [Theory]
        [MemberData(nameof(NotEqualTestValues))]
        public void Equals_NonEqualValuesReturnFalse(object firstObj, object secondObj)
        {
            var first = (ResultCore)firstObj;
            var second = (ResultCore)secondObj;

            bool equals = first.Equals(second);
            bool operatorEquals = first == second;
            bool operatorNotEquals = first != second;
            bool objEquals = first.Equals((object)second);
            bool objNotEquals = first.Equals((object)null!);

            Assert.False(equals);
            Assert.False(operatorEquals);
            Assert.True(operatorNotEquals);
            Assert.False(objEquals);
            Assert.False(objNotEquals);
        }

        [Theory]
        [MemberData(nameof(ResultConstructorValues))]
        public void GetHashCode_EqualsCombinationOfValues(bool success, string message, Exception? cause)
        {
            var result = new ResultCore(success, message, cause);
            var expectedHashCode = HashProxy.Combine(success, message, cause);
            var actualHashCode = result.GetHashCode();

            Assert.Equal(expectedHashCode, actualHashCode);
        }

        public static IEnumerable<object[]> ResultConstructorValues()
        {
            yield return ArrayExtensions.Of<object>(true, Guid.NewGuid().ToString(), null!);
            yield return ArrayExtensions.Of<object>(true, Guid.NewGuid().ToString(), new InvalidOperationException());
            yield return ArrayExtensions.Of<object>(false, Guid.NewGuid().ToString(), new InvalidOperationException());
        }

        public static IEnumerable<object[]> NotEqualTestValues()
        {
            var firstId = Guid.NewGuid().ToString();
            var first = new ResultCore(true, firstId, null);
            var second = new ResultCore(false, Guid.NewGuid().ToString(), new NotSupportedException());
            var third = new ResultCore(true, firstId, new InvalidCastException());

            yield return ArrayExtensions.Of<object>(first, second);
            yield return ArrayExtensions.Of<object>(second, first);
            yield return ArrayExtensions.Of<object>(first, third);
        }

    }
}
