// CSharpServiceWrapper.cpp : Implementation of CCSharpServiceWrapper

#include "pch.h"
#include "CSharpServiceWrapper.h"
#include <algorithm>

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

