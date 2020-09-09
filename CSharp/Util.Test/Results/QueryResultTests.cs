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
using Moreland.CSharp.Util.Results;
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Test.Results
{
    [TestFixture]
    public sealed class ValueTypeQueryResultTests : QueryResultTests<int>
    {
        public ValueTypeQueryResultTests()
            : base(() => BuildRandomInt32())
        {
        }
    }

    [TestFixture]
    public sealed class EquatableReferenceTypeQueryResultTests : QueryResultTests<string>
    {
        public EquatableReferenceTypeQueryResultTests()
            : base(() => BuildRandomString())
        {
        }
    }

    [TestFixture]
    public sealed class ReferenceTypeQueryResultTests : QueryResultTests<List<string>>
    {
        public ReferenceTypeQueryResultTests()
            : base(() => BuildRandomListOfString())
        {
        }
    }

    public abstract class QueryResultTests<T>
    {
        private readonly Func<T> _builder;
        private string _message = null!;
        private Exception _cause = null!;
        private T _value = default!;
        private QueryResult<T> _ok = QueryResult.Failed<T>("invalid");
        private QueryResult<T> _okWithMessage = QueryResult.Failed<T>("invalid");
        private QueryResult<T> _failed = QueryResult.Failed<T>("invalid");
        private QueryResult<T> _failedWithCause = QueryResult.Failed<T>("invalid");

        protected QueryResultTests(Func<T> builder)
        {
            _builder = builder;
        }

        [SetUp]
        public void Setup()
        {
            _message = Guid.NewGuid().ToString("N");
            _cause = new Exception(_message);
            _value = _builder();

            _ok = QueryResult.Ok(_value);
            _okWithMessage = QueryResult.Ok(_value, _message);
            _failed = QueryResult.Failed<T>(_message);
            _failedWithCause = QueryResult.Failed<T>(_message, _cause);
        }

        [Test]
        public void Failed_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = QueryResult.Failed<T>(null!));
            Assert.That(ex.ParamName, Is.EqualTo("message"));
        }

        [Test]
        public void FailedWithCause_ThrowsArgumentNullException_WhenMessageIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = QueryResult.Failed<T>(null!, _cause));
            Assert.That(ex.ParamName, Is.EqualTo("message"));
        }

        [Test]
        public void Success_ReturnsTrue_WhenOk()
        {
            Assert.That(_ok.Success, Is.True);
        }

        [Test]
        public void Success_ReturnsTrue_WhenOkWithMesssage()
        {
            Assert.That(_okWithMessage.Success, Is.True);
        }

        [Test]
        public void Success_ReturnsFalse_WhenFailed()
        {
            Assert.That(_failed.Success, Is.False);
        }

        [Test]
        public void Success_ReturnsFalse_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Success, Is.False);
        }

        [Test]
        public void Value_ReturnsGivenValue_WhenOk()
        {
            Assert.That(_ok.Value, Is.EqualTo(_value));
        }

        [Test]
        public void Value_ReturnsGivenValue_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Value, Is.EqualTo(_value));
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenFailed()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _ = _failed.Value);
            Assert.That(ex.Message, Is.EqualTo(ProjectResources.InvalidResultValueAccess));
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenFailedWithCause()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _ = _failedWithCause.Value);
            Assert.That(ex.Message, Is.EqualTo(ProjectResources.InvalidResultValueAccess));
        }

        [Test]
        public void Message_ReturnsMessage_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Message_ReturnsEmptyString_WhenOkWithMessageGivenNull()
        {
            Assert.That(QueryResult.Ok(_value).Message, Is.EqualTo(""));
        }

        [Test]
        public void Message_ReturnsMessage_WhenFailed()
        {
            Assert.That(_failed.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Message_ReturnsMessage_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Message, Is.EqualTo(_message));
        }

        [Test]
        public void Cause_ReturnsNull_WhenOk()
        {
            Assert.That(_ok.Cause, Is.Null);
        }

        [Test]
        public void Cause_ReturnsNull_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Cause, Is.Null);
        }

        [Test]
        public void Cause_ReturnsNull_WhenFailed()
        {
            Assert.That(_failed.Cause, Is.Null);
        }

        [Test]
        public void Cause_ReturnsException_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Cause, Is.Not.Null);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenOk()
        {
            Assert.That(_ok.Success, Is.True);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Success, Is.True);
        }

        [Test]
        public void ToBoolean_ReturnsFalse_WhenFailed()
        {
            Assert.That(_failed.Success, Is.False);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Success, Is.False);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenOk()
        {
            bool @bool = _ok;
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsTrue_WhenOkWithMessage()
        {
            bool @bool = _okWithMessage;
            Assert.That(@bool, Is.True);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailed()
        {
            bool @bool = _failed;
            Assert.That(@bool, Is.False);
        }

        [Test]
        public void ImplicitBool_ReturnsFalse_WhenFailedWithCause()
        {
            bool @bool = _failedWithCause;
            Assert.That(@bool, Is.False);
        }
    }
}
