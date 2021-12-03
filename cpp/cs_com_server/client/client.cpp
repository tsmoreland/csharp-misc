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
#include "CsCoreClientService.h"

using namespace client;

template <typename F>
constexpr auto try_com_method(F functor)
{
    const auto hr = functor();
    return SUCCEEDED(hr);
}

using namespace core::client_service;

int main()
{
    com_environment env;

    try {
        std::wstring const lower = L"all lower case";
        HostServerLib::IServiceProxyPtr service_proxy_ptr{};

        if (!try_com_method([&service_proxy_ptr]() { return service_proxy_ptr.CreateInstance(__uuidof(HostServerLib::ServiceProxy), nullptr, CLSCTX_LOCAL_SERVER); }))
            return 1;

        service_proxy_ptr->RegisterOwningProcessId(static_cast<int>(GetCurrentProcessId()));

        _bstr_t const input(lower.c_str());
        auto upper_bstr = service_proxy_ptr->ToUpper(input);
        std::wstring upper(static_cast<wchar_t const*>(upper_bstr), upper_bstr.length());
        std::wcout  << "(From Proxy): Lower case: " << lower << " upper case: " << upper << std::endl;

        ClientServiceLib::IServicePtr service_ptr;
        if (!try_com_method([&service_ptr]() { return service_ptr.CreateInstance(__uuidof(ClientServiceLib::Service)); }))
            return 1;

        upper_bstr = service_ptr->ToUpper(lower.c_str());
        upper = static_cast<wchar_t const*>(upper_bstr), upper_bstr.length();

        std::wcout  << "Lower case: " << lower << " upper case: " << upper << std::endl;

        CsClientService::_ServicePtr cs_service_ptr;
        if (!try_com_method([&cs_service_ptr]() { return cs_service_ptr.CreateInstance(__uuidof(CsClientService::Service), nullptr, CLSCTX_INPROC); }))
            return 1;

        core_service_ptr core_service;
        if (!try_com_method([&core_service]() { return core_service.CreateInstance(__uuidof(CoreClientService), nullptr, CLSCTX_INPROC); }))
            return 1;

        int i;
        std::cin >> i;

        std::wstring const all_upper = L"ALL UPPER CASE";
        upper_bstr = core_service->ToLower(all_upper.c_str());
        upper = static_cast<wchar_t const*>(upper_bstr), upper_bstr.length();
        std::wcout  << "Upper case: " << all_upper << " lower case: " << upper << std::endl;

        std::cin >> i;

    } catch (ATL::CAtlException const&) {
        return 2;
    } catch (_com_error const&) {
        return 3;
    }

    return 0;
}

