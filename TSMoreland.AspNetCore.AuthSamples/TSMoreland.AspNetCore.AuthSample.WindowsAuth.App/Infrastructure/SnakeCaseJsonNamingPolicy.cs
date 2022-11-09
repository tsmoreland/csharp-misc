//
// Copyright (c) 2022 Terry Moreland
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

using System.Text;
using System.Text.Json;

namespace TSMoreland.AspNetCore.AuthSample.WindowsAuth.App.Infrastructure;

public sealed class SnakeCaseJsonNamingPolicy : JsonNamingPolicy
{
    private static readonly Lazy<SnakeCaseJsonNamingPolicy> s_instance = new(() => new SnakeCaseJsonNamingPolicy());
    public static SnakeCaseJsonNamingPolicy Instance => s_instance.Value;

    /// <inheritdoc />
    public override string ConvertName(string name)
    {
        if (name is not { Length: > 0 })
        {
            return name;
        }

        StringBuilder builder = new();

        ReadOnlySpan<char> asSpan = name.AsSpan();

        builder.Append(char.ToLower(asSpan[0]));

        for (int i = 1; i < asSpan.Length; i++)
        {
            if (char.IsUpper(asSpan[i]))
            {
                builder.Append('_');
                builder.Append(char.ToLower(asSpan[i]));
            }
            else
            {
                builder.Append(asSpan[i]);
            }
        }
        return builder.ToString();
    }
}
