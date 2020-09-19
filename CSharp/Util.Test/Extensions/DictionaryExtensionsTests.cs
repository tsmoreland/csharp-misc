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
using NSubstitute;
using NUnit.Framework;

namespace Moreland.CSharp.Util.Test.Extensions
{
    public abstract class DictionaryExtensionsTests<TKey, TValue> where TKey : notnull
    {
        private readonly Func<TKey> _keyBuilder;
        private readonly Func<TValue> _valueBuilder;
        private Dictionary<TKey, TValue> _dictionary;

        protected DictionaryExtensionsTests(Func<TKey> keyBuilder, Func<TValue> valueBuilder)
        {
            _keyBuilder = keyBuilder;
            _valueBuilder = valueBuilder;
        }

        [SetUp]
        public void Setup()
        {
            _dictionary = Substitute.For<Dictionary<TKey, TValue>>();
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenFirstIsNull()
        {
            Dictionary<TKey, TValue> first = null!;
            Dictionary<TKey, TValue> second = BuildDictionary();

            var ex = Assert.Throws<ArgumentNullException>(() => _ = first.Union(second));
            Assert.That(ex.ParamName, Is.EqualTo("first"));
        }

        [Test]
        public void Union_ThrowsArgumentNullException_WhenSecondIsNull()
        {
            Dictionary<TKey, TValue> first = BuildDictionary();
            Dictionary<TKey, TValue> second = null!;

            var ex = Assert.Throws<ArgumentNullException>(() => _ = first.Union(second));
            Assert.That(ex.ParamName, Is.EqualTo("second"));
        }

        private Dictionary<TKey, TValue> BuildDictionary()
        {
            return new Dictionary<TKey, TValue> 
            { 
                {_keyBuilder(), _valueBuilder() },
                {_keyBuilder(), _valueBuilder() },
                {_keyBuilder(), _valueBuilder() },
            };
        }
    }

    [TestFixture]
    public sealed class ValueDictionaryExtensionsTests : DictionaryExtensionsTests<int, int>
    {
        public ValueDictionaryExtensionsTests()
            : base(() => TestData.RandomValueFactory.BuildRandomInt32(), () => TestData.RandomValueFactory.BuildRandomInt32())
        {
            
        }
    }

    [TestFixture]
    public sealed class EquatableReferenceDictionaryExtensionsTests : DictionaryExtensionsTests<int, string>
    {
       public EquatableReferenceDictionaryExtensionsTests()
           : base(() => TestData.RandomValueFactory.BuildRandomInt32(), () => TestData.RandomValueFactory.BuildRandomString())
       {
       }
    }

    [TestFixture]
    public sealed class ReferenceDictionaryExtensionsTests : DictionaryExtensionsTests<string, List<string>>
    {
       public ReferenceDictionaryExtensionsTests()
           : base(() =>TestData.RandomValueFactory.BuildRandomString(), () => TestData.RandomValueFactory.BuildRandomListOfString())
       {
       }
    }
}
