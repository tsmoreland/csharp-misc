

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0622 */
/* at Tue Jan 19 03:14:07 2038
 */
/* Compiler settings for ClientService.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.01.0622 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */


#ifndef __ClientService_i_h__
#define __ClientService_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IService_FWD_DEFINED__
#define __IService_FWD_DEFINED__
typedef interface IService IService;

#endif 	/* __IService_FWD_DEFINED__ */


#ifndef __Service_FWD_DEFINED__
#define __Service_FWD_DEFINED__

#ifdef __cplusplus
typedef class Service Service;
#else
typedef struct Service Service;
#endif /* __cplusplus */

#endif 	/* __Service_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "shobjidl.h"

#ifdef __cplusplus
extern "C"{
#endif 



#ifndef __ClientServiceLib_LIBRARY_DEFINED__
#define __ClientServiceLib_LIBRARY_DEFINED__

/* library ClientServiceLib */
/* [custom][version][uuid] */ 


EXTERN_C const IID LIBID_ClientServiceLib;

#ifndef __IService_INTERFACE_DEFINED__
#define __IService_INTERFACE_DEFINED__

/* interface IService */
/* [custom][unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IService;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("23ca7d29-7f18-4112-928f-5221f589b494")
    IService : public IDispatch
    {
    public:
        virtual /* [id] */ HRESULT STDMETHODCALLTYPE ToUpper( 
            /* [in] */ BSTR input,
            /* [retval][out] */ BSTR *output) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IServiceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IService * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IService * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IService * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IService * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IService * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IService * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IService * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [id] */ HRESULT ( STDMETHODCALLTYPE *ToUpper )( 
            IService * This,
            /* [in] */ BSTR input,
            /* [retval][out] */ BSTR *output);
        
        END_INTERFACE
    } IServiceVtbl;

    interface IService
    {
        CONST_VTBL struct IServiceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IService_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IService_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IService_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IService_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IService_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IService_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IService_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IService_ToUpper(This,input,output)	\
    ( (This)->lpVtbl -> ToUpper(This,input,output) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IService_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_Service;

#ifdef __cplusplus

class DECLSPEC_UUID("886ce33c-5e78-4690-8ebd-66ab81ce26da")
Service;
#endif
#endif /* __ClientServiceLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


