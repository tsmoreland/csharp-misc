// Grizzly.h : Declaration of the CGrizzly

#pragma once
#include "resource.h"       // main symbols
#include <memory>
#include "ATLBear_i.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;

namespace Bears::Native
{
    class GrizzlyBear;
}

// CGrizzly

class ATL_NO_VTABLE CGrizzly :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CGrizzly, &CLSID_Grizzly>,
	public IDispatchImpl<IGrizzly, &IID_IGrizzly, &LIBID_ATLBearLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:

    CGrizzly();
    STDMETHODIMP get_Name(BSTR* value) override;
    STDMETHODIMP put_Name(BSTR value) override;
    STDMETHODIMP Roar() override;

public:

DECLARE_REGISTRY_RESOURCEID(106)

DECLARE_NOT_AGGREGATABLE(CGrizzly)

BEGIN_COM_MAP(CGrizzly)
	COM_INTERFACE_ENTRY(IGrizzly)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY_AGGREGATE(IID_IMarshal, m_pUnkMarshaler.p)
END_COM_MAP()


	DECLARE_PROTECT_FINAL_CONSTRUCT()
	DECLARE_GET_CONTROLLING_UNKNOWN()

	HRESULT FinalConstruct() 
	{
		return CoCreateFreeThreadedMarshaler(
			GetControllingUnknown(), &m_pUnkMarshaler.p);
	}

	void FinalRelease()
	{
        m_pGrizzly.reset();
		m_pUnkMarshaler.Release();
	}

    CComPtr<IUnknown> m_pUnkMarshaler{ nullptr };

private:
    std::unique_ptr<Bears::Native::GrizzlyBear> m_pGrizzly;

    template<class F>
    [[nodiscard]] static HRESULT SafeComCall(F functor) noexcept;
};

OBJECT_ENTRY_AUTO(__uuidof(Grizzly), CGrizzly)
