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

#if !(NET40 || NET45)
using System;
#endif

namespace Moreland.CSharp.Util.Collections
{
    /// <summary>
    /// Fluent Api for Array
    /// </summary>
    public static class FluentArray
    {
        /// <summary>
        /// Returns an Empty array, same functionality as System.Array.Empty{T} available in NET461
        /// but supported in NET40 and NET45
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue[] Empty<TValue>() =>
#if NET40 || NET45
            new TValue[0];
#else
            Array.Empty<TValue>();
#endif

        /// <summary>
        /// Constructs a new array from <paramref name="values"/>
        /// </summary>
        /// <typeparam name="TValue">Element type of the array</typeparam>
        /// <param name="values">values that make up the new array</param>
        /// <returns><paramref name="values"/></returns>
        /// <remarks>Provided for readability purposes as a simple way to construct an array</remarks>
        public static TValue[] Of<TValue>(params TValue[] values) =>
            values;
    }
}
