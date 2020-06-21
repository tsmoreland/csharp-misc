// client.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "com_environment.h"
#include "com_imports.h"
#include <Windows.h>

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

    wrapper_ptr->Ping();

    return 0;
}

