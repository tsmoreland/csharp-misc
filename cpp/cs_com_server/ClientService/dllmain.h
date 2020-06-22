// dllmain.h : Declaration of module class.

class CClientServiceModule : public ATL::CAtlDllModuleT< CClientServiceModule >
{
public :
	DECLARE_LIBID(LIBID_ClientServiceLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_CLIENTSERVICE, "{4e4ca0a4-c0cf-4d6c-a1ba-06bcf3b0e30f}")
};

extern class CClientServiceModule _AtlModule;
