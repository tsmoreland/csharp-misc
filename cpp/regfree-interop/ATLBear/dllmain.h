// dllmain.h : Declaration of module class.

class CATLBearModule : public ATL::CAtlDllModuleT< CATLBearModule >
{
public :
	DECLARE_LIBID(LIBID_ATLBearLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_ATLBEAR, "{3cbf3d3a-db02-4a72-8674-8088e52d0ba6}")
};

extern class CATLBearModule _AtlModule;
