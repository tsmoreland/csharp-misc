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

#if NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_1
using System;

namespace Moreland.CSharp.Util.Modernizer
{
    /// <summary>
    /// Placeholder for matching type in dotnet core 3.1+ to allow usage in earlier versions, it won't be
    /// functional in these cases and is intended soley to reduce code size by allowing the attribute to still be used
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>Initializes a new instace of <see cref="CallerArgumentExpressionAttribute"/> class.</summary>
        /// <param name="parameterName">
        /// For the type supported in dotnet core 3.1+ this would be the
        /// name of the targeted parameter
        /// </param>
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Storage for parameter name set in constructor, in dotnet core 3.1+ this would store the
        /// name of the parameter source 
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string ParameterName { get; }
    }
}
#endif
