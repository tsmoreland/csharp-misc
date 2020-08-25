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
using System.Linq;
using Moq;
using Xunit;

namespace Moreland.CSharp.Util.Test.Extensions
{
    public sealed class EnumerableTests
    {
        [Fact]
        public void AsOrToArrayDoesNotCreateNewArrayWhenAlreadyArray()
        {
            // Arrange
            IEnumerable<Guid> uids = new [] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            // Act
            var arrayOfUids = uids.AsOrToArray();

            // Assert
            Assert.Same(uids, arrayOfUids);
        }

        [Fact]
        public void AsOrToArrayDoesCreateNewArrayWhenNotArray()
        {
            // Arrange
            IEnumerable<Guid> uids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            // Act
            var arrayOfUids = uids.AsOrToArray();

            // Assert
            Assert.Equal(uids.ToArray(), arrayOfUids);
        }

        [Fact]
        public void AsOrToArray_ThrowsArgumentNullException_WhenEnumerabeIsNull()
        {
            // Arrange
            IEnumerable<Guid> uids = null!;

            // Act / Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _ = uids.AsOrToArray());
            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void AsOrToListDoesNotCreateNewListWhenAlreadyList()
        {
            // Arrange
            IEnumerable<Guid> uids = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            // Act
            var listOfUids = uids.AsOrToList();

            // Assert
            Assert.Same(uids, listOfUids);
        }

        [Fact]
        public void AsOrToListDoesCreateNewListWhenNotList()
        {
            // Arrange
            IEnumerable<Guid> uids = new [] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            // Act
            var listOfUids = uids.AsOrToList();

            // Assert
            Assert.Equal(uids, listOfUids);
        }

        [Fact]
        public void AsOrToList_ThrowsArgumentNullException_WhenEnumerableIsNull()
        {
            // Arrange
            IEnumerable<Guid> uids = null!;

            // Act / Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _ = uids.AsOrToList());
            Assert.Equal("source", ex.ParamName);

        }

        [Fact]
        public void ForEachThrowsArgumentNullIfItemsAreNull()
        {
            IEnumerable<int> items = null!;

            var exception = Assert.Throws<ArgumentNullException>(() => items.ForEach((value) => { }));
            Assert.Contains($"'{nameof(items)}'", exception.Message);
        }
        [Fact]
        public void ForEachThrowsArgumentNullIfConsumerIsNull()
        {
            var items = (new List<int>() { 1, 2, 3 }).AsEnumerable();
            var exception = Assert.Throws<ArgumentNullException>(() => items.ForEach(null!));
            Assert.Contains("'consumer'", exception.Message);
        }

        [Fact]
        public void ForEachIteratesOverEachElement()
        {
            var items = (new List<int>() { 1, 2, 3 }).AsEnumerable();
            var mock = new Mock<object>();
            mock
                .Setup(o => o.ToString())
                .Returns(string.Empty);

            items.ForEach(value => { _ = value + mock.Object.ToString(); });

            mock.Verify(o => o.ToString(), Times.Exactly(items.Count()));
        }
    }
}
