﻿//
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
using Moreland.CSharp.Util.Results;
using NSubstitute;

namespace Moreland.CSharp.Util.Test.Results
{
    internal class QueryResultTestHelper<T> : IValueResultTestHelper<T>
    {
        public bool ImplicitBool(IValueResult<T> genericResult)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");
            return result;
        }

        public T ExplicitValue(IValueResult<T> genericResult)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");

            return (T)result;
        }

        public IValueResult<T> OkBuilder(T value) =>
            QueryResult.Ok(value);
        public IValueResult<TOther> OkBuilder<TOther>(TOther value) =>
            QueryResult.Ok(value);
        public IValueResult<TOther> OkWithMessageBuilder<TOther>(TOther value, string message) =>
            QueryResult.Ok(value, message);
        public IValueResult<T> OkWithMessageBuilder(T value, string message) =>
            QueryResult.Ok(value, message);

        public IValueResult<TOther> FailedBuilder<TOther>(string message) =>
            QueryResult.Failed<TOther>(message);
        public IValueResult<T> FailedBuilder(string message) =>
            QueryResult.Failed<T>(message);
        public IValueResult<TOther> FailedWithCauseBuilder<TOther>(string message, Exception? cause) =>
            QueryResult.Failed<TOther>(message, cause);
        public IValueResult<T> FailedWithCauseBuilder(string message, Exception? cause) =>
            QueryResult.Failed<T>(message, cause);


        public bool ObjectEquals(IValueResult<T> genericFirst, object? second)
        {
            if (!(genericFirst is QueryResult<T> first))
                throw new InvalidOperationException("Unexpected type");

            return first.Equals(second);
        }
        public bool EquatableEquals(IValueResult<T> genericFirst, IValueResult<T> genericSecond)
        {
            if (!(genericFirst is QueryResult<T> first) || !(genericSecond is QueryResult<T> second))
                throw new InvalidOperationException("Unexpected type");

            return first.Equals(second);
        }

        public bool OperatorEquals(IValueResult<T> genericFirst, IValueResult<T> genericSecond)
        {
            if (!(genericFirst is QueryResult<T> first) || !(genericSecond is QueryResult<T> second))
                throw new InvalidOperationException("Unexpected type");

            return first == second;
        }
        public bool OperatorNotEquals(IValueResult<T> genericFirst, IValueResult<T> genericSecond)
        {
            if (!(genericFirst is QueryResult<T> first) || !(genericSecond is QueryResult<T> second))
                throw new InvalidOperationException("Unexpected type");

            return first != second;
        }

        public IValueResult<TMapped> Select<TMapped>(IValueResult<T> genericResult, Func<T, TMapped> genericSelector)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");
            return result.Select(genericSelector);
        }

        public IValueResult<TMapped> Select<TMapped>(IValueResult<T> genericResult, Func<T, IValueResult<TMapped>> genericSelector)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");
            return genericSelector != null!
                ? result.Select(Selector)
                : result.Select((Func<T, QueryResult<TMapped>>)null!);

            QueryResult<TMapped> Selector(T value) => 
                (QueryResult<TMapped>)genericSelector(value);
        }

        public IValueResult<T> ValueOrSupplied(IValueResult<T> genericResult, string messsage, Exception? cause, T @else)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");

            var supplier = Substitute.For<Func<string, Exception?, QueryResult<T>>>();
            supplier.Invoke(messsage, cause).Returns(QueryResult.Ok(@else));

            return result.ValueOr(supplier);
        }
        public void ValueOrNullSupplier(IValueResult<T> genericResult)
        {
            if (!(genericResult is QueryResult<T> result))
                throw new InvalidOperationException("Unexpected type");

            Func<string, Exception?, QueryResult<T>> supplier = null!;
            _ = result.ValueOr(supplier);
        }
    }
}
