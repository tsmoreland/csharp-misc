# Native AOT Example

The follow sample demonstrates a trival use of Native AOT to allow C++ to C# interop without the use of COM or C++/CLI,
additionally in a platform independent way.

There are lost of requirements for this to work documented on Microsoft learn.

Some examples used as a basis for this example

- [Native AOT Library](https://github.com/dotnet/samples/tree/main/core/nativeaot/NativeLibrary)

## Build Instructions

At the time of writing it requires some console build in addition to building the solution.  Building the solution will generate
the console app and C# library - but not in a way that can be consumed by the console app.

For C/C++ to access the library it must first be published using something like


```
dotnet publish -r win-x64 -c debug
```

optionally ```-o $(OUTDIR)``` can be used to publish to a more specific folder.  When ready the published dll must be placed
in the same folder as the C++ Console application (because it's coded to expect it there)

Additional notes

the library is consumed in the same fashion as other dynamic libs in C/C++ where they aren't directly linked -
via ```LoadLibrary``` on Windows or ```dlopen``` on Linux/mac.
A macro is ued in csharp_interop_aot.cpp SYM_LOAD which on windows uses ```GetProcAddress``` while on linux uses ```dlsym```

## Additional notes

Part of this sample is experimental to see if this can be used as a way to interop with actual C# libraries, chances
are no because the NativeAOT library has be self-contained and trimmed but none the less an attempt will be made
