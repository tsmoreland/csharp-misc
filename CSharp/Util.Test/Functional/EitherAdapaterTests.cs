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
using NSubstitute;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Functional
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
        public void Map_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => _ = source.Map(MapFromLeft, MapFromRight));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Map_ThrowsArgumentException_WhenSourceIsEmpty()
        {
            Either<Guid, string> source = new Either<Guid, string>();
            var ex = Assert.Throws<ArgumentException>(() => _ = source.Map(MapFromLeft, MapFromRight));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Map_ThrowsArgumentNullException_WhenFromLeftIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _leftEither.Map(null!, MapFromRight));
            Assert.That(ex.ParamName, Is.EqualTo("fromLeft"));
        }

        [Test]
        public void Map_ThrowsArgumentNullException_WhenFromRightIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _ = _leftEither.Map(MapFromLeft, null!));
            Assert.That(ex.ParamName, Is.EqualTo("fromRight"));
        }

        [Test]
        public void Map_UsesFromLeft_WhenSourceHasLeft()
        {
            var fromLeft = Substitute.For<Func<Guid, int>>();
            var fromRight = Substitute.For<Func<string, int>>();
            fromLeft.Invoke(_leftValue).Returns(42);

            _ = _leftEither.Map(fromLeft, fromRight);

            fromLeft.ReceivedWithAnyArgs(1);
        }

        [Test]
        public void Map_UsesFromRight_WhenSourceHasRight()
        {
            var fromLeft = Substitute.For<Func<Guid, int>>();
            var fromRight = Substitute.For<Func<string, int>>();
            fromRight.Invoke(_rightValue).Returns(42);

            _ = _rightEither.Map(fromLeft, fromRight);

            fromRight.ReceivedWithAnyArgs(1);
        }

        [Test]
        public void Select_NewRight_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Select(value => 0));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Select_NewRight_ThrowsArgumentException_WhenSelectoreIsNull()
        {
            Func<string, int> selector = null!;
            var ex = Assert.Throws<ArgumentNullException>(() => _leftEither.Select(selector));
            Assert.That(ex.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void Select_NewRight_ReturnsLeftValue_WhenSourceHasLeft()
        {
            var actual = _leftEither.Select(s => s.Length);
            Assert.That(_leftEither.LeftValue, Is.EqualTo(actual.LeftValue));
        }

        [Test]
        public void Select_NewRight_ReturnsNewRightValue_WhenSourceHasRight()
        {
            var actual = _rightEither.Select(s => s.Length);
            Assert.That(_rightValue.Length, Is.EqualTo(actual.RightValue));
        }

        [Test]
        public void Select_Either_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Select(s => Either.From<Guid, int>(0)));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Select_Either_ThrowsArgumentException_WhenSelectoreIsNull()
        {
            Func<string, Either<Guid, int>> selector = null!;
            var ex = Assert.Throws<ArgumentNullException>(() => _leftEither.Select(selector));
            Assert.That(ex.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void Select_Either_ReturnsLeftValue_WhenSourceHasLeft()
        {
            var actual = _leftEither.Select(s => Either.From<Guid,int>(s.Length));
            Assert.That(_leftEither.LeftValue, Is.EqualTo(actual.LeftValue));
        }

        [Test]
        public void Select_Either_ReturnsRightValue_WhenSourceHasRight()
        {
            var actual = _rightEither.Select(s => Either.From<Guid,int>(s.Length));
            Assert.That(_rightValue.Length, Is.EqualTo(actual.RightValue));
        }

        [Test]
        public void Reduce_Left_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Reduce(str => Guid.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }
        [Test]
        public void Reduce_Left_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            Func<string, Guid> reducer = null!;
            var ex = Assert.Throws<ArgumentNullException>(() => _leftEither.Reduce(reducer));
            Assert.That(ex.ParamName, Is.EqualTo("reducer"));
        }

        [Test]
        public void Reduce_Left_ReturnsLeftValue_WhenSourceHasLeftValue()
        {
            var actual = _leftEither.Reduce(s => new Guid("2CFE52C6-DD42-4D33-969D-0B88885B88B4"));

            Assert.That(actual, Is.EqualTo(_leftEither.LeftValue));
        }

        [Test]
        public void Reduce_Left_ReturnsReducerResult_WhenSourceHasRightValue()
        {
            var expected = new Guid("2CFE52C6-DD42-4D33-969D-0B88885B88B4");
            var actual = _rightEither.Reduce(s => expected);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Reduce_Right_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.Reduce(guid => string.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Reduce_Right_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            Func<Guid, string> reducer = null!;
            var ex = Assert.Throws<ArgumentNullException>(() => _leftEither.Reduce(reducer));
            Assert.That(ex.ParamName, Is.EqualTo("reducer"));
        }

        [Test]
        public void Reduce_Right_ReturnsLeftValue_WhenSourceHasRightValue()
        {
            var actual = _rightEither.Reduce(guid => guid.ToString());

            Assert.That(actual, Is.EqualTo(_rightEither.RightValue));
        }

        [Test]
        public void Reduce_Right_ReturnsReducerResult_WhenSourceHasLeftValue()
        {
            var expected = _leftEither.LeftValue.ToString();
            var actual = _leftEither.Reduce(guid => guid.ToString());

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ValueOr_Left_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.ValueOr(str => Guid.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }
        [Test]
        public void ValueOr_Left_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            Func<string, Guid> reducer = null!;
            var ex = Assert.Throws<ArgumentNullException>(() => _leftEither.ValueOr(reducer));
            Assert.That(ex.ParamName, Is.EqualTo("reducer"));
        }

        [Test]
        public void ValueOr_Left_ReturnsLeftValue_WhenSourceHasLeftValue()
        {
            var actual = _leftEither.ValueOr(s => new Guid("2CFE52C6-DD42-4D33-969D-0B88885B88B4"));

            Assert.That(actual, Is.EqualTo(_leftEither.LeftValue));
        }

        [Test]
        public void ValueOr_Left_ReturnsValueOrrResult_WhenSourceHasRightValue()
        {
            var expected = new Guid("2CFE52C6-DD42-4D33-969D-0B88885B88B4");
            var actual = _rightEither.ValueOr(s => expected);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ValueOr_Right_ThrowsArgumentException_WhenSourceIsNull()
        {
            Either<Guid, string> source = null!;
            var ex = Assert.Throws<ArgumentException>(() => source.ValueOr(guid => string.Empty));
            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void ValueOr_Right_ThrowsArgumentNullException_WhenSelectorIsNull()
        {
            Func<Guid, string> reducer = null!;
            var ex = Assert.Throws<ArgumentNullException>(() => _leftEither.ValueOr(reducer));
            Assert.That(ex.ParamName, Is.EqualTo("reducer"));
        }

        [Test]
        public void ValueOr_Right_ReturnsLeftValue_WhenSourceHasRightValue()
        {
            var actual = _rightEither.ValueOr(guid => guid.ToString());

            Assert.That(actual, Is.EqualTo(_rightEither.RightValue));
        }

        [Test]
        public void ValueOr_Right_ReturnsValueOrrResult_WhenSourceHasLeftValue()
        {
            var expected = _leftEither.LeftValue.ToString();
            var actual = _leftEither.ValueOr(guid => guid.ToString());

            Assert.That(actual, Is.EqualTo(expected));
        }

        private static int MapFromLeft(Guid g) => g.ToString().Length;
        private static int MapFromRight(string s) => s.Length;
    }
}
