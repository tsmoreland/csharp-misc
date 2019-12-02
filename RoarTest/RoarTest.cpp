#include "Imports.h"
#include "ComEnvironment.h"
#include <metahost.h>

using std::cout;
using std::wcout;
using std::wcout;
using std::endl;
using std::exception;
using namespace Bear::Test;

constexpr int ComError = 100;
constexpr int ExceptionError = 101;
constexpr int Win32Error = 102;

template <typename F>
constexpr auto TryComMethod(F functor)
{
    if (const auto hr = functor();
        FAILED(hr))
    {
        ComEnvironment::WriteErrorToConsole(hr);
        return false;
    }
    return true;
}

int main()
{
    try
    {
        ComEnvironment environment;

        ATLBearLib::IGrizzlyPtr pGrizzly{ nullptr };
        if (!TryComMethod([&pGrizzly]() { return pGrizzly.CreateInstance(__uuidof(ATLBearLib::Grizzly)); }))
            return 1;

        pGrizzly->Name = L"Gordie";
        pGrizzly->Roar();

        const auto name = pGrizzly->Name;
        wcout << name << L" said Roar, name retrieved from ATLBear" << endl;
        pGrizzly.Release();

        ICLRMetaHost *pMetaHost{nullptr};
        if (!TryComMethod([&pMetaHost]() { return CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&pMetaHost)); }))
            return 2;

        ICLRRuntimeInfo *pRuntimeInfo{nullptr};
        if (!TryComMethod([pMetaHost, &pRuntimeInfo]() { return pMetaHost->GetRuntime(L"v4.0.30319", IID_PPV_ARGS(&pRuntimeInfo)); }))
            return 3;

        ICLRRuntimeHost *pClrRuntimeHost{nullptr};
        if (!TryComMethod([pRuntimeInfo, &pClrRuntimeHost]() { return pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_PPV_ARGS(&pClrRuntimeHost)); }))
            return 4;
        
        if (!TryComMethod([pClrRuntimeHost]() { return pClrRuntimeHost->Start(); }))
            return 5;

        NetFrameworkBear::IPolarBear *pBear{nullptr};

        auto uuid = __uuidof(NetFrameworkBear::PolarBear);
        if (const auto hr = CLRCreateInstance(__uuidof(NetFrameworkBear::PolarBear), IID_PPV_ARGS(&pBear));
            FAILED(hr))
        {
            ComEnvironment::WriteErrorToConsole(hr);
            return 6;
        }

        NetFrameworkBear::IPolarBearPtr pPolarBear{ nullptr };
        if (const auto hr = pPolarBear.CreateInstance(__uuidof(NetFrameworkBear::PolarBear));
            FAILED(hr))
        {
            ComEnvironment::WriteErrorToConsole(hr);
            return 3;
        }
        pPolarBear.Release();

        pClrRuntimeHost->Stop();
        pClrRuntimeHost->Release();
        pRuntimeInfo->Release();
        pMetaHost->Release();

        return 0;
    }
    catch (const _com_error & comEx)
    {
        wcout << comEx.ErrorMessage() << endl;
        return ComError;
    }
    catch (const exception & ex)
    {
        cout << ex.what() << endl;
        return ExceptionError;
    }
    catch (...)
    {
        wcout << "Unknown error occured, probably a win32 exception" << endl;
        return Win32Error;
    }
}

