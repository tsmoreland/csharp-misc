//
// Copyright © 2020 Terry Moreland
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

// client.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "com_environment.h"
#include "com_imports.h"
#include <Windows.h>
#include <iostream>
#include <memory>
#include <string>
#include <comdef.h>
#include <atlbase.h>

using namespace client;

template <typename F>
constexpr auto try_com_method(F functor)
{
    const auto hr = functor();
    return SUCCEEDED(hr);
}


int main()
{
    com_environment env;

    std::wstring const lower = L"all lower case";
    HostServerLib::IServiceProxyPtr service_proxy_ptr{};

    if (!try_com_method([&service_proxy_ptr]() { return service_proxy_ptr.CreateInstance(__uuidof(HostServerLib::ServiceProxy), nullptr, CLSCTX_LOCAL_SERVER); }))
        return 1;

    try {
        _bstr_t const input(lower.c_str());
        auto upper_bstr = service_proxy_ptr->ToUpper(input);
        std::wstring upper(static_cast<wchar_t const*>(upper_bstr), upper_bstr.length());
        std::wcout  << "(From Proxy): Lower case: " << lower << " upper case: " << upper << std::endl;

        //service_proxy_ptr.RegisterOwningProcessId(static_cast<int>(GetCurrentProcessId()));

        service_proxy_ptr.Release();

        ClientServiceLib::IServicePtr service_ptr;
        if (!try_com_method([&service_ptr]() { return service_ptr.CreateInstance(__uuidof(ClientServiceLib::Service)); }))
            return 1;

        upper_bstr = service_ptr->ToUpper(lower.c_str());
        upper = static_cast<wchar_t const*>(upper_bstr), upper_bstr.length();

        std::wcout  << "Lower case: " << lower << " upper case: " << upper << std::endl;

    } catch (ATL::CAtlException const&) {
        service_proxy_ptr.Release();
        return 2;
    } catch (_com_error const&) {
        service_proxy_ptr.Release();
        return 3;
    }

    return 0;
}

