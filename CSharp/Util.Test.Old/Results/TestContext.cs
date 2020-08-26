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

using Moq;
using System;
using Moreland.CSharp.Util.Results;
using Xunit;

namespace Moreland.CSharp.Util.Test.Old.Results
{
    public delegate IValueResult<T> SuccessResultFactory<T>(T value);
    public delegate IValueResult<T> FailureResultFactory<T>(string message);

    public delegate TestContext<T> TestContextFactory<T>();

    public static class TestContext
    {
        public static TestContext<TValue> BuildQueryContext<TValue>() => new TestContext<TValue>(
            (value) => QueryResult.Ok<TValue>(value),
            (message) => QueryResult.Failed<TValue>(message)
        );
        public static TestContext<TValue> BuildCommandContext<TValue>() => new TestContext<TValue>(
            (value) => CommandResult.Ok<TValue>(value),
            (message) => CommandResult.Failed<TValue>(message)
        );
        public static void SucessfulResult_FlatMapInvoked(TestContextFactory<Guid> factory)
        {
            Guid value = Guid.NewGuid();
            factory()
                .ArrangeWithSuccess(value)
                .ActAndAssertWithFlatMap_VerifyFlatMapperInvoked(value.ToString());
        }

        public static void SucessfulResult_FlatMapAppliedResultSuccess(TestContextFactory<Guid> factory)
        {
            // Arrange
            Guid value = Guid.NewGuid();
            factory()
                .ArrangeWithSuccess(value)
                .ActAndAssertWithFlatMap_VerifyExpectedResult(value.ToString());
        }
        public static void SucessfulResult_MapInvoked(TestContextFactory<Guid> factory)
        {
            Guid value = Guid.NewGuid();
            factory()
                .ArrangeWithSuccess(value)
                .ActAndAssertWithMap_VerifyMapperInvoked(value.ToString());
        }

        public static void SucessfulResult_MapAppliedResultSuccess(TestContextFactory<Guid> factory)
        {
            // Arrange
            Guid value = Guid.NewGuid();
            factory()
                .ArrangeWithSuccess(value)
                .ActAndAssertWithMap_VerifyExpectedResult(value.ToString());
        }
        public static void SucessfulResult_OrElseNotUsed(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseValue(Guid.NewGuid())
                .ActWithOrElseOther()
                .AssertExpectedResult();
        }
        public static void SucessfulResult_OrElseGetOtherNotInvoked(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseValue(Guid.NewGuid())
                .ActWithOrElseGet()
                .AssertOrElseInvoked();
        }

        public static void SucessfulResult_OrElseGetNotUsedValueMatches(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseValue(Guid.NewGuid())
                .ActWithOrElseGet()
                .AssertExpectedResult();
        }
        public static void SucessfulResult_OrElseThrowDoesNotThrow(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseThrow(new NotImplementedException())
                .ActAndAssertWithOrElseThrow<NotImplementedException>();
        }
        public static void SucessfulResult_OrElseThrowOverloadDoesNotThrow(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseThrow("not implemented", new NotImplementedException())
                .ActAndAssertWithOrElseThrow<NotImplementedException>("not implemented");
        }
        public static void SucessfulResult_OrElseThrowDoesValueMatches(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseThrow(new NotImplementedException())
                .ActAndAssertWithOrElseThrow<NotImplementedException>();
        }
        public static void SucessfulResult_OrElseThrowOverloadDoesValueMatches(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithSuccess(Guid.NewGuid())
                .ConfigureOrElseThrow("message", new NotImplementedException())
                .ActAndAssertWithOrElseThrow<NotImplementedException>("not implemented");
        }
        public static void FailureResult_OrElseThrowThrowsArgumentNullExceptionIfSupplierIsNull(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure(Guid.NewGuid().ToString())
                .ActAndAssertWithOrElseThrow<NotSupportedException>();
        }

        public static void FailedResult_FlatMapThrowsArgumentNullWhenMapperIsNull(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ActAndAssertWithFlatMap_ThrowsArgumentNullIfMapperNull<string>();
        }
        public static void FailedResult_FlatMapNotApplied(TestContextFactory<Guid> factory)
        {
            Guid value = Guid.NewGuid();
            factory()
                .ArrangeWithFailure("error")
                .ActAndAssertWithFlatMap_VerifyFlatMapperInvoked(value.ToString());
        }
        public static void FailedResult_MapThrowsArgumentNullWhenMapperIsNull(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ActAndAssertWithMap_ThrowsArgumentNullIfMapperNull<string>();
        }
        public static void FailedResult_MapNotApplied(TestContextFactory<Guid> factory)
        {
            Guid value = Guid.NewGuid();
            factory()
                .ArrangeWithFailure("error")
                .ActAndAssertWithMap_VerifyMapperInvoked(value.ToString());
        }
        public static void FailedResult_OrElseUsed(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ConfigureOrElseValue(Guid.NewGuid())
                .ActWithOrElseOther()
                .AssertExpectedResult();
        }
        public static void FailedResult_OrElseGetUsedOtherInvoked(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ConfigureOrElseValue(Guid.NewGuid())
                .ActWithOrElseGet()
                .AssertOrElseInvoked();
        }
        public static void FailedResult_OrElseGetUsedValueMatches(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ConfigureOrElseValue(Guid.NewGuid())
                .ActWithOrElseGet()
                .AssertOrElseInvoked();
        }

        public static void FailedResult_OrElseThrowThrows(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ConfigureOrElseThrow(new NotImplementedException())
                .ActAndAssertWithOrElseThrow<NotImplementedException>();
        }
        public static void FailedResult_OrElseThrowOverloadThrows(TestContextFactory<Guid> factory)
        {
            var message = Guid.NewGuid().ToString();
            factory()
                .ArrangeWithFailure(message)
                .ConfigureOrElseThrow(message, new NotImplementedException())
                .ActAndAssertWithOrElseThrow<NotImplementedException>(message);
        }
        public static void FailedResult_OrElseThrowThrowsArgumentNullWhenSupplierIsNull(TestContextFactory<Guid> factory)
        {
            factory()
                .ArrangeWithFailure("error")
                .ActAndAssertWithOrElseThrow<NotImplementedException>();
        }
    }

    public class TestContext<T>
    {
        public TestContext(SuccessResultFactory<T> successFactory, FailureResultFactory<T> failureFactory)
        {
            SuccessFactory = successFactory;
            FailureFactory = failureFactory;
        }


        public IValueResult<T> Result { get; private set; } = null!;
        public TestContext<T> ArrangeWithSuccess(T value)
        {
            Result = SuccessFactory(value);
            _expectedOrElseValue = Maybe.Of(value);
            return this;
        }
        public TestContext<T> ArrangeWithFailure(string message)
        {
            Result = FailureFactory(message);
            return this;
        }
        public TestContext<T> ConfigureOrElseValue(T @else)
        {
            OrElseGetFunc = new Mock<Func<T>>();
            OrElseGetFunc
                .Setup(o => o.Invoke())
                .Returns(@else);
            if (!Result.Success)
                _expectedOrElseValue = Maybe.Of(@else);
            return this;
        }
        public TestContext<T> ConfigureOrElseThrow<TException>(TException exceptionToThrow) where TException : Exception
        {
            OrElseThrowFunc = new Mock<Func<Exception>>();
            OrElseThrowFunc
                .Setup(o => o.Invoke())
                .Returns(exceptionToThrow);
            return this;
        }
        public TestContext<T> ConfigureOrElseThrow<TException>(string message, TException exceptionToThrow) where TException : Exception
        {
            OrElseThrowFuncOverload = new Mock<Func<string, Exception?, Exception>>();
            OrElseThrowFuncOverload
                .Setup(o => o.Invoke(message, It.IsAny<Exception?>()))
                .Returns(exceptionToThrow);
            return this;
        }
        public void ActAndAssertWithFlatMap_ThrowsArgumentNullIfMapperNull<U>()
        {
            if (Result is CommandResult<T> cmd)
                Assert.Throws<ArgumentNullException>(() => _ = cmd.FlatMap<U>(null!));
            else if (Result is QueryResult<T> query)
                Assert.Throws<ArgumentNullException>(() => _ = query.FlatMap<U>(null!));
            else
                throw new NotSupportedException($"test for {Result.GetType()} not implemented");
        }
        public void ActAndAssertWithFlatMap_VerifyFlatMapperInvoked<U>(U FlatMappedValue)
        {

            if (Result is CommandResult<T> cmd)
            {
                var FlatMapper = new Mock<Func<T, CommandResult<U>>>();
                FlatMapper
                    .Setup(o => o.Invoke(It.IsAny<T>()))
                    .Returns(CommandResult.Ok(FlatMappedValue));
                _ = cmd.FlatMap(FlatMapper.Object);

                if (Result.Success)
                    FlatMapper.Verify(m => m.Invoke(It.IsAny<T>()), Times.Once);
                else
                    FlatMapper.Verify(m => m.Invoke(It.IsAny<T>()), Times.Never);
            }
            else if (Result is QueryResult<T> query)
            {
                var FlatMapper = new Mock<Func<T, QueryResult<U>>>();
                FlatMapper
                    .Setup(o => o.Invoke(It.IsAny<T>()))
                    .Returns(QueryResult.Ok(FlatMappedValue));
                _ = query.FlatMap(FlatMapper.Object);

                if (Result.Success)
                    FlatMapper.Verify(m => m.Invoke(It.IsAny<T>()), Times.Once);
                else
                    FlatMapper.Verify(m => m.Invoke(It.IsAny<T>()), Times.Never);
            }
            else
                throw new NotSupportedException($"test for {Result.GetType()} not implemented");

        }
        public void ActAndAssertWithMap_ThrowsArgumentNullIfMapperNull<U>()
        {
            if (Result is CommandResult<T> cmd)
                Assert.Throws<ArgumentNullException>(() => _ = cmd.Map<U>(null!));
            else if (Result is QueryResult<T> query)
                Assert.Throws<ArgumentNullException>(() => _ = query.Map<U>(null!));
            else
                throw new NotSupportedException($"test for {Result.GetType()} not implemented");
        }
        public void ActAndAssertWithFlatMap_VerifyExpectedResult<U>(U FlatMappedValue)
        {

            bool success;
            U FlatMapped;
            if (Result is CommandResult<T> cmd)
            {
                var FlatMapper = new Mock<Func<T, CommandResult<U>>>();
                FlatMapper
                    .Setup(o => o.Invoke(It.IsAny<T>()))
                    .Returns(CommandResult.Ok(FlatMappedValue));
                (success, FlatMapped) = cmd.FlatMap(FlatMapper.Object);

                if (Result.Success)
                    Assert.Equal(FlatMappedValue, FlatMapped);
                else
                    Assert.False(success);
            }
            else if (Result is QueryResult<T> query)
            {
                var FlatMapper = new Mock<Func<T, QueryResult<U>>>();
                FlatMapper
                    .Setup(o => o.Invoke(It.IsAny<T>()))
                    .Returns(QueryResult.Ok(FlatMappedValue));
                (success, FlatMapped) = query.FlatMap(FlatMapper.Object);

                if (Result.Success)
                    Assert.Equal(FlatMappedValue, FlatMapped);
                else
                    Assert.False(success);
            }
            else
                throw new NotSupportedException($"test for {Result.GetType()} not implemented");
        }
        public void ActAndAssertWithMap_VerifyMapperInvoked<U>(U mappedValue)
        {
            var mapper = new Mock<Func<T, U>>();
            mapper
                .Setup(o => o.Invoke(It.IsAny<T>()))
                .Returns(mappedValue);

            if (Result is CommandResult<T> cmd)
                _ = cmd.Map(mapper.Object);
            else if (Result is QueryResult<T> query)
                _ = query.Map(mapper.Object);
            else
                throw new NotSupportedException($"test for {Result.GetType()} not implemented");

            if (Result.Success)
                mapper.Verify(m => m.Invoke(It.IsAny<T>()), Times.Once);
            else
                mapper.Verify(m => m.Invoke(It.IsAny<T>()), Times.Never);
        }
        public void ActAndAssertWithMap_VerifyExpectedResult<U>(U mappedValue)
        {
            var mapper = new Mock<Func<T, U>>();
            mapper
                .Setup(o => o.Invoke(It.IsAny<T>()))
                .Returns(mappedValue);

            bool success;
            U mapped;
            if (Result is CommandResult<T> cmd)
                (success, mapped) = cmd.Map(mapper.Object);
            else if (Result is QueryResult<T> query)
                (success, mapped) = query.Map(mapper.Object);
            else
                throw new NotSupportedException($"test for {Result.GetType()} not implemented");

            if (Result.Success)
                Assert.Equal(mappedValue, mapped);
            else
                Assert.False(success);
        }

        public TestContext<T> ActAndAssertWithOrElseThrow_VerifyValue<TException>() where TException : Exception
        {
            if (OrElseThrowFunc == null)
            {
                Assert.True(false, "OrElseThrow not set, please configure this method before calling act");
                return this;
            }

            if (Result.Success)
            {
                _actualOrElseValue = Maybe.Of(Result.OrElseThrow(OrElseThrowFunc.Object));
                return AssertExpectedResult();
            }
            else
            {
                Assert.Throws<TException>(() => Result.OrElseThrow(OrElseThrowFunc.Object));

            }

            return this;
        }
        public TestContext<T> ActAndAssertWithOrElseThrow<TException>() where TException : Exception
        {
            if (Result.Success && OrElseThrowFunc != null)
            {
                _actualOrElseValue = Maybe.Of(Result.OrElseThrow(OrElseThrowFunc.Object));
                OrElseThrowFunc.Verify(f => f.Invoke(), Times.Never);
                return AssertExpectedResult();
            }
            else
            {
                if (OrElseThrowFunc != null)
                {
                    Assert.Throws<TException>(() => Result.OrElseThrow(OrElseThrowFunc.Object));
                    OrElseThrowFunc.Verify(f => f.Invoke(), Times.Once);
                }
                else
                {
                    Assert.Throws<ArgumentNullException>(() => Result.OrElseThrow((Func<Exception>)null!));
                    Assert.Throws<ArgumentNullException>(() => Result.OrElseThrow((Func<string, Exception?, Exception>)null!));
                }
                return this;
            }
        }
        public TestContext<T> ActAndAssertWithOrElseThrow<TException>(string message) where TException : Exception
        {
            if (OrElseThrowFuncOverload == null)
            {
                Assert.True(false, "OrElseThrow not set, please configure this method before calling act");
                return this;
            }

            if (Result.Success)
            {
                _actualOrElseValue = Maybe.Of(Result.OrElseThrow(OrElseThrowFuncOverload.Object));
                OrElseThrowFuncOverload.Verify(f => f.Invoke(It.IsAny<string>(), It.IsAny<Exception?>()), Times.Never);
                return AssertExpectedResult();
            }
            else
            {
                Assert.Throws<TException>(() => Result.OrElseThrow(OrElseThrowFuncOverload.Object));
                OrElseThrowFuncOverload.Verify(f => f.Invoke(message, It.IsAny<Exception?>()), Times.Once);
                return this;
            }

        }
        public TestContext<T> ActWithOrElseOther()
        {
            _actualOrElseValue = Maybe.Of(Result.OrElseOther(_expectedOrElseValue.Value));
            return this;
        }
        public TestContext<T> ActWithOrElseGet()
        {
            if (OrElseGetFunc == null)
            {
                Assert.True(false, "OrElseGet not set, please configure this method before calling act");
                return this;
            }

            _actualOrElseValue = Maybe.Of(Result.OrElseGet(OrElseGetFunc.Object));
            return this;
        }

        public TestContext<T> AssertOrElseInvoked()
        {
            Func<Times> times;
            if (Result.Success)
                times = Times.Never;
            else
                times = Times.Once;

            if (OrElseGetFunc != null)
                OrElseGetFunc.Verify(o => o.Invoke(), times);
            else if (OrElseThrowFunc != null)
                OrElseThrowFunc.Verify(o => o.Invoke(), times);
            else
                Assert.True(false, "no OrElse.. methods were configured");

            return this;
        }
        public TestContext<T> AssertExpectedResult()
        {
            Assert.Equal(_expectedOrElseValue, _actualOrElseValue);
            return this;
        }

        private Maybe<T> _actualOrElseValue = Maybe.Empty<T>();
        private Maybe<T> _expectedOrElseValue = Maybe.Empty<T>();
        private Mock<Func<T>>? OrElseGetFunc { get; set; }  
        private Mock<Func<Exception>>? OrElseThrowFunc { get; set; }  
        private Mock<Func<string, Exception?, Exception>>? OrElseThrowFuncOverload { get; set; }  
        private SuccessResultFactory<T> SuccessFactory { get; }
        private FailureResultFactory<T> FailureFactory { get; }
    }
}
