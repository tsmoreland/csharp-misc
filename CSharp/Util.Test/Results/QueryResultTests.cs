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
using Moreland.CSharp.Util.Results;
using NUnit.Framework;
using static Moreland.CSharp.Util.Test.TestData.RandomValueFactory;

namespace Moreland.CSharp.Util.Test.Results
{
    internal static class QueryResultTestHelper
    {
        public static bool ImplicitBool<T>(IValueResult<T> genericResult)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");
            return result;
        }

        public static IValueResult<T> OkBuilder<T>(T value) =>
            QueryResult.Ok(value);
        public static IValueResult<T> OkWithMessageBuilder<T>(T value, string message) =>
            QueryResult.Ok(value, message);

        public static IValueResult<T> FailedBuilder<T>(string message) =>
            QueryResult.Failed<T>(message);
        public static IValueResult<T> FailedWithCauseBuilder<T>(string message, Exception? cause) =>
            QueryResult.Failed<T>(message, cause);
    }

    [TestFixture]
    public sealed class ValueTypeQueryResultTests : ValueResultTests<int>
    {
        public ValueTypeQueryResultTests()
            : base(
                () => BuildRandomInt32(),
                QueryResultTestHelper.OkBuilder,
                QueryResultTestHelper.OkWithMessageBuilder,
                QueryResultTestHelper.FailedBuilder<int>,
                QueryResultTestHelper.FailedWithCauseBuilder<int>,
                QueryResultTestHelper.ImplicitBool)
        {
        }

    }

    [TestFixture]
    public sealed class EquatableReferenceTypeQueryResultTests : ValueResultTests<string>
    {
        public EquatableReferenceTypeQueryResultTests()
            : base(
                () => BuildRandomString(),
                QueryResultTestHelper.OkBuilder,
                QueryResultTestHelper.OkWithMessageBuilder,
                QueryResultTestHelper.FailedBuilder<string>,
                QueryResultTestHelper.FailedWithCauseBuilder<string>,
                QueryResultTestHelper.ImplicitBool)
        {
        }
    }

    [TestFixture]
    public sealed class ReferenceTypeQueryResultTests : ValueResultTests<List<string>>
    {
        public ReferenceTypeQueryResultTests()
            : base(
                () => BuildRandomListOfString(),
                QueryResultTestHelper.OkBuilder,
                QueryResultTestHelper.OkWithMessageBuilder,
                QueryResultTestHelper.FailedBuilder<List<string>>,
                QueryResultTestHelper.FailedWithCauseBuilder<List<string>>,
                QueryResultTestHelper.ImplicitBool)
        {
        }
    }
}
