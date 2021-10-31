using System;
using System.Runtime.InteropServices;

namespace TSMoreland.Utilities.FileUtilities.Linux;

internal static class NativeMethods
{

    [DllImport("libc", CharSet = CharSet.Ansi, EntryPoint = "fopen")]
    public static extern IntPtr Fopen([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.LPStr)] string mode);

    [DllImport("libc", CharSet = CharSet.Ansi, EntryPoint = "fclose")]
    public static extern IntPtr Fclose(IntPtr filePointer);
}
