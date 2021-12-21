// SimpleObject.h : Declaration of the CSimpleObject

#pragma once
#include "pch.h"
#include "SimpleInProcessCOM_i.h"
#include "resource.h" // main symbols


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error \
    "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CSimpleObject

class ATL_NO_VTABLE CSimpleObject : public CComObjectRootEx<CComMultiThreadModel>,    // NOLINT(clang-diagnostic-non-virtual-dtor)
                                    public CComCoClass<CSimpleObject, &CLSID_SimpleObject>,
                                    public IDispatchImpl<ISimpleObject2, &IID_ISimpleObject2, &LIBID_SimpleInProcessCOMLib, /*wMajor =*/1, /*wMinor =*/0> {
    LONG numeric_{0};

public:

    /// <summary>
    /// Returns the name of this object
    /// </summary>
    /// <param name="result">on success stores the name of this object</param>
    /// <returns>
    /// S_OK on success, otherwise E_INVALIDARG if <paramref name="result"/> is nullptr
    /// </returns>
    STDMETHOD(get_Name)(BSTR* result) noexcept override;

    /// <summary>
    /// returns the current numeric value
    /// </summary>
    /// <param name="result">on success stores the current numeric value</param>
    /// <returns>
    /// S_OK on success, otherwise E_INVALIDARG if <paramref name="result"/> is nullptr
    /// </returns>
    STDMETHOD(get_Numeric)(LONG* result) noexcept override;

    /// <summary>
    /// updates the numeric value
    /// </summary>
    /// <param name="value">value to update</param>
    /// <returns>S_OK</returns>a
    STDMETHOD(put_Numeric)(LONG value) noexcept override;

    /// <summary>
    /// Returns the unique id of this object
    /// </summary>
    /// <param name="result">on success stores the id</param>
    /// <returns>
    /// S_OK on success, otherwise either E_INVALIDARG if <paramref name="result"/> is nullptr
    /// or E_FAIL if unable to create GUID
    /// </returns>
    STDMETHOD(get_Id)(GUID* result) noexcept override;

    /// <summary>
    /// Returns string representation of <paramref name="input"/>
    /// </summary>
    /// <param name="input">GUID to convert</param>
    /// <param name="result">on success stores the string representation</param>
    /// <returns>S_OK on success; otherwise, E_INVALIDARG if result is nullptr, and E_FAIL for any other failure</returns>
    STDMETHOD(ConvertToString)(GUID input, BSTR* result) noexcept override;


    /// <summary>
    /// Returns description
    /// </summary>
    /// <param name="result">on success stores the description</param>
    /// <returns>
    /// S_OK on success, otherwise E_INVALIDARG if <paramref name="result"/> is nullptr
    /// </returns>
    STDMETHOD(get_Description)(BSTR* result) noexcept;

    CSimpleObject() = default;

    DECLARE_REGISTRY_RESOURCEID(106)

    DECLARE_NOT_AGGREGATABLE(CSimpleObject)

    BEGIN_COM_MAP(CSimpleObject)
    COM_INTERFACE_ENTRY(ISimpleObject)
    COM_INTERFACE_ENTRY(ISimpleObject2)
    COM_INTERFACE_ENTRY(IDispatch)
    END_COM_MAP()


    DECLARE_PROTECT_FINAL_CONSTRUCT()

    HRESULT FinalConstruct();

    void FinalRelease();
};

OBJECT_ENTRY_AUTO(__uuidof(SimpleObject), CSimpleObject)
