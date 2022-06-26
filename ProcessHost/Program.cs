using System.Diagnostics;
using System.Runtime.InteropServices;

// See https://aka.ms/new-console-template for more information

ProcessStartInfo startInfo = new ProcessStartInfo(args[0]);
startInfo.WorkingDirectory = Path.GetDirectoryName(args[0]);

Console.WriteLine($"Starting {args[0]} in {startInfo.WorkingDirectory}");

Process? p = Process.Start(startInfo);
if (p is null)
{
    return;
}


// keep process alive for 2 seconds before 'stopping' with ctrl+c to determine if it correctly picks up the sigint
Thread.Sleep(2000);

try
{
    NativeMethods.SetConsoleCtrlHandler(null, true);
    if (!NativeMethods.GenerateConsoleCtrlEvent(NativeMethods.ControlCEvent, 0))
    {
        Console.WriteLine("Failed to send ctrl+c");
        p.Kill();
    }

    Console.WriteLine("Waiting for exit");
    p.WaitForExit();
}
finally
{
    NativeMethods.SetConsoleCtrlHandler(null, false);
    NativeMethods.FreeConsole();
}


internal class NativeMethods
{
    public const int ControlCEvent = 0;

    [DllImport("kernel32.dll")]
    public static extern bool GenerateConsoleCtrlEvent(uint ctrlEvent, uint processGroupId);

    [DllImport("kernel32.dll", SetLastError = true)]
    public  static extern bool AttachConsole(uint processId);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern bool FreeConsole();

    [DllImport("kernel32.dll")]
    public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate? handlerRoutine, bool add);

    public delegate bool ConsoleCtrlDelegate(uint CtrlType);
}
