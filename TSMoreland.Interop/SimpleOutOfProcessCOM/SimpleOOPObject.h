// SimpleOOPObject.h : Declaration of the CSimpleOOPObject

#pragma once
#include "resource.h"       // main symbols



#include "SimpleOutOfProcessCOM_i.h"
#include "_ISimpleOOPObjectEvents_CP.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CSimpleOOPObject

class ATL_NO_VTABLE CSimpleOOPObject :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CSimpleOOPObject, &CLSID_SimpleOOPObject>,
	public IConnectionPointContainerImpl<CSimpleOOPObject>,
	public CProxy_ISimpleOOPObjectEvents<CSimpleOOPObject>,
	public IDispatchImpl<ISimpleOOPObject2, &IID_ISimpleOOPObject2, &LIBID_SimpleOutOfProcessCOMLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
    LONG numeric_{0};

public:

    STDMETHOD(get_Name)(BSTR* result) noexcept;
    STDMETHOD(get_Id)(GUID* result) noexcept;
    STDMETHOD(get_Numeric)(LONG* result) noexcept;
    STDMETHOD(put_Numeric)(LONG value) noexcept;
    STDMETHOD(get_Description)(BSTR *result) noexcept;

    #pragma region infrastructure

    CSimpleOOPObject() = default;

DECLARE_REGISTRY_RESOURCEID(106)

DECLARE_NOT_AGGREGATABLE(CSimpleOOPObject)

BEGIN_COM_MAP(CSimpleOOPObject)
	COM_INTERFACE_ENTRY(ISimpleOOPObject)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
END_COM_MAP()

BEGIN_CONNECTION_POINT_MAP(CSimpleOOPObject)
	CONNECTION_POINT_ENTRY(__uuidof(_ISimpleOOPObjectEvents))
END_CONNECTION_POINT_MAP()


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct();

    void FinalRelease();

    #pragma endregion

};

OBJECT_ENTRY_AUTO(__uuidof(SimpleOOPObject), CSimpleOOPObject)
