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
    public sealed class ValueResultCoreTest
    {
        [Theory]
        [MemberData(nameof(ResultConstructorValues))]
        public void Constructor_StoresProvidedValues(object value, bool success, string message, Exception? cause)
        {
            if (value is Guid uid)
            {
                var result = new ValueResultCore<Guid>(uid, success, message, cause);
                Assert.Equal(uid, result.Value);
                Assert.Equal(success, result.Success);
                Assert.Equal(message, result.Message);
                Assert.Equal(cause, result.Cause);
            }
            else
            {
                var result = new ValueResultCore<object>(value, success, message, cause);
                Assert.Equal(value, result.Value);
                Assert.Equal(success, result.Success);
                Assert.Equal(message, result.Message);
                Assert.Equal(cause, result.Cause);
            }
        }

        [Theory]
        [MemberData(nameof(EqualValueTests))]
        public void Equals_EqualValuesReturnTrue(object firstObj, object secondObj)
        {
            var first = (ValueResultCore<Guid>)firstObj;
            var second = (ValueResultCore<Guid>)secondObj;

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
        [MemberData(nameof(EqualReferenceTests))]
        public void Equals_EqualReferenceReturnTrue(object firstObj, object secondObj)
        {
            var first = (ValueResultCore<object>)firstObj;
            var second = (ValueResultCore<object>)secondObj;

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
            var first = (ValueResultCore<Guid>)firstObj;
            var second = (ValueResultCore<Guid>)secondObj;

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
        [MemberData(nameof(NotEqualTestReferences))]
        public void Equals_NonEqualReferenceReturnFalse(object firstObj, object secondObj)
        {
            var first = (ValueResultCore<object>)firstObj;
            var second = (ValueResultCore<object>)secondObj;

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
        public void GetHashCode_EqualsCombinationOfValues(object value, bool success, string message, Exception? cause)
        {
            int expectedHashCode;
            int actualHashCode;
            if (value is Guid uid)
            {
                var result = new ValueResultCore<Guid>(uid, success, message, cause);
                expectedHashCode = HashProxy.Combine(uid, success, message, cause);
                actualHashCode = result.GetHashCode();
            }
            else
            {
                var result = new ValueResultCore<object>(value, success, message, cause);
                expectedHashCode = HashProxy.Combine(value, success, message, cause);
                actualHashCode = result.GetHashCode();
            }
            Assert.Equal(expectedHashCode, actualHashCode);
        }

        public static IEnumerable<object[]> ResultConstructorValues()
        {
            yield return ArrayExtensions.Of<object>(Guid.NewGuid(), true, Guid.NewGuid().ToString(), null!);
            yield return ArrayExtensions.Of<object>(Guid.NewGuid(), true, Guid.NewGuid().ToString(), new InvalidOperationException());
            yield return ArrayExtensions.Of<object>(Guid.Empty, false, Guid.NewGuid().ToString(), new InvalidOperationException());

            var @object = new object();
            yield return ArrayExtensions.Of<object>(@object, true, Guid.NewGuid().ToString(), null!);
            yield return ArrayExtensions.Of<object>(@object, true, Guid.NewGuid().ToString(), new InvalidOperationException());
            yield return ArrayExtensions.Of<object>(null!, false, Guid.NewGuid().ToString(), new InvalidOperationException());
        }

        public static IEnumerable<object[]> EqualValueTests()
        {
            Guid value = Guid.NewGuid();
            string message = Guid.NewGuid().ToString();
            Exception cause = new NotSupportedException();

            yield return ArrayExtensions.Of<object>(new ValueResultCore<Guid>(value, true, message, cause), new ValueResultCore<Guid>(value, true, message, cause));
        }
        public static IEnumerable<object[]> EqualReferenceTests()
        {
            string message = Guid.NewGuid().ToString();
            Exception cause = new NotSupportedException();
            var @object = new object();

            yield return ArrayExtensions.Of<object>(new ValueResultCore<object>(@object, true, message, cause), new ValueResultCore<object>(@object, true, message, cause));
        }
        public static IEnumerable<object[]> NotEqualTestValues()
        {
            var firstId = Guid.NewGuid().ToString();
            Guid value = Guid.NewGuid();
            var firstValue = new ValueResultCore<Guid>(value, true, firstId, null);
            var secondValue = new ValueResultCore<Guid>(Guid.Empty, false, Guid.NewGuid().ToString(), new NotSupportedException());
            var thirdValue = new ValueResultCore<Guid>(value, true, firstId, new InvalidCastException());

            yield return ArrayExtensions.Of<object>(firstValue, secondValue);
            yield return ArrayExtensions.Of<object>(secondValue, firstValue);
            yield return ArrayExtensions.Of<object>(firstValue, thirdValue);
        }
        public static IEnumerable<object[]> NotEqualTestReferences()
        {
            var firstId = Guid.NewGuid().ToString();
            var @object = new object();
            var firstRef = new ValueResultCore<object>(@object, true, firstId, null);
            var secondRef = new ValueResultCore<object>(new object(), false, Guid.NewGuid().ToString(), new NotSupportedException());
            var thirdRef = new ValueResultCore<object>(@object, true, firstId, new InvalidCastException());
            var fourthRef = new ValueResultCore<object>(null!, false, Guid.NewGuid().ToString(), new NotSupportedException());

            yield return ArrayExtensions.Of<object>(firstRef, secondRef);
            yield return ArrayExtensions.Of<object>(secondRef, firstRef);
            yield return ArrayExtensions.Of<object>(firstRef, thirdRef);
            yield return ArrayExtensions.Of<object>(secondRef, fourthRef);
            yield return ArrayExtensions.Of<object>(fourthRef, secondRef);
        }
    }
}
