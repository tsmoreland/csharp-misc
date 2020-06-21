// HostServer.cpp : Implementation of WinMain


#include "pch.h"
#include "framework.h"
#include "resource.h"
#include "HostServer_i.h"
#include "xdlldata.h"


using namespace ATL;


class CHostServerModule : public ATL::CAtlExeModuleT< CHostServerModule >
{
public :
	DECLARE_LIBID(LIBID_HostServerLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_HOSTSERVER, "{eb39a88d-2c65-47f4-a560-87941c0a456f}")
};

CHostServerModule _AtlModule;



//
extern "C" int WINAPI _tWinMain(HINSTANCE /*hInstance*/, HINSTANCE /*hPrevInstance*/,
								LPTSTR /*lpCmdLine*/, int nShowCmd)
{
	return _AtlModule.WinMain(nShowCmd);
}

