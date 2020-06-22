// Service.cpp : Implementation of CService

#include "pch.h"
#include "Service.h"
#include <string>
#include <string_view>
#include <algorithm>
#include <cctype>
#include <locale>

// CService

STDMETHODIMP CService::ToUpper(BSTR input, BSTR* output)
{
	if (input == nullptr || output == nullptr)
		return E_INVALIDARG;

	std::wstring_view input_view(input, SysStringLen(input));
	std::wstring out;

	*output = nullptr;
	
	try {
		std::transform(std::begin(input_view), std::end(input_view), std::back_inserter(out),
			[](auto& ch) {
				return std::toupper(ch, std::locale());
			});

		CComBSTR out_bstr(out.c_str());
		*output = out_bstr.Detach();

	} catch (std::bad_alloc const&) {
		return E_OUTOFMEMORY;
	}

	return S_OK;
}

STDMETHODIMP CService::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* const arr[] = 
	{
		&IID_IService
	};

	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}
