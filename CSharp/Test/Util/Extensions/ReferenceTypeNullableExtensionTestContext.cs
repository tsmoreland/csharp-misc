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
    public sealed class ReferenceTypeNullableExtensionTestContext : INullableExtentensionTestContext
    {
        public ReferenceTypeNullableExtensionTestContext(object? value, object or)
        {
            Value = value;
            Or = or;
        }

        public static INullableExtentensionTestContext Arrange(object? value, object or) => 
            new ReferenceTypeNullableExtensionTestContext(value, or);

        public object? Value { get; }
        public object Or { get; } = null!;

        public object Result { get; private set; } = null!;

        public Func<object> OrSupplier => OrGetter;

        private object OrGetter() => Or;

        public INullableExtentensionTestContext ActUsingOrElse()
        {
            Result = Value.OrElse(Or);
            return this;
        }
        public INullableExtentensionTestContext ActUsingOrElseGet()
        {
            Result = Value.OrElseGet(OrGetter);
            return this;
        }
        public INullableExtentensionTestContext ActUsingOrElseThrow<TException>(Func<Exception> supplier) where TException : Exception
        {
            Result = Value.OrElseThrow(supplier);
            return this;
        }

        public void ActAndAssertThrowUsingOrElseThrow<TException>(Func<Exception> supplier) where TException : Exception
        {
            Assert.Throws<TException>(() => Value.OrElseThrow(supplier));
        }

        public void AssertHasValue(bool expected) => Assert.Equal(expected, Value.HasValue());

        public void AssertCorrectResult()
        {
            if (Value == null)
                Assert.Equal(Or, Result);
            else
                Assert.Equal(Value, Result);
        }
    }
}
