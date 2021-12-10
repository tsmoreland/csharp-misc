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
using Moreland.CSharp.Util.Functional;
using NUnit.Framework;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class GuardAgainstTests
    {
        [Test]
        public void ArgumentBeginNull_ThrowsArgumentNullExceptionWithUnknownParameterName_WhenArgumentIsNullAndParameterNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => GuardAgainst.ArgumentBeingNull<object>(null!, string.Empty));
            Assert.That(ex!.ParamName, Is.EqualTo(ProjectResources.UnknownParameterName));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_Either_ThrowsArgumentExceptionWithUnknownParameterName_WhenArgumentIsNullAndParameterNameIsEmpty()
        {
            Either<Guid, string> either = null!;
            var ex = Assert.Throws<ArgumentException>(() => GuardAgainst.ArgumentIsNullOrEmpty(either, string.Empty));
            Assert.That(ex!.ParamName, Is.EqualTo(ProjectResources.UnknownParameterName));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_String_ThrowsArgumentExceptionWithUnknownParameterName_WhenArgumentIsNullAndParameterNameIsEmpty()
        {
            string @string = null!;
            var ex = Assert.Throws<ArgumentException>(() => GuardAgainst.ArgumentIsNullOrEmpty(@string, string.Empty));
            Assert.That(ex!.ParamName, Is.EqualTo(ProjectResources.UnknownParameterName));
        }

        [Test]
        public void ArgumentBeginNull_ThrowsArgumentNullExceptionWithGivenParameterName_WhenArgumentIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => GuardAgainst.ArgumentBeingNull<object>(null!, "parameter"));
            Assert.That(ex!.ParamName, Is.EqualTo("parameter"));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_Either_ThrowsArgumentExceptionWithGivenParameterName_WhenArgumentIsNull()
        {
            Either<Guid, string> either = null!;
            var ex = Assert.Throws<ArgumentException>(() => GuardAgainst.ArgumentIsNullOrEmpty(either, "parameter"));
            Assert.That(ex!.ParamName, Is.EqualTo("parameter"));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_Either_ThrowsArgumentExceptionWithGivenParameterName_WhenArgumentIsEmpty()
        {
            Either<Guid, string> either = new Either<Guid, string>();
            var ex = Assert.Throws<ArgumentException>(() => GuardAgainst.ArgumentIsNullOrEmpty(either, "parameter"));
            Assert.That(ex!.ParamName, Is.EqualTo("parameter"));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_String_ThrowsArgumentExceptionWithGivenParameterName_WhenArgumentIsNull()
        {
            string @string = null!;
            var ex = Assert.Throws<ArgumentException>(() => GuardAgainst.ArgumentIsNullOrEmpty(@string, "parameter"));
            Assert.That(ex!.ParamName, Is.EqualTo("parameter"));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_String_ThrowsArgumentExceptionWithGivenParameterName_WhenArgumentIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => GuardAgainst.ArgumentIsNullOrEmpty(string.Empty, "parameter"));
            Assert.That(ex!.ParamName, Is.EqualTo("parameter"));
        }

        [Test]
        public void ArgumentBeginNull_DoesNotThrow_WhenArgumentIsNotNull()
        {
            object @object = new object();
            Assert.DoesNotThrow(() => GuardAgainst.ArgumentBeingNull(@object, "parameter"));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_Either_DoesNotThrow_WhenArgumentIsNotNullOrEmpty()
        {
            var either = Either.From<Guid, string>(Guid.Empty);
            Assert.DoesNotThrow(() => GuardAgainst.ArgumentIsNullOrEmpty(either, "parameter"));
        }

        [Test]
        public void ArgumentIsNullOrEmpty_String_DoesNotThrow_WhenArgumentIsNotNullOrEmpty()
        {
            string @string = "not-empty";
            Assert.DoesNotThrow(() => GuardAgainst.ArgumentIsNullOrEmpty(@string, "parameter"));
        }
    }
}
