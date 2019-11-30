#pragma once

#include <iostream>
#include <string>
#include <atlbase.h>

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
// not supported
#   else
// not supported
#   endif
#endif
