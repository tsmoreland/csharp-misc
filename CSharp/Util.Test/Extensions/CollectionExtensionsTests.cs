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
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Moreland.CSharp.Extensions;

namespace Moreland.CSharp.Util.Test.Extensions
{
    public abstract class CollectionExtensionsTests<T>
    {
        private readonly Func<T> _builder;
        private ICollection<T> _collection = null!;

        protected CollectionExtensionsTests(Func<T> builder)
        {
            _builder = builder;
        }

        [SetUp]
        public void Setup()
        {
            _collection = Substitute.For<ICollection<T>>();
        }

        [Test]
        public void AddRange_Params_ThrowsArgumentNullException_WhenCollectionIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => ((ICollection<T>)null!).AddRange(_builder(), _builder()));
            Assert.That(ex.ParamName, Is.EqualTo("collection"));
        }

        [Test]
        public void AddRange_IEnumerable_ThrowsArgumentNullException_WhenCollectionIsNull()
        {
            IEnumerable<T> enumerable = ListExtensions.Of(_builder(), _builder());
            var ex = Assert.Throws<ArgumentNullException>(() => ((ICollection<T>)null!).AddRange(enumerable));
            Assert.That(ex.ParamName, Is.EqualTo("collection"));
        }

        [Test]
        public void AddRange_IEnumerable_ThrowsArgumentNullException_WhenItemsIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _collection.AddRange((IEnumerable<T>)null!));
            Assert.That(ex.ParamName, Is.EqualTo("items"));
        }

        [Test]
        public void AddRange_Params_CallsAdd_ForAllGivenItems()
        {
            var item1 = _builder();
            var item2 = _builder();
            var item3 = _builder();

            _collection.AddRange(item1, item2, item3);

            _collection.Received(1).Add(item1);
            _collection.Received(1).Add(item2);
            _collection.Received(1).Add(item3);
        }

        [Test]
        public void AddRange_Params_DoesNotCallAdd_WhenNotGivenItems()
        {
            _collection.AddRange();

            _collection.Received(0).Add(Arg.Any<T>());
        }

        [Test]
        public void AddRange_IEnumerable_CallsAdd_ForAllGivenItems()
        {
            var item1 = _builder();
            var item2 = _builder();
            var item3 = _builder();
            IEnumerable<T> enumerable = ListExtensions.Of(item1, item2, item3);

            _collection.AddRange(enumerable);

            _collection.Received(1).Add(item1);
            _collection.Received(1).Add(item2);
            _collection.Received(1).Add(item3);
        }

        [Test]
        public void AddRange_IEnumerable_DoesNotCallAdd_WhenNotGivenItems()
        {
            IEnumerable<T> enumerable = ListExtensions.Of<T>();
            _collection.AddRange(enumerable);

            _collection.Received(0).Add(Arg.Any<T>());
        }
    }

    [TestFixture]
    public sealed class ValueCollectionExtensionsTests : CollectionExtensionsTests<int>
    {
        public ValueCollectionExtensionsTests()
            : base(() => TestData.RandomValueFactory.BuildRandomInt32())
        {
            
        }
    }

    [TestFixture]
    public sealed class EquatableReferenceCollectionExtensionsTests : CollectionExtensionsTests<string>
    {
       public EquatableReferenceCollectionExtensionsTests()
           : base(() => TestData.RandomValueFactory.BuildRandomString())
       {
       }
    }

    [TestFixture]
    public sealed class ReferenceCollectionExtensionsTests : CollectionExtensionsTests<List<string>>
    {
       public ReferenceCollectionExtensionsTests()
           : base(() => TestData.RandomValueFactory.BuildRandomListOfString())
       {
       }
    }
}
