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
using System.Diagnostics.CodeAnalysis;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util.Functional
{
    /// <inheritdoc/>
    public sealed class LeftEither<TLeft, TRight> : Either<TLeft, TRight>
    {
        /// <summary>
        /// Instantiates a new instance of the <see cref="LeftEither{TLeft,TRight}"/> class.
        /// </summary>
        /// <param name="value">value to wrap</param>
        public LeftEither(TLeft value)
        {
            Value = value;
        }

        /// <summary>
        /// Contained Value 
        /// </summary>
        public TLeft Value { get; }

        /// <summary>
        /// Implicitly converts either to its contained value
        /// </summary>
        /// <param name="either">object containing value to return</param>
        /// <exception cref="ArgumentException">
        /// If either is null, while it is generally considered bad practise to throw an exception from implicit one
        /// would occur anyway in this case, the only difference is null reference vs. argument exception
        /// </exception>
        [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Provided by static Either class")]
        public static implicit operator TLeft(LeftEither<TLeft, TRight> either) =>
            either
                .ToLeftValue()
                .ValueOrThrow(() => new ArgumentException(ProjectResources.BadEitherAccessLeft));
    }
}
