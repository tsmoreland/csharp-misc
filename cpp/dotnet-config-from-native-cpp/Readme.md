# app.config use from native C++ sample

## Project structure

Sample consists of 3 projects

- host application, such as clrConsole a C++/CLI console application
- managed library, exposes function get value from appSettings key/value pair as ```std::string```
- native library, consumes the function to get appsetting value from key/value pair

## Implementation Notes

Managed library was created as a standard (shared) dll project with C++/CLI support added afterwords.  
Dllmain was removed as it doesn't behave well in a C++/CLI context.  Project was updated to create
export library, it was disable when C++/CLI support was enabled

Native library has a project reference to managed which satisfies build order but doesn't handle importing
the '.lib' file, as such this must be added to the linking input files or there will be linking errors

## Alternate approach

Managed library could be replaced with C# library and configured to create an export lib for consumption in
C++, a project of this style may yet be created
