﻿//
// Copyright © 2022 Terry Moreland
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
using TSMoreland.Interop.App;

if (!OperatingSystem.IsWindows())
{
    Console.WriteLine("Application not supported on any platform other than Windows");
    return;
}

try
{
    Console.WriteLine("Presss enter to begin");
    Console.ReadLine();

    Console.WriteLine("Test for leaks against manual callsite usage");
    OutOfProcessTest.VerifyMemoryUse();

    Console.WriteLine("Presss enter to begin phase II");
    Console.ReadLine();

    Console.WriteLine("Test for leaks against dynamic usage");
    OutOfProcessTest.VerifyMemoryUseWithDynamic();
}
catch (Exception ex)
{
    Console.WriteLine("error occurred testing in process COM: " + ex);
}

#if VERIFY
try
{
    InProcessTest.Verify();
}
catch (Exception ex)
{
    Console.WriteLine("error occurred testing in process COM: " + ex);
}


try
{
    OutOfProcessTest.Verify();
}
catch (Exception ex)
{
    Console.WriteLine("error occurred testing out of process COM: " + ex);
}

#endif
