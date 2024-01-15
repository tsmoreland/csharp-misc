//
// Copyright (c) 2024 Terry Moreland
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

using System.Runtime.InteropServices;

namespace NetAotLogger;

public class AotLogger
{
    private static readonly Dictionary<string,  LogLevel> s_logLevelByName = [];
    private const int Success = 0;
    private const int ExceptionalError = -1;
    private const int ArgumentNull = -2;

    [UnmanagedCallersOnly(EntryPoint = "log_info")]
    public static int LogInfo(IntPtr stringPtr)
    {
        try
        {
            string? message;
            if (stringPtr == IntPtr.Zero || (message = Marshal.PtrToStringAnsi(stringPtr)) is null)  
            {
                return ArgumentNull;
            }

            Console.WriteLine(message);

            return Success;
        }
        catch
        {
            return ExceptionalError;
        }
    }

}