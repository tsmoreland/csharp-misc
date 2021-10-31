using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TSMoreland.Utilities.FileUtilities.Linux;

internal static class LinuxLockUtilities
{
    /// <inheritdoc cref="LockUtilities.IsFileLocked(FileInfo)"/>
    public static bool IsFileLocked(this FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException(nameof(file));
        }

        throw new NotImplementedException("pending");
    }

    /// <inheritdoc cref="LockUtilities.GetProcessesLockingFile(FileInfo)"/>
    public static IList<Process> GetProcessesLockingFile(this FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException(nameof(file));
        }

        throw new NotImplementedException("pending");
    }
}
