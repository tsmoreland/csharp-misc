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
