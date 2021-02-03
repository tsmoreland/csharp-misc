// Grizzly.cpp : Implementation of CGrizzly

#include "pch.h"
#include <comdef.h>
#include "CGrizzly.h"
#include "GrizzlyBear.h"

using std::make_unique;
using std::exception;
using Bears::Native::GrizzlyBear;

namespace
{
    template <typename T>
    constexpr auto to_size_t(T const value)
    {
        return static_cast<std::size_t>(value);
    }

    [[nodiscard]]
    bool HasExpectedDimensionAndBounds(SAFEARRAY * const source, unsigned expectedDimension, std::size_t expectedSize)
    {
        if (source == nullptr)
            return false;

        
        if (auto const dim = SafeArrayGetDim(source);
            dim != expectedDimension)
            return false;

        long lowerBound{0};
        long upperBound{0};

        if (auto const hr = SafeArrayGetUBound(source, 1, &upperBound);
            FAILED(hr)) {
            return false;
        }
        if (auto const hr = SafeArrayGetLBound(source, 1, &lowerBound);
            FAILED(hr)) {
            return false;
        }

        if ((to_size_t(upperBound) - to_size_t(lowerBound) + 1UL) != expectedSize)
            return false;

        return true;
    }
    
}

// CGrizzly
CGrizzly::CGrizzly()
    : m_pGrizzly{make_unique<GrizzlyBear>()}
{
}

STDMETHODIMP CGrizzly::get_Name(BSTR* value) 
{
    return SafeComCall([this, &value]() {
        const CComBSTR pValue(m_pGrizzly->Name.c_str());
        return pValue.CopyTo(value);
    });
}

STDMETHODIMP CGrizzly::put_Name(BSTR value)
{
    return SafeComCall([this, value]() {
        m_pGrizzly->Name = value;
        return S_OK;
    });
}

STDMETHODIMP CGrizzly::Roar()
{
    return SafeComCall([this]() {
        m_pGrizzly->Roar();
        return S_OK;
    });
}

STDMETHODIMP CGrizzly::Oneify(SAFEARRAY** pSource, VARIANT_BOOL* pRetVal) 
{
    return SafeComCall([pSource, pRetVal]() {
        byte const bytes[] = { 1, 1, 1, 1,  1, 1, 1, 1};
        if (pSource == nullptr || !HasExpectedDimensionAndBounds(*pSource, 1, std::size(bytes)) ) {
            return E_INVALIDARG;
        }

        void* pData{nullptr};
        if (auto const hr = SafeArrayAccessData(*pSource, &pData);
            FAILED(hr)) {
            *pRetVal = VARIANT_FALSE;
            return S_OK;
        }

        memcpy_s(pData, std::size(bytes), bytes, std::size(bytes));
        SafeArrayUnaccessData(*pSource);

        *pRetVal = VARIANT_TRUE;
        return S_OK;
    });
}

STDMETHODIMP CGrizzly::Twoify(SAFEARRAY* pSource, VARIANT_BOOL* pRetVal) 
{
    return SafeComCall([pSource, pRetVal]()
    {
        byte const bytes[] = { 2, 2, 2, 2,  2, 2, 2, 2};
        if (!HasExpectedDimensionAndBounds(pSource, 1u, std::size(bytes))) {
            return E_INVALIDARG;
        }

        void* pData{nullptr};
        if (auto const hr = SafeArrayAccessData(pSource, &pData);
            FAILED(hr)) {
            *pRetVal = VARIANT_FALSE;
            return S_OK;
        }

        memcpy_s(pData, std::size(bytes), bytes, std::size(bytes));
        SafeArrayUnaccessData(pSource);

        *pRetVal = VARIANT_TRUE;
        return S_OK;
    });
}

template<class F>
HRESULT CGrizzly::SafeComCall(F functor) noexcept
{
    try {
        return functor();
    }
    catch (const _com_error & comEx) {
        return comEx.Error();
    }
    catch (const exception &) {
        return E_FAIL;
    }
    catch (...) {
        return E_FAIL;
    }
}

