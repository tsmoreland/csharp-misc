using System.Runtime.InteropServices;

namespace TSMoreland.Samples.CSharpInteropAot;

public sealed class Calculator
{
    private static Lazy<Calculator> s_calculator = new Lazy<Calculator>(() => new Calculator());

    public Calculator()
    {
    }

    public int Add(int x, int y)
    {
        return x + y;
    }

    [UnmanagedCallersOnly(EntryPoint = "add")]
    public static int NativeAdd(int x, int y)
    {
        return s_calculator.Value.Add(x, y);
    }
}