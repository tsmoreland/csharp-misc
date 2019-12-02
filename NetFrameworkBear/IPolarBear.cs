using System;
using System.Runtime.InteropServices;

namespace NetFrameworkBear
{
    [Guid("0a40fade-0443-493a-adf5-c92122d0a380")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface IPolarBear
    {
        [DispId(1)]
        string Name { get; set; }
        [DispId(2)]
        void Roar();
    }
}
