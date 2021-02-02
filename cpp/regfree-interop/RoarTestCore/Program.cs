using System;
using Bears.Native;

namespace RoarTestCore
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            var grizzly = new GrizzlyClass();

            grizzly.Roar();

            var bytes = new byte[8];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = 0;

            var originalBytes = bytes;
            grizzly.Oneify(ref bytes);

            foreach (var t in originalBytes)
                Console.Out.Write($"{t} ");
            Console.Out.WriteLine();

            foreach (var t in bytes)
                Console.Out.Write($"{t} ");
            Console.Out.WriteLine();
            
            Console.Out.WriteLine("----------------------");

            bytes = new byte[8];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = 0;

            originalBytes = bytes;
            grizzly.Twoify(bytes);

            foreach (var t in originalBytes)
                Console.Out.Write($"{t} ");
            Console.Out.WriteLine();

            foreach (var t in bytes)
                Console.Out.Write($"{t} ");
            Console.Out.WriteLine();
        }
    }
}
