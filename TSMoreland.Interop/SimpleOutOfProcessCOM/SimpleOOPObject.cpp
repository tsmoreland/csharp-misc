//
// Copyright �2022 Terryy Moreland
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
    auto const uuid       = std::make_unique<char[]>(64);
    strcpy_s(uuid.get(), 64, source);

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
    Fire_OnPropertyChanaged(CComBSTR{L"Numeric"}.Detach());
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

STDMETHODIMP CSimpleOOPObject::ToUpper(BSTR input, BSTR* result) noexcept {
    if (input == nullptr || result == nullptr) {
        return E_INVALIDARG;
    }

    std::wstring upper{input};
    std::ranges::for_each(upper, [](wchar_t& ch) { ch = ::towupper(ch); });

    CComBSTR output{upper.c_str()};
    *result = output.Detach();
    return S_OK;
}

#pragma region infrastructure
HRESULT CSimpleOOPObject::FinalConstruct() {
    return S_OK;
}

void CSimpleOOPObject::FinalRelease() {
}
#pragma endregion
