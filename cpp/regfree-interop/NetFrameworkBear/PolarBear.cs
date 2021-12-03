using System;
using System.Runtime.InteropServices;

namespace NetFrameworkBear
{
    [Guid("f4f5a9f9-7c2e-4aa5-aea3-dd1efbd55d14")]
    [ComVisible(true)]
    [ProgId("NetFrameworkBear.PolarBear")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PolarBear : IPolarBear
    {
        public string Name { get; set; }

        public void Roar()
        {
            Console.Out.WriteLine($"{Name} Roars from .NET Framework");
        }
    }
}
