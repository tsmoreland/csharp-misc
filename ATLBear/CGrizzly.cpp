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
    catch (const exception & ex)
    {
        return E_FAIL;
    }
    catch (...)
    {
        return E_FAIL;
    }
}

