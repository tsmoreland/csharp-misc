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
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Moreland.CSharp.Util.Test.TestData
{
    public static class RandomValueFactory
    {
        private static readonly Lazy<RandomNumberGenerator> _generator =
            new Lazy<RandomNumberGenerator>(RandomNumberGenerator.Create);
        private static RandomNumberGenerator Genertor => _generator.Value;

        public static int BuildRandomInt32(params int[] exclusions) =>
            Build(buffer => BitConverter.ToInt32(buffer, 0), exclusions);
        public static long BuildRandomInt64(params long[] exclusions) =>
            Build(buffer => BitConverter.ToInt64(buffer, 0), exclusions);

        public static Guid BuildRandomGuid(params Guid[] exclusions)
        {
            Guid value;
            do
            {
                value = Guid.NewGuid();
            } while (exclusions.Contains(value));
            return value;
        }
        public static String BuildRandomString(params string[] exclusions)
        {
            String value;
            do
            {
                value = $"s{Guid.NewGuid():N}-{Guid.NewGuid():N}";
            } while (exclusions.Contains(value));
            return value;
        }

        public static List<string> BuildRandomListOfString(int maxSize = 5)
        {
            int size = Math.Abs(BuildRandomInt32()) % maxSize;
            var list = new List<string>();
            for (int i = 0; i < size; i++)
                list.Add(BuildRandomString());
            return list;
        }

        private static T Build<T>(Func<byte[], T> converter, params T[] exclusions) where T : struct
        {
            byte[] buffer = new byte[Marshal.SizeOf<T>()];

            T value;
            do
            {
                Genertor.GetBytes(buffer);
                value = converter(buffer);

            } while (exclusions.Contains(value));

            return value;
        }
    }
}
