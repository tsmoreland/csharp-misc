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
using NUnit.Framework.Internal;

namespace Moreland.CSharp.Util.Test
{
    [TestFixture]
    public sealed class EitherTests
    {
        private Either<Guid, string> _leftEither = null!;
        private Either<Guid, string> _rightEither = null!;
        private Guid _leftValue = Guid.Empty;
        private string _rightValue = string.Empty;
        private Guid _alternateLeft = Guid.Empty;
        private string _alternateRight = string.Empty;

        [OneTimeSetUp]
        public void SetUp()
        {
            _leftValue = new Guid("B2D7D513-BBB0-4320-AFFF-B28B7BC2F640");
            _leftEither = _leftValue;
            _rightValue = "ARBITRARY VALUE";
            _rightEither = _rightValue;
            _alternateLeft = new Guid("DA839174-9043-4F2F-B2E8-00B92F03CD55");
            _alternateRight = _alternateLeft.ToString("N");
        }

        [Test]
        public void IsEmpty_ReturnsTrue_WhenEmptyConstructorUsed()
        {
            Either<Guid, string> @default = new Either<Guid, string>();
            Assert.That(@default.IsEmpty, Is.True);
        }

        [Test]
        public void IsEmpty_ReturnsFalse_WhenHasLeftValue() =>
            Assert.That(_leftEither.IsEmpty, Is.False);

        [Test]
        public void IsEmpty_ReturnsFalse_WhenHasRightValue() =>
            Assert.That(_rightEither.IsEmpty, Is.False);

        [Test]
        public void LeftValue_ReturnsValue_WhenHasLeftValue()
        {
            Guid actual = _leftEither.LeftValue;
            Assert.That(actual, Is.EqualTo(_leftValue));
        }
        [Test]
        public void LeftValue_ThrowsInvalidOperationException_WhenDoesNotHaveLeftValue()
        {
            Assert.Throws<InvalidOperationException>(() => _ = _rightEither.LeftValue);
        }

        [Test]
        public void RightValue_ReturnsValue_WhenHasRightValue()
        {
            string actual = _rightEither.RightValue;
            Assert.That(actual, Is.EqualTo(_rightValue));
        }

        [Test]
        public void RightValue_ThrowsInvalidOperationException_WhenDoesNotHaveRightValue()
        {
            Assert.Throws<InvalidOperationException>(() => _ = _leftEither.RightValue);
        }

        [Test]
        public void HasLeftValue_ReturnsTrue_WhenEitherContainsTLeft() =>
            Assert.That(_leftEither.HasLeftValue, Is.True);

        [Test]
        public void HasLeftValue_ReturnsFalse_WhenEitherContainsTRight() =>
            Assert.That(_rightEither.HasLeftValue, Is.False);

        [Test]
        public void HasRightValue_ReturnsTrue_WhenEitherContainsTRight() =>
            Assert.That(_rightEither.HasRightValue, Is.True);

        [Test]
        public void HasRightValue_ReturnsFalse_WhenEitherContainsTLeft() =>
            Assert.That(_leftEither.HasRightValue, Is.False);

        [Test]
        public void LeftValueOrDefault_ReturnsLeftvalue_WhenHasLeftValue()
        {
            var actual = _leftEither.LeftValueOrDefault();
            Assert.That(actual, Is.EqualTo(_leftValue));
        }
        [Test]
        public void LeftValueOrDefault_ReturnsDefault_WhenNotHasLeftValue()
        {
            var expected = default(Guid);
            var actual = _rightEither.LeftValueOrDefault();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void RightValueOrDefault_ReturnsRightvalue_WhenHasRightValue()
        {
            var actual = _rightEither.RightValueOrDefault();
            Assert.That(actual, Is.EqualTo(_rightValue));
        }
        [Test]
        public void RightValueOrDefault_ReturnsDefault_WhenNotHasRightValue()
        {
            const string expected = default!;
            var actual = _leftEither.RightValueOrDefault();
            Assert.That(actual, Is.EqualTo(expected));
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

        public enum EitherValueType
        {
            Left, 
            Right,
        }

        [TestCase(EitherValueType.Left)]
        [TestCase(EitherValueType.Right)]
        public void OperatorEquals_ReturnsTrue_WhenContainsEqualValues(EitherValueType type)
        {
            var first = GetEitherUsing(type);
            var second = GetEitherUsing(type);

            Assert.That(first == second, Is.True);
        }

        [TestCase(EitherValueType.Left)]
        [TestCase(EitherValueType.Right)]
        public void OperatorEquals_ReturnsFalse_WhenContainsUnequalValues(EitherValueType type)
        {
            var first = GetEitherUsing(type);
            var second = GetEitherUsing(type, _alternateLeft, _alternateRight);

            Assert.That(first == second, Is.False);
        }

        [Test]
        public void OperatorEquals_ReturnsFalse_WhenContainsDifferentTypes() =>
            Assert.That(_leftEither == _rightEither, Is.False);

        [TestCase(EitherValueType.Left)]
        [TestCase(EitherValueType.Right)]
        public void OperatorNotEquals_ReturnsFalse_WhenContainsEqualValues(EitherValueType type)
        {
            var first = GetEitherUsing(type);
            var second = GetEitherUsing(type);

            Assert.That(first != second, Is.False);
        }

        [TestCase(EitherValueType.Left)]
        [TestCase(EitherValueType.Right)]
        public void OperatorEquals_ReturnsTrue_WhenContainsUnequalValues(EitherValueType type)
        {
            var first = GetEitherUsing(type);
            var second = GetEitherUsing(type, _alternateLeft, _alternateRight);

            Assert.That(first != second, Is.True);
        }

        [Test]
        public void OperatorEquals_ReturnsTrue_WhenContainsDifferentTypes() =>
            Assert.That(_leftEither != _rightEither, Is.True);

        private Either<Guid, string> GetEitherUsing(EitherValueType type) =>
            GetEitherUsing(type, _leftValue, _rightValue);
        private static Either<Guid, string> GetEitherUsing(EitherValueType type, Guid leftValue, string rightValue) =>
            type switch
            {
                EitherValueType.Left => leftValue,
                EitherValueType.Right => rightValue,
                _ => throw new InvalidTestFixtureException("Unrecongized either value type")
            };
    };
}
