// dllmain.h : Declaration of module class.

class CSimpleInProcessCOMModule : public ATL::CAtlDllModuleT< CSimpleInProcessCOMModule >
{
public :
	DECLARE_LIBID(LIBID_SimpleInProcessCOMLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_SIMPLEINPROCESSCOM, "{580185ad-317a-4eb7-a6ab-48ebd08c8407}")
};

extern class CSimpleInProcessCOMModule _AtlModule;
