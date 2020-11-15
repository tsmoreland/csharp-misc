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
using Moreland.CSharp.Util.Functional;
using ProjectResources = Moreland.CSharp.Util.Properties.Resources;

namespace Moreland.CSharp.Util
{
    /// <summary>
    /// Guard clauses to simplify guard clauses methods
    /// </summary>
    public static class GuardAgainst
    {
        /// <summary>
        /// Guard Check ensuring argument is not null
        /// </summary>
        /// <param name="argument">value to check</param>
        /// <param name="parameterName">name of parameter being checked</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="argument"/> is null
        /// </exception>
        public static void ArgumentBeingNull<T>(T? argument, string parameterName)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                parameterName = ProjectResources.UnknownParameterName;

            if (argument == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Guard check ensuring argument is not Empty as determined by IsEmpty method
        /// </summary>
        /// <typeparam name="TLeft">type parameter of Either</typeparam>
        /// <typeparam name="TRight">type parameter of Either</typeparam>
        /// <param name="argument">value to check</param>
        /// <param name="parameterName">name of parameter being checked</param>
        /// <exception cref="ArgumentException">
        /// if <paramref name="argument"/> is empty
        /// </exception>
        public static void ArgumentIsNullOrEmpty<TLeft, TRight>(Either<TLeft, TRight> argument, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                parameterName = ProjectResources.UnknownParameterName;
            if (argument.IsEmpty)
                throw new ArgumentException(ProjectResources.InvalidEither, parameterName);
        }

        /// <summary>
        /// Guard check ensuring argument is not Empty as determined by IsEmpty method
        /// </summary>
        /// <param name="argument">value to check</param>
        /// <param name="parameterName">name of parameter being checked</param>
        /// <exception cref="ArgumentException">
        /// if <paramref name="argument"/> is empty
        /// </exception>
        public static void ArgumentIsNullOrEmpty(string argument, string parameterName = "")
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                parameterName = ProjectResources.UnknownParameterName;

            if (string.IsNullOrEmpty(argument))
                throw new ArgumentException(ProjectResources.EmptyNotAllowed, parameterName);
        }
    }
}
