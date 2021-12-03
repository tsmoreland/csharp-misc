# Sample-Code
General Samples of Techniques that may be useful in other projects.  Intended to be a hub of reference code

The 'Utilities class' of repositories, a dumping ground for various experimental or proof of concept style code.

## Out of Process C# COM object Proxy (cpp/cs_com_server/)

- a C++ console application using 
    1. registration free COM written in C++
    2. out of process registered C++/CLI COM Server serving a proxy to the same COM object in 1
    3. registered C# .NET 4.8 COM object 
    4. registration free COM C# dotnet core 3.1 

build of this solution is a bit of mess, not much testing beyond Debug/Win32 and even then build order is a bit of a mess, client depends on everything 
else having built and being registered

dotnet core 3.1 COM is a bit tricky, easy enough to build and support registration free but no support for tlb so either an IDL or header file needs to be written seperately,
I went with writing the header file which is part of the client project

## registration free COM interop sample

Interop Samples is intended to be a set of COM interop examples/samples demonstrating or at least providing a working sample for:

1. Registration Free COM (C++) consumed by a C++ COM Client
2. Registration Free COM (C#) consumed by a C++ COM Client
3. Registration Free COM (C++) consumed by a C# Client
4. Registration Free COM (C#) consumed by a C# client -- this one will likely not be done as it's pointless, C# to C# can be done directly
but I may glance at it to see what sort of madness may lie there.
5. dotnet Core COM Interop, again registration free both as a COM Server and COM Client

Eventually this will lead up to an attempt to demonstrate 
https://github.com/dotnet/coreclr/issues/20715

which was fixed in dotnet core with
https://github.com/dotnet/coreclr/pull/20746

but remains at large in .NET Framework, the hope is that shifting the C# to dotnet Core will be enough, the concern is that the interop
is still generated with .NET Framework so may yet still hold the same problem
