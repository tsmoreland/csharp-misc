// client.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "com_environment.h"
#include "com_imports.h"
#include <Windows.h>
#include <iostream>
#include <memory>
#include <string>
#include <comdef.h>

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

    HostServerLib::ICSharpServiceWrapperPtr wrapper_ptr{};

    if (!try_com_method([&wrapper_ptr]() { return wrapper_ptr.CreateInstance(__uuidof(HostServerLib::CSharpServiceWrapper)); }))
        return 1;

    if (!wrapper_ptr->Ping() == VARIANT_TRUE)
        std::cout << "Ping returned true from out of process" << std::endl;

    _bstr_t value_bstr = L"Hello World";
    auto expected_length = static_cast<int>(value_bstr.length());
    auto actual_length = wrapper_ptr->StringLength(value_bstr);

    if (actual_length != expected_length)
        std::cout << "Lengths do not match, got" << actual_length << " but expected " << expected_length << std::endl;

    wrapper_ptr.Release();

    ClientServiceLib::IServicePtr service_ptr;
    if (!try_com_method([&service_ptr]() { return service_ptr.CreateInstance(__uuidof(ClientServiceLib::Service)); }))
        return 1;

    std::wstring const lower = L"all lower case";
    auto const upper_bstr = service_ptr->ToUpper(lower.c_str());
    std::wstring const upper(static_cast<wchar_t const*>(upper_bstr), upper_bstr.length());

    std::wcout  << "Lower case: " << lower << " upper case: " << upper << std::endl;

    return 0;
}

