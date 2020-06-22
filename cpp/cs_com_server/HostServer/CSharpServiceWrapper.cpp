// CSharpServiceWrapper.cpp : Implementation of CCSharpServiceWrapper

#include "pch.h"
#include "CSharpServiceWrapper.h"
#include <algorithm>
#include <string>
#include <string_view>

// CCSharpServiceWrapper
using std::begin;
using std::end;
using std::any_of;

STDMETHODIMP CCSharpServiceWrapper::InterfaceSupportsErrorInfo(REFIID riid) noexcept
{
	static IID const * const arr[] = 
	{
		&IID_ICSharpServiceWrapper
	};


	return any_of(begin(arr), end(arr), 
		[&riid](IID const* iid) {
			return InlineIsEqualGUID(*iid, riid) != 0;
		})
		? S_OK
		: S_FALSE;
}

STDMETHODIMP CCSharpServiceWrapper::Ping(VARIANT_BOOL* result)
{
	if (result == nullptr)
		return E_INVALIDARG;

	*result = VARIANT_TRUE;
	return S_OK;
}

STDMETHODIMP_(HRESULT __stdcall) CCSharpServiceWrapper::StringLength(BSTR value, INT* length)
{
	if (length == nullptr)
		return E_INVALIDARG;

	*length = 0;
	if (value == nullptr)
		return E_INVALIDARG;

	std::wstring_view value_view(value, SysStringLen(value));
	*length = static_cast<int>(value_view.size());

	return S_OK;
}

