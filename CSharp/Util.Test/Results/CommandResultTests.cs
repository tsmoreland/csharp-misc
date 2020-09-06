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
using Moreland.CSharp.Util.Results;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Results
{
    [TestFixture]
    public sealed class CommandResultTests
    {
        private string _message = null!;
        private Exception _cause = null!;
        private CommandResult _ok = CommandResult.Ok();
        private CommandResult _okWithMessage = CommandResult.Ok();
        private CommandResult _failed = CommandResult.Ok();
        private CommandResult _failedWithCause = CommandResult.Ok();

        [SetUp]
        public void Setup()
        {
            _message = Guid.NewGuid().ToString("N");
            _cause = new Exception(_message);

            _ok = CommandResult.Ok();
            _okWithMessage = CommandResult.Ok(_message);
            _failed = CommandResult.Failed(_message);
            _failedWithCause = CommandResult.Failed(_message, _cause);
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
        public void Message_ReturnsEmpty_WhenOk()
        {
            Assert.That(_ok.Message, Is.Empty);
        }

        [Test]
        public void Message_ReturnsMessage_WhenOkWithMessage()
        {
            Assert.That(_okWithMessage.Message, Is.EqualTo(_message));
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
        public void ToBoolean_ReturnsTrue_WhenFailed()
        {
            Assert.That(_failed.Success, Is.False);
        }

        [Test]
        public void ToBoolean_ReturnsTrue_WhenFailedWithCause()
        {
            Assert.That(_failedWithCause.Success, Is.False);
        }
    }
}
