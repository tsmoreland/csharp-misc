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
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test.Extensions
{
    [TestFixture]
    public sealed class NullableExtensionsTests 
    {
        [Test]
        public void HasValue_ValueType_ReturnsTrue_WhenNotNull()
        {
            int? value = BuildRandomInt32();
            Assert.That(value.HasValue(), Is.True);
        }
        [Test]
        public void HasValue_ReferenceType_ReturnsTrue_WhenNotNull()
        {
            List<string>? value = BuildRandomListOfString();
            Assert.That(value.HasValue(), Is.True);
        }

        [Test]
        public void HasValue_ValueType_ReturnsFalse_WhenNull()
        {
            int? value = null;
            Assert.That(value.HasValue(), Is.False);
        }
        [Test]
        public void HasValue_ReferenceType_ReturnsFalse_WhenNull()
        {
            List<string>? value = null;
            Assert.That(value.HasValue(), Is.False);
        }

        [Test]
        public void ValueOr_ValueType_ReturnsValue_WhenNotNull()
        {
            int? value = BuildRandomInt32();
            int or = BuildRandomInt32();

            var actual = value.ValueOr(or);

            Assert.That(actual, Is.EqualTo(value));
        }
        [Test]
        public void ValueOr_ReferenceType_ReturnsValue_WhenNotNull()
        {
            List<string>? value = BuildRandomListOfString();
            List<string> or = BuildRandomListOfString();

            var actual = value.ValueOr(or);

            Assert.That(actual, Is.EqualTo(value));
        }
        [Test]
        public void ValueOr_ValueType_ReturnsOr_WhenNull()
        {
            int? value = null;
            int or = BuildRandomInt32();

            var actual = value.ValueOr(or);

            Assert.That(actual, Is.EqualTo(or));
        }
        [Test]
        public void ValueOr_ReferenceType_ReturnsOr_WhenNull()
        {
            List<string>? value = null;
            List<string> or = BuildRandomListOfString();

            var actual = value.ValueOr(or);

            Assert.That(actual, Is.EqualTo(or));
        }

        [Test]
        public void ValueOr_Supplier_ValueType_ThrowsArgumentNullException_WhenSupplierIsNull()
        {
            int? value = BuildRandomInt32();

            var ex = Assert.Throws<ArgumentNullException>(() => _ = value.ValueOr(null!));
            Assert.That(ex.ParamName, Is.EqualTo("supplier"));
        }

        [Test]
        public void ValueOr_Supplier_ReferenceType_ThrowsArgumentNullException_WhenSupplierIsNull()
        {
            List<string>? value = BuildRandomListOfString();

            var ex = Assert.Throws<ArgumentNullException>(() => _ = value.ValueOr((Func<List<string>>)null!));
            Assert.That(ex.ParamName, Is.EqualTo("supplier"));
        }

        [Test]
        public void ValueOr_Supplier_ValueType_ReturnsValue_WhenHasValue()
        {
            int? value = BuildRandomInt32();
            int Supplier() => BuildRandomInt32((int)value);

            var actual = value.ValueOr(Supplier);

            Assert.That(actual, Is.EqualTo(value));
        }

        [Test]
        public void ValueOr_Supplier_ReferenceType_ReturnsValue_WhenHasValue()
        {
            List<string>? value = BuildRandomListOfString();
            static List<string> Supplier() => BuildRandomListOfString();

            var actual = value.ValueOr(Supplier);

            Assert.That(actual, Is.EqualTo(value));
        }

        [Test]
        public void ValueOr_Supplier_ValueType_ReturnsSupplierValue_WhenNull()
        {
            int? value = null;
            int suppliedValue = BuildRandomInt32();
            int Supplier() => suppliedValue;

            var actual = value.ValueOr(Supplier);

            Assert.That(actual, Is.EqualTo(suppliedValue));
        }

        [Test]
        public void ValueOr_Supplier_ReferenceType_ReturnsSupplierValue_WhenNull()
        {
            List<string>? value = null;
            var suppliedValue = BuildRandomListOfString();
            List<string> Supplier() => suppliedValue;

            var actual = value.ValueOr(Supplier);

            Assert.That(actual, Is.EqualTo(suppliedValue));
        }

        [Test]
        public void ValueOrThrow_ValueType_ThrowsArgumentNullException_WhenSupplierIsNull()
        {
            int? value = BuildRandomInt32();

            var ex = Assert.Throws<ArgumentNullException>(() => _ = value.ValueOrThrow(null!));
            Assert.That(ex.ParamName, Is.EqualTo("exceptionSupplier"));
        }

        [Test]
        public void ValueOrThrow_ReferenceType_ThrowsArgumentNullException_WhenSupplierIsNull()
        {
            List<string>? value = BuildRandomListOfString();

            var ex = Assert.Throws<ArgumentNullException>(() => _ = value.ValueOrThrow(null!));
            Assert.That(ex.ParamName, Is.EqualTo("exceptionSupplier"));
        }

        [Test]
        public void ValueOrThrow_ValueType_ReturnsValue_WhenHasValue()
        {
            int? value = BuildRandomInt32();
            static Exception Supplier() => new Exception(Guid.NewGuid().ToString());

            var actual = value.ValueOrThrow(Supplier);

            Assert.That(actual, Is.EqualTo(value));
        }

        [Test]
        public void ValueOrThrow_ReferenceType_ReturnsValue_WhenHasValue()
        {
            List<string>? value = BuildRandomListOfString();
            static Exception Supplier() => new Exception(Guid.NewGuid().ToString());

            var actual = value.ValueOrThrow(Supplier);

            Assert.That(actual, Is.EqualTo(value));
        }

        [Test]
        public void ValueOrThrow_ValueType_ThrowsSupplierException_WhenNull()
        {
            int? value = null;
            Exception expected = new Exception(Guid.NewGuid().ToString());
            Exception Supplier() => expected;

            var actual = Assert.Catch<Exception>(() => _ = value.ValueOrThrow(Supplier));
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ValueOrThrow_ReferenceType_ThrowsSupplierException_WhenNull()
        {
            List<string>? value = null;
            Exception expected = new Exception(Guid.NewGuid().ToString());
            Exception Supplier() => expected;

            var actual = Assert.Catch<Exception>(() => _ = value.ValueOrThrow(Supplier));
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
