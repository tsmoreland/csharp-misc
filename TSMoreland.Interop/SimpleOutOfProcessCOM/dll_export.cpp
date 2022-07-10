// SimpleOutOfProcessCOM.cpp : Implementation of WinMain


// ReSharper disable CppInconsistentNaming
// ReSharper disable CppClangTidyBugproneReservedIdentifier
// ReSharper disable CppClangTidyClangDiagnosticReservedIdentifier
#include "pch.h"
#include "framework.h"
#include "resource.h"
#include "SimpleOutOfProcessCOM_i.h"
#include "xdlldata.h"


using namespace ATL;


class CSimpleOutOfProcessCOMModule : public ATL::CAtlExeModuleT< CSimpleOutOfProcessCOMModule >
{
public :
	DECLARE_LIBID(LIBID_SimpleOutOfProcessCOMLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_SIMPLEOUTOFPROCESSCOM, "{4faab4cd-f38e-4709-a0e3-b15763ec7452}")
};

CSimpleOutOfProcessCOMModule _AtlModule;



//
#pragma warning( disable : 28251 )
extern "C" int WINAPI _tWinMain(HINSTANCE /*hInstance*/, HINSTANCE /*hPrevInstance*/,
								LPTSTR /*lpCmdLine*/, int nShowCmd)
{
	return _AtlModule.WinMain(nShowCmd);
}

