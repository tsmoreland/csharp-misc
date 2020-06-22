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
    _bstr_t input(lower.c_str());
    auto upper_bstr = service_proxy_ptr->ToUpper(input);
    std::wstring upper(static_cast<wchar_t const*>(upper_bstr), upper_bstr.length());
    std::wcout  << "(From Proxy): Lower case: " << lower << " upper case: " << upper << std::endl;

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

