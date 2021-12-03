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

