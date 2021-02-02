// Grizzly.cpp : Implementation of CGrizzly

#include "pch.h"
#include <comdef.h>
#include "CGrizzly.h"
#include "GrizzlyBear.h"

using std::make_unique;
using std::exception;
using Bears::Native::GrizzlyBear;

// CGrizzly
CGrizzly::CGrizzly()
    : m_pGrizzly(make_unique<GrizzlyBear>())
{
}

STDMETHODIMP CGrizzly::get_Name(BSTR* value) 
{
    return SafeComCall([this, &value]()
    {
        const CComBSTR pValue(m_pGrizzly->Name.c_str());
        return pValue.CopyTo(value);
    });
}

STDMETHODIMP CGrizzly::put_Name(BSTR value)
{
    return SafeComCall([this, value]()
    {
        m_pGrizzly->Name = value;
        return S_OK;
    });
}

STDMETHODIMP CGrizzly::Roar()
{
    return SafeComCall([this]()
    {
        m_pGrizzly->Roar();
        return S_OK;
    });
}

STDMETHODIMP CGrizzly::Oneify(SAFEARRAY** pSource, VARIANT_BOOL* pRetVal) 
{
    return SafeComCall([pSource, pRetVal]()
    {
        void* pData{nullptr};
        if (auto const hr = SafeArrayAccessData(*pSource, &pData);
            FAILED(hr)) {
            *pRetVal = VARIANT_FALSE;
            return S_OK;
        }

        byte const bytes[] = { 1, 1, 1, 1,  1, 1, 1, 1};
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
        void* pData{nullptr};
        if (auto const hr = SafeArrayAccessData(pSource, &pData);
            FAILED(hr)) {
            *pRetVal = VARIANT_FALSE;
            return S_OK;
        }

        byte const bytes[] = { 2, 2, 2, 2,  2, 2, 2, 2};
        memcpy_s(pData, std::size(bytes), bytes, std::size(bytes));
        SafeArrayUnaccessData(pSource);

        *pRetVal = VARIANT_TRUE;
        return S_OK;
    });
}

template<class F>
HRESULT CGrizzly::SafeComCall(F functor) noexcept
{
    try
    {
        return functor();
    }
    catch (const _com_error & comEx)
    {
        return comEx.Error();
    }
    catch (const exception &)
    {
        return E_FAIL;
    }
    catch (...)
    {
        return E_FAIL;
    }
}

