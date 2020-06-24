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
#include "process_monitor.h"
#include "windows_exception.h"
#include <comdef.h>
// CServiceProxy

STDMETHODIMP CServiceProxy::ToUpper(BSTR input, BSTR* output) noexcept
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

STDMETHODIMP CServiceProxy::RegisterOwningProcessId(INT processId) noexcept
{
	using host_server::process_monitor;
	using host_server::process;
	try {
		auto const pid = static_cast<DWORD>(processId);
		process_monitor::get_instance().exit_when_process_exits(process(pid));
        return S_OK;
	} catch (modern_win32::windows_exception const&) {
		return E_INVALIDARG;
	}
}

STDMETHODIMP CServiceProxy::InterfaceSupportsErrorInfo(REFIID riid) noexcept
{
	static const IID* const arr[] = { &IID_IServiceProxy };

	return std::any_of(std::begin(arr), std::end(arr), 
		[&riid](IID const* iid) {
			return InlineIsEqualGUID(*iid, riid) != 0;
		})
		? S_OK
		: S_FALSE;
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
