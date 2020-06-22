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
// CSharpServiceWrapper.h : Declaration of the CCSharpServiceWrapper

#pragma once
#include "resource.h"       // main symbols



#include "HostServer_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CCSharpServiceWrapper

class ATL_NO_VTABLE CCSharpServiceWrapper :
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<CCSharpServiceWrapper, &CLSID_CSharpServiceWrapper>,
	public ISupportErrorInfo,
	public IDispatchImpl<ICSharpServiceWrapper, &IID_ICSharpServiceWrapper, &LIBID_HostServerLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CCSharpServiceWrapper() = default;

    STDMETHODIMP Ping(VARIANT_BOOL* result) override;
    STDMETHODIMP StringLength(BSTR value, INT* length) override;

    DECLARE_REGISTRY_RESOURCEID(106)

    DECLARE_NOT_AGGREGATABLE(CCSharpServiceWrapper)

    BEGIN_COM_MAP(CCSharpServiceWrapper)
        COM_INTERFACE_ENTRY(ICSharpServiceWrapper)
        COM_INTERFACE_ENTRY(IDispatch)
        COM_INTERFACE_ENTRY(ISupportErrorInfo)
    END_COM_MAP()

    // ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid) noexcept override;


	DECLARE_PROTECT_FINAL_CONSTRUCT()

    // ReSharper disable once CppHidingFunction
    // ReSharper disable once CppMemberFunctionMayBeStatic
    HRESULT FinalConstruct()
	{
		return S_OK;
	}

    // ReSharper disable once CppHidingFunction
    // ReSharper disable once CppMemberFunctionMayBeStatic
    void FinalRelease()
	{
	}


};

OBJECT_ENTRY_AUTO(__uuidof(CSharpServiceWrapper), CCSharpServiceWrapper)
