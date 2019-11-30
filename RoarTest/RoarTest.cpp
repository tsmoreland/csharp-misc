#include "Imports.h"

using std::cout;
using std::wcout;
using std::wcout;
using std::endl;
using std::exception;

constexpr int ComError = 100;
constexpr int ExceptionError = 101;
constexpr int Win32Error = 102;

int main()
{
    try
    {
        if (const auto hr = CoInitializeEx(nullptr, COINIT_MULTITHREADED);
            FAILED(hr))
        {
            cout << "Failed to initialize COM" << endl;
            return 1;
        }

        ATLBearLib::IGrizzlyPtr pGrizzly{ nullptr };
        if (const auto hr = pGrizzly.CreateInstance(__uuidof(ATLBearLib::Grizzly));
            FAILED(hr))
        {
            cout << "Failed to create Grizzly Bear" << endl;
            return 2;
        }

        pGrizzly->Name = L"Gordie";
        pGrizzly->Roar();

        const auto name = pGrizzly->Name;
        wcout << name << L" said Roar, name retrieved from ATLBear" << endl;
        pGrizzly.Release();

        NetFrameworkBear::IPolarBearPtr pPolarBear{ nullptr };
        if (const auto hr = pPolarBear.CreateInstance(__uuidof(NetFrameworkBear::PolarBear));
            FAILED(hr))
        {
            cout << "Failed to create Polar Bear" << endl;
            return 3;
        }


        pPolarBear.Release();

        CoUninitialize();
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

