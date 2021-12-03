// Service.h : Declaration of the CService

#pragma once
#include "resource.h"       // main symbols



#include "ClientService_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CService

class ATL_NO_VTABLE CService :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CService, &CLSID_Service>,
	public ISupportErrorInfo,
	public IDispatchImpl<IService, &IID_IService, &LIBID_ClientServiceLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CService() = default;

	STDMETHODIMP ToUpper(BSTR input, BSTR* output) override;

    DECLARE_REGISTRY_RESOURCEID(106)

    DECLARE_NOT_AGGREGATABLE(CService)

    BEGIN_COM_MAP(CService)
        COM_INTERFACE_ENTRY(IService)
        COM_INTERFACE_ENTRY(IDispatch)
        COM_INTERFACE_ENTRY(ISupportErrorInfo)
    END_COM_MAP()

// ISupportsErrorInfo
	STDMETHODIMP InterfaceSupportsErrorInfo(REFIID riid) noexcept override;


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:



};

OBJECT_ENTRY_AUTO(__uuidof(Service), CService)
