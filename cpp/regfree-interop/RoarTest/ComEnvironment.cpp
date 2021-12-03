#include "ComEnvironment.h"
#include <windows.h>
#include <exception>
#include <iostream>
#include <memory>
#include <string>

using std::exception;
using std::wcout;
using std::endl;
using std::hex;
using std::dec;
using std::make_unique;
using std::wstring_view;

namespace Bear::Test
{
    ComEnvironment::ComEnvironment()
    {
        if (const auto hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
            FAILED(hr))
            throw exception("Failed to initialize COM");
    }
    ComEnvironment::~ComEnvironment()
    {
       CoUninitialize(); 
    }

    void ComEnvironment::WriteErrorToConsole(const HRESULT hr)
    {
        const auto szErrMsg = make_unique<TCHAR[]>(1024);
        if (FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, nullptr, hr, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), static_cast<LPWSTR>(szErrMsg.get()), 1024, nullptr) != 0)
        {
            const wstring_view view(szErrMsg.get());
            wcout << view << endl;
        }
        else
            wcout << L"Could not find a description for error 0x" << hex << hr << dec << endl;
    }
}
