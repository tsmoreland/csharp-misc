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

using Moreland.CSharp.Util.Internal;
using Xunit;

namespace Moreland.CSharp.Util.Test
{
    public class HashCodeBuilderTest
    {
        [Fact]
        public void Create_HashCodeZeroWhenEmpty()
        {
            var builder = HashCodeBuilder.Create();
            int hasCode = builder.ToHashCode();
            Assert.Equal(0, hasCode);
        }

        [Fact]
        public void Create_NullValuesGiveHashCodeOfZero()
        {
            var builder = HashCodeBuilder.Create(null, null, null);
            var hashCode = builder.ToHashCode();
            Assert.Equal(0, hashCode);
        }

        [Fact]
        public void Equals_EqualWhenBuiltFromSameValues()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(1, 2, 3);

            AssertEquality(true, first, second);
        }
        [Fact]
        public void Equals_NotEqualWhenBuiltFromDifferentValues()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(10, 20, 30);

            AssertEquality(false, first, second);
        }

        [Fact]
        public void Equals_NotEqualToNull()
        {
            HashCodeBuilder first = HashCodeBuilder.Create(1, 2, 3);
            HashCodeBuilder second = null!;

            AssertEquality(false, first, second);
        }

        [Fact]
        public void Equals_EqualWhenBuiltFromSameValuesUsingAddValues()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            first = first.WithAddedValues(4, 5, 6);
            var second = HashCodeBuilder.Create(1, 2, 3, 4, 5, 6);

            AssertEquality(true, first, second);
        }

        [Fact]
        public void ToHashCode_HashCodeEqualsWhenBuiltFromSameValues()
        {
            var first = HashCodeBuilder.Create(1, 2, 3).ToHashCode();
            var second = HashCodeBuilder.Create(1, 2, 3).ToHashCode();
            Assert.Equal(first, second);
        }

        [Fact]
        public void CompareTo_CompareBuilderMatchesHashCode()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(4, 5, 6);
            var firstCode = first.ToHashCode();
            var secondCode = second.ToHashCode();

            int compareTo = first.CompareTo(second);
            int codeCompareTo = firstCode.CompareTo(secondCode);

            Assert.Equal(compareTo, codeCompareTo);
        }

        [Fact]
        public void CompareTo_CompareToNullReturnsOne()
        {
            var builder = HashCodeBuilder.Create(1, 2, 3);

            int compareTo = builder.CompareTo(null!);

            Assert.Equal(1, compareTo);
        }

        [Fact]
        public void ComparisonOperators_BuilderMatchesHashCode()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(4, 5, 6);
            HashCodeBuilder third = null!;
            var firstCode = first.ToHashCode();
            var secondCode = second.ToHashCode();

            Assert.Equal(first < second, firstCode < secondCode);
            Assert.Equal(first > second, firstCode > secondCode);
            Assert.Equal(first <= second, firstCode <= secondCode);
            Assert.Equal(first >= second, firstCode >= secondCode);

            Assert.True(first > third);
            Assert.False(first < third);
            Assert.True(first >= third);
            Assert.False(first <= third);

            Assert.False(third > second);
            Assert.True(third < second);
            Assert.False(third >= second);
            Assert.True(third <= second);
        }

        [Fact]
        public void GetHashCode_EqualBuildersHaveEqualHashCode()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(1, 2, 3);

            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }
        [Fact]
        public void GetHashCode_DifferentBuildersHaveDifferentHashCode()
        {
            var first = HashCodeBuilder.Create(1, 2, 3);
            var second = HashCodeBuilder.Create(4, 5, 6);

            Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
        }

        private void AssertEquality(bool expectedEquality, HashCodeBuilder first, HashCodeBuilder second)
        {
            bool equals = first.Equals(second);
            bool objEquals = first.Equals((object)second);

            bool operatorEquals = first == second;
            bool operatorNotEquals = first != second;

            Assert.Equal(expectedEquality, equals);
            Assert.Equal(expectedEquality, objEquals);
            Assert.Equal(expectedEquality, operatorEquals);
            Assert.Equal(!expectedEquality, operatorNotEquals);
        }

        [Fact]
        public void HashProxy_BuildsHashCode()
        {
            var firstCode = HashProxy.Combine(1);
            var secondCode = HashProxy.Combine(1);
            bool oneValueEqual = firstCode == secondCode;

            firstCode = HashProxy.Combine(1, 2);
            secondCode = HashProxy.Combine(1, 2);
            bool twoValuesEqual = firstCode == secondCode;

            firstCode = HashProxy.Combine(1, 2, 3);
            secondCode = HashProxy.Combine(1, 2, 3);
            bool threeValuesEqual = firstCode == secondCode;

            firstCode = HashProxy.Combine(1, 2, 3, 4);
            secondCode = HashProxy.Combine(1, 2, 3, 4);
            bool fourValuesEqual = firstCode == secondCode;

            firstCode = HashProxy.Combine(1, 2, 3, 4, 5);
            secondCode = HashProxy.Combine(1, 2, 3, 4, 5);
            bool fiveValuesEqual = firstCode == secondCode;
            
            firstCode = HashProxy.Combine(1, 2, 3, 4, 5, 6);
            secondCode = HashProxy.Combine(1, 2, 3, 4, 5, 6);
            bool sixValuesEqual = firstCode == secondCode;

            firstCode = HashProxy.Combine(1, 2, 3, 4, 5, 6, 7);
            secondCode = HashProxy.Combine(1, 2, 3, 4, 5, 6, 7);
            bool sevenValuesEqual = firstCode == secondCode;

            firstCode = HashProxy.Combine(1, 2, 3, 4, 5, 6, 7, 8);
            secondCode = HashProxy.Combine(1, 2, 3, 4, 5, 6, 7, 8);
            bool eightValuesEqual = firstCode == secondCode;

            Assert.True(oneValueEqual);
            Assert.True(twoValuesEqual);
            Assert.True(threeValuesEqual);
            Assert.True(fourValuesEqual);
            Assert.True(fiveValuesEqual);
            Assert.True(sixValuesEqual);
            Assert.True(sevenValuesEqual);
            Assert.True(eightValuesEqual);
        }
    }
}
