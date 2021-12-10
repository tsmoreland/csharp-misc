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

namespace Moreland.CSharp.Util.Extensions
{
    /// <summary>
    /// Extensions to Nullable types
    /// </summary>
    /// <remarks>
    /// This includes <see cref="Nullable{T}"/> as well as structs using the <c>?</c> operator or
    /// when nullable is enabled classes using <c>?</c>
    /// </remarks>
    public static class NullableExtensions
    {
        /// <summary>Returns true if <paramref name="value"/> is non-null</summary>
        public static bool HasValue<TValue>(this TValue? value) where TValue : class => value != null;

        /// <summary>Returns true if <paramref name="value"/> is non-null</summary>
        public static bool HasValue<TValue>(this TValue? value) where TValue : struct => value != null;

        /// <summary>simple wrapper around null coallese operator for improved readability</summary>
        /// <typeparam name="TValue">nullable class type</typeparam>
        /// <param name="value">value to return if non-null</param>
        /// <param name="or">alternate value which should not be null</param>
        /// <returns><paramref name="value"/> if non-null; otherwise, <paramref name="or"/></returns>
        /// <remarks>no null check is performed on <paramref name="or"/> so null may still be returned if that value is null</remarks>
        public static TValue ValueOr<TValue>(this TValue? value, TValue or) where TValue : class => value ?? or;

        /// <summary>simple wrapper around null coallese operator for improved readability</summary>
        /// <typeparam name="TValue">nullable value type</typeparam>
        /// <param name="value">value to return if non-null</param>
        /// <param name="or">alternate value which should not be null</param>
        /// <returns><paramref name="value"/> if non-null; otherwise, <paramref name="or"/></returns>
        /// <remarks>no null check is performed on <paramref name="or"/> so null may still be returned if that value is null</remarks>
        public static TValue ValueOr<TValue>(this TValue? value, TValue or) where TValue : struct => value ?? or;

        /// <summary>
        /// returns <paramref name="value"/> or value provided by <paramref name="supplier"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="supplier"/> is null if <paramref name="value"/> is not</exception>
        public static TValue ValueOr<TValue>(this TValue? value, Func<TValue> supplier) where TValue : class
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));
            return value ?? supplier.Invoke();
        }
        /// <summary>
        /// returns <paramref name="value"/> or value provided by <paramref name="supplier"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="supplier"/> is null if <paramref name="value"/> is not</exception>
        public static TValue ValueOr<TValue>(this TValue? value, Func<TValue> supplier) where TValue : struct
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));
            return value ?? supplier.Invoke();
        }

        /// <summary>
        /// returns <paramref name="value"/> if non-null or throws exception provided by <paramref name="exceptionSupplier"/>
        /// </summary>
        public static TValue ValueOrThrow<TValue>(this TValue? value, Func<Exception> exceptionSupplier) where TValue : class
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            return value ?? throw exceptionSupplier.Invoke();
        }

        /// <summary>
        /// returns <paramref name="value"/> if non-null or throws exception provided by <paramref name="exceptionSupplier"/>
        /// </summary>
        public static TValue ValueOrThrow<TValue>(this TValue? value, Func<Exception> exceptionSupplier) where TValue : struct
        {
            if (exceptionSupplier == null)
                throw new ArgumentNullException(nameof(exceptionSupplier));
            return value ?? throw exceptionSupplier.Invoke();
        }
    }
}
