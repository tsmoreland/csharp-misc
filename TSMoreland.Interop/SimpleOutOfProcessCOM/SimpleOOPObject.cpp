// SimpleOOPObject.cpp : Implementation of CSimpleOOPObject

#include "pch.h"
#include <algorithm>
#include "SimpleOOPObject.h"

#include <memory>

STDMETHODIMP CSimpleOOPObject::get_Name(BSTR* result) noexcept {
    if (result == nullptr) {
        return E_INVALIDARG;
    }

    CComBSTR value{L"OOP Name"};
    *result = value.Detach();

    return S_OK;
}
STDMETHODIMP CSimpleOOPObject::get_Id(GUID* result) noexcept {
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

    return S_OK;
}
STDMETHODIMP CSimpleOOPObject::get_Numeric(LONG* result) noexcept {
    if (result == nullptr) {
        return E_INVALIDARG;
    }

    *result = numeric_;

    return S_OK;
}
STDMETHODIMP CSimpleOOPObject::put_Numeric(LONG value) noexcept {

    numeric_ = value;
    return S_OK;
}
STDMETHODIMP CSimpleOOPObject::get_Description(BSTR *result) noexcept  {
    if (result == nullptr) {
        return E_INVALIDARG;
    }

    CComBSTR value{L"OOP Description"};
    *result = value.Detach();

    return S_OK;
}

#pragma region infrastructure
HRESULT CSimpleOOPObject::FinalConstruct() {
    return S_OK;
}

void CSimpleOOPObject::FinalRelease() {
}
#pragma endregion
