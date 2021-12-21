//
// Copyright © 2021 Terry Moreland
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

#include "pch.h"
#include "SimpleObject.h"

#include <memory>


// CSimpleObject

// ReSharper disable once CppInconsistentNaming
// ReSharper disable once CppMemberFunctionMayBeStatic
HRESULT CSimpleObject::FinalConstruct() {
    return S_OK;
}

// ReSharper disable once CppInconsistentNaming
// ReSharper disable once CppMemberFunctionMayBeStatic
void CSimpleObject::FinalRelease() {}

STDMETHODIMP CSimpleObject::get_Id(GUID* result) noexcept {
    
    if (result == nullptr) {
        return E_INVALIDARG;
    }

    constexpr auto source = "E3FF39CC-D456-4A43-A799-8B19A6139908";
    auto const uuid       = std::make_unique<char[]>(37);
    strcpy_s(uuid.get(), 36, source);

    if (GUID id{};
        RPC_S_OK == UuidFromStringA(reinterpret_cast<RPC_CSTR>(uuid.get()), &id)) {
        *result = id;
        return S_OK;
    }

    *result = GUID{};

    return E_FAIL;
}

STDMETHODIMP CSimpleObject::get_Name(BSTR* result) noexcept {

    if (result == nullptr) {
        return E_INVALIDARG;
    }

    CComBSTR value(L"Simple Name");
    *result = value.Detach();

    return S_OK;
}

STDMETHODIMP CSimpleObject::get_Numeric(LONG* result) noexcept {

    if (result == nullptr) {
        return E_INVALIDARG;
    }

    *result = numeric_;
    return S_OK;
}

STDMETHODIMP CSimpleObject::put_Numeric(LONG value) noexcept {

    numeric_ = value;
    return S_OK;
}

STDMETHODIMP CSimpleObject::ConvertToString(GUID input, BSTR* result) noexcept {

    if (result == nullptr) {
        return E_INVALIDARG;
    }
    
    wchar_t* stringified{};
    if (RPC_S_OK !=  UuidToStringW(&input, reinterpret_cast<RPC_WSTR*>(&stringified))) {
        return E_FAIL;
    }

    // will this copy the string or do I need to?  time will tell, that or google
    CComBSTR output{stringified};
    *result = output.Detach();

    RpcStringFreeW(reinterpret_cast<RPC_WSTR*>(&stringified));
    return S_OK;
}

STDMETHODIMP CSimpleObject::get_Description(BSTR* result) noexcept {

    if (result == nullptr) {
        return E_INVALIDARG;
    }

    CComBSTR value(L"Simple Description");
    *result = value.Detach();

    return S_OK;
}
