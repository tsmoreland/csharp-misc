# Introduction

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
