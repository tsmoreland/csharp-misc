//
// Copyright � 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

#pragma once


#ifdef _WIN64
#   ifdef _DEBUG
#       import "../HostServer/x64/Debug/HostServer.tlb"
#       import "../ClientService/x64/Debug/ClientService.tlb"
#   else
#       import "../HostServer/x64/Release/HostServer.tlb"
#       import "../ClientService/x64/Release/ClientService.tlb"
#   endif
#else
#   ifdef _DEBUG
#       import "../HostServer/Win32/Debug/HostServer.tlb"
#       import "../ClientService/Win32/Debug/ClientService.tlb" rename("value", "csValue")
#       import "../Win32/Debug/CsClientService.tlb"
#   else
#       import "../HostServer/Win32/Release/HostServer.tlb"
#       import "../ClientService/Win32/Release/ClientService.tlb"
#   endif
#endif
