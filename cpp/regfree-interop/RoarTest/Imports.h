#pragma once

#include <iostream>
#include <string>
#include <atlbase.h>

#pragma comment(lib, "mscoree.lib")
#import "mscorlib.tlb" rename("or", "interop_or") rename("ReportEvent", "InteropServices_ReportEvent")

#ifdef _WIN64
#   ifdef _DEBUG
#       import "../ATLBear/x64/Debug/ATLBear.tlb"
#       import "../NETFrameworkBear/bin/x64/Debug/NetFrameworkBear.tlb" 
#   else
#       import "../ATLBear/x64/Release/ATLBear.tlb"
#       import "../NETFrameworkBear/bin/x64/Release/NETFramework.tlb"
#   endif
#else
#   ifdef _DEBUG
#       import "../ATLBear/Debug/ATLBear.tlb"
#       import "../NETFrameworkBear/bin/x86/Debug/NetFrameworkBear.tlb" 
#   else
#       import "../ATLBear/Release/ATLBear.tlb"
#       import "../NETFrameworkBear/bin/x86/Release/NETFramework.tlb"
#   endif
#endif
