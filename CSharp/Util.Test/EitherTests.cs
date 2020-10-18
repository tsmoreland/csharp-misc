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

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class EitherTests
    {
        private Either<Guid, string> _leftEither = null!;
        private Either<Guid, string> _rightEither = null!;
        private Guid _leftValue = Guid.Empty;
        private string _rightValue = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _leftValue = new Guid("B2D7D513-BBB0-4320-AFFF-B28B7BC2F640");
            _leftEither = _leftValue;
            _rightValue = "ARBITRARY VALUE";
            _rightEither = _rightValue;
        }

        [Test]
        public void LeftValue_ReturnsValue_WhenHasLeftValue()
        {
            Guid actual = _leftEither.LeftValue;
            Assert.That(actual, Is.EqualTo(_leftValue));
        }

        [Test]
        public void RightValue_ReturnsValue_WhenHasRightValue()
        {
            string actual = _rightEither.RightValue;
            Assert.That(actual, Is.EqualTo(_rightValue));
        }

        [Test]
        public void ImplicitOperator_ReturnsEitherContainingLeftValue_FromLeftValue()
        {
            Either<Guid, string> either = _leftValue;
            Assert.That(either.HasLeftValue, Is.True);
        }
        [Test]
        public void ImplicitOperator_ReturnsEitherContainingRightValue_FromRightValue()
        {
            Either<Guid, string> either = _rightValue;
            Assert.That(either.HasRightValue, Is.True);
        }

        [Test]
        public void ToLeftValue_ReturnsMaybeWithValue_WhenHasLeftValue()
        {
            var maybeLeft = _leftEither.ToLeftValue();
            Assert.That(maybeLeft.HasValue, Is.True);
        }
        [Test]
        public void ToLeftValue_ReturnsEmptyMaybe_WhenHasRightValue()
        {
            var maybeLeft = _rightEither.ToLeftValue();
            Assert.That(maybeLeft.HasValue, Is.False);
        }
        [Test]
        public void ToLeftValue_ReturnsLeftValue_WhenHasLeftValue()
        {
            var maybeLeft = _leftEither.ToLeftValue();
            Assert.That(maybeLeft.Value, Is.EqualTo(_leftValue));
        }
        [Test]
        public void ToRightValue_ReturnsMaybeWithValue_WhenHasRightValue()
        {
            var maybeRight = _rightEither.ToRightValue();
            Assert.That(maybeRight.HasValue, Is.True);
        }
        [Test]
        public void ToRightValue_ReturnsEmptyMaybe_WhenHasLeftValue()
        {
            var maybeRight = _leftEither.ToRightValue();
            Assert.That(maybeRight.HasValue, Is.False);
        }
        [Test]
        public void ToRightValue_ReturnsRightValue_WhenHasRightValue()
        {
            var maybeRight = _rightEither.ToRightValue();
            Assert.That(maybeRight.Value, Is.EqualTo(_rightValue));
        }

    }
}
