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
using System.Linq;
using System.Runtime.Versioning;

namespace TSMoreland.Utilities.FileUtilities.Win32;

#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
internal static class Win32LockUtilities
{

    /// <inheritdoc cref="LockUtilities.IsFileLocked(FileInfo)"/>
    public static bool IsFileLocked(this FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException(nameof(file));
        }

        return GetProcessesLockingPath(file.FullName).Any();
    }

    /// <inheritdoc cref="LockUtilities.GetProcessesLockingFile(FileInfo)"/>
    public static IList<Process> GetProcessesLockingFile(this FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException(nameof(file));
        }

        return GetProcessesLockingPath(file.FullName).ToList();
    }


    /// <summary>
    /// Use <a href="https://docs.microsoft.com/en-us/windows/win32/api/restartmanager/">Restart Manager</a>
    /// to find the processes locking a file
    /// </summary>
    /// <param name="path">file to check</param>
    /// <returns><see cref="IEnumerable{Process}"/> of the processes currently locking <paramref name="path"/></returns>
    private static IEnumerable<Process>  GetProcessesLockingPath(string path)
    {
        var key = Guid.NewGuid().ToString();

        if (NativeMethods.RmStartSession(out var handle, 0, key) != 0)
        {
            // consider exception
            yield break;
        }

        try
        {
            const int errorMoreData = 234;
            uint processInfoCount = 0;
            uint lpdwRebootReasons = NativeMethods.RmRebootReasonNone;

            var resources = new [] { path }; // Just checking on one resource.

            if (NativeMethods.RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null) != 0)
            {
                // consider exception
                yield break;
            }

            var result = NativeMethods.RmGetList(handle, out var procInfoNeeded, ref processInfoCount, null, ref lpdwRebootReasons);
            if (result == errorMoreData)
            {
                var processInfo = new NativeMethods.RM_PROCESS_INFO[procInfoNeeded];
                processInfoCount = procInfoNeeded;

                if (NativeMethods.RmGetList(handle, out procInfoNeeded, ref processInfoCount, processInfo, ref lpdwRebootReasons) == 0)
                {
                    // processInfoCount may not match procInfoNeeded if one or more processes started/stopped between calls
                    var processes =  Enumerable.Range(0, (int)processInfoCount)
                        .Select(i => GetProcessOrDefault(processInfo, i))
                        .Where(p => p is not null)
                        .Cast<Process>();
                    foreach (var process in processes)
                    {
                        yield return process;
                    }
                }
            }
        }
        finally
        {
            _ = NativeMethods.RmEndSession(handle);
        }

        static Process? GetProcessOrDefault(in NativeMethods.RM_PROCESS_INFO[] processes, int index)
        {
            try
            {
                return Process.GetProcessById(processes[index].Process.dwProcessId);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
