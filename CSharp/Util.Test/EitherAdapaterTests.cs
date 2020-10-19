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
    public sealed class EitherAdapaterTests
    {
        private Either<Guid, string> _leftEither = null!;
        private Either<Guid, string> _rightEither = null!;
        private Guid _leftValue = Guid.Empty;
        private string _rightValue = string.Empty;

        [OneTimeSetUp]
        public void SetUp()
        {
            _leftValue = new Guid("B2D7D513-BBB0-4320-AFFF-B28B7BC2F640");
            _leftEither = _leftValue;
            _rightValue = "ARBITRARY VALUE";
            _rightEither = _rightValue;
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

        [Test]
        public void Select_NewRight_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Select(value => 0));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Select_Either_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Select(s => Either.From<Guid, int>(0)));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Reduce_Left_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Reduce(guid => string.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }
        [Test]
        public void Reduce_Right_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Reduce(str => Guid.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        /*
        [Test]
        public void Select_NewRight_ReturnsTransformedContainingLeft_WhenSourceHasLeftValue()
        {
            var transformed = _leftEither.Select(SelectNewRight);
            Assert.That(transformed, Is.InstanceOf<LeftEither<Guid, int>>());
        }
        [Test]
        public void Select_NewRight_ReturnsTransformedContainingRight_WhenSourceHasRightValue()
        {
            var transformed = _rightEither.Select(SelectNewRight);
            Assert.That(transformed, Is.InstanceOf<RightEither<Guid, int>>());
        }
        [Test]
        public void Select_Either_ReturnsTransformedContainingLeft_WhenSourceHasLeftValue()
        {
            var transformed = _leftEither.Select(SelectEither);
            Assert.That(transformed, Is.InstanceOf<LeftEither<Guid, int>>());
        }
        [Test]
        public void Select_Either_ReturnsTransformedContainingRight_WhenSourceHasRightValue()
        {
            var transformed = _rightEither.Select(SelectEither);
            Assert.That(transformed, Is.InstanceOf<RightEither<Guid, int>>());
        }

        private static int SelectNewRight(string value) => value.Length;
        private static Either<Guid, int> SelectEither(string value) => SelectNewRight(value);
        */
    }
}
