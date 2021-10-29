//
// Copyright © 2021 Terry Moreland
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
using System.Diagnostics;
using System.IO;

namespace TSMoreland.Utilities.FileUtilities;

public static class LockUtilities
{

    public static bool IsFileLocked(string filename)
    {
        return IsFileLocked(new FileInfo(filename));
    }

    /// <summary>
    /// Determines if File is locked
    /// </summary>
    /// <param name="file">file to check</param>
    /// <returns>
    /// <see langword="true"/> if file is locked; otherwise <see langword="false"/>
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// if file does not exist
    /// </exception>
    public static bool IsFileLocked(this FileInfo file)
    {
        if (file == null!)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (OperatingSystem.IsWindows())
        {
            return Win32.Win32LockUtilities.IsFileLocked(file);
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            return Linux.LinuxLockUtilities.IsFileLocked(file);
        }

        throw new NotSupportedException("Not supported on this platform");
    }

    /// <summary>
    /// Returns a list of processes currently locking <paramref name="file"/>
    /// </summary>
    /// <param name="file">file to get locking processes for</param>
    /// <returns><see cref="IList{Process}"/> </returns>
    public static IList<Process> GetProcessesLockingFile(this FileInfo file)
    {
        if (file == null!)
        {
            throw new ArgumentNullException(nameof(file));
        }

        if (OperatingSystem.IsWindows())
        {
            return Win32.Win32LockUtilities.GetProcessesLockingFile(file);
        }

        throw new NotSupportedException("Not supported on this platform");
    }
}