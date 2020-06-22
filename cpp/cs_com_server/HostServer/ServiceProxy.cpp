//
// Copyright © 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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
