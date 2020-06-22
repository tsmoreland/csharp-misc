// ServiceProxy.cpp : Implementation of CServiceProxy

#include "pch.h"
#include "ServiceProxy.h"
#include <comdef.h>
// CServiceProxy

STDMETHODIMP_(HRESULT __stdcall) CServiceProxy::ToUpper(BSTR input, BSTR* output)
{
	try {
		*output = m_service_ptr->ToUpper(input);
		return S_OK;

	} catch (ATL::CAtlException const& e) {
		return static_cast<HRESULT>(e);

	} catch (_com_error const& e) {
		return e.Error();
	}
}

STDMETHODIMP CServiceProxy::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* const arr[] = 
	{
		&IID_IServiceProxy
	};

	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}

HRESULT CServiceProxy::FinalConstruct()
{
	try {
        return m_service_ptr.CreateInstance(__uuidof(ClientServiceLib::Service));

	} catch (ATL::CAtlException const& e) {
		return static_cast<HRESULT>(e);

	} catch (_com_error const& e) {
		return e.Error();
	}

}

void CServiceProxy::FinalRelease()
{
	m_service_ptr.Release();
}
