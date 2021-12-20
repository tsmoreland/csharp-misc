// SimpleObject.cpp : Implementation of CSimpleObject

#include "pch.h"
#include "SimpleObject.h"


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

    GUID id{};

    if (RPC_S_OK == UuidFromStringA(reinterpret_cast<RPC_CSTR>("E3FF39CC-D456-4A43-A799-8B19A6139908"), &id)) {
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
