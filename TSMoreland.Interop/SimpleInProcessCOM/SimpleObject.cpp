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
        return S_OK;
    }

    *result = id;

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
