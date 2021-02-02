

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0622 */
/* at Tue Jan 19 03:14:07 2038
 */
/* Compiler settings for ATLBear.idl:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 8.01.0622 
    protocol : all , ms_ext, c_ext, robust
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

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __ATLBear_i_h__
#define __ATLBear_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IGrizzly_FWD_DEFINED__
#define __IGrizzly_FWD_DEFINED__
typedef interface IGrizzly IGrizzly;

#endif 	/* __IGrizzly_FWD_DEFINED__ */


#ifndef __Grizzly_FWD_DEFINED__
#define __Grizzly_FWD_DEFINED__

#ifdef __cplusplus
typedef class Grizzly Grizzly;
#else
typedef struct Grizzly Grizzly;
#endif /* __cplusplus */

#endif 	/* __Grizzly_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "shobjidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IGrizzly_INTERFACE_DEFINED__
#define __IGrizzly_INTERFACE_DEFINED__

/* interface IGrizzly */
/* [custom][unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGrizzly;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("6b42a33c-c7e3-4c64-9081-84174e5c9b97")
    IGrizzly : public IDispatch
    {
    public:
        virtual /* [propget][id] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *pRetVal) = 0;
        
        virtual /* [propput][id] */ HRESULT STDMETHODCALLTYPE put_Name( 
            /* [in] */ BSTR value) = 0;
        
        virtual /* [id] */ HRESULT STDMETHODCALLTYPE Roar( void) = 0;
        
        virtual /* [id] */ HRESULT STDMETHODCALLTYPE Oneify( 
            /* [in] */ SAFEARRAY * pSource,
            /* [retval][out] */ VARIANT_BOOL *pRetVal) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IGrizzlyVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGrizzly * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGrizzly * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGrizzly * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGrizzly * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGrizzly * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGrizzly * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGrizzly * This,
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
        
        /* [propget][id] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            IGrizzly * This,
            /* [retval][out] */ BSTR *pRetVal);
        
        /* [propput][id] */ HRESULT ( STDMETHODCALLTYPE *put_Name )( 
            IGrizzly * This,
            /* [in] */ BSTR value);
        
        /* [id] */ HRESULT ( STDMETHODCALLTYPE *Roar )( 
            IGrizzly * This);
        
        /* [id] */ HRESULT ( STDMETHODCALLTYPE *Oneify )( 
            IGrizzly * This,
            /* [in] */ SAFEARRAY * pSource,
            /* [retval][out] */ VARIANT_BOOL *pRetVal);
        
        END_INTERFACE
    } IGrizzlyVtbl;

    interface IGrizzly
    {
        CONST_VTBL struct IGrizzlyVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGrizzly_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGrizzly_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGrizzly_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGrizzly_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGrizzly_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGrizzly_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGrizzly_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGrizzly_get_Name(This,pRetVal)	\
    ( (This)->lpVtbl -> get_Name(This,pRetVal) ) 

#define IGrizzly_put_Name(This,value)	\
    ( (This)->lpVtbl -> put_Name(This,value) ) 

#define IGrizzly_Roar(This)	\
    ( (This)->lpVtbl -> Roar(This) ) 

#define IGrizzly_Oneify(This,pSource,pRetVal)	\
    ( (This)->lpVtbl -> Oneify(This,pSource,pRetVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGrizzly_INTERFACE_DEFINED__ */



#ifndef __ATLBearLib_LIBRARY_DEFINED__
#define __ATLBearLib_LIBRARY_DEFINED__

/* library ATLBearLib */
/* [custom][version][uuid] */ 


EXTERN_C const IID LIBID_ATLBearLib;

EXTERN_C const CLSID CLSID_Grizzly;

#ifdef __cplusplus

class DECLSPEC_UUID("97c0edc5-dd19-4a03-9b21-ba371ebbcc9e")
Grizzly;
#endif
#endif /* __ATLBearLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  LPSAFEARRAY_UserSize(     unsigned long *, unsigned long            , LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserMarshal(  unsigned long *, unsigned char *, LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserUnmarshal(unsigned long *, unsigned char *, LPSAFEARRAY * ); 
void                      __RPC_USER  LPSAFEARRAY_UserFree(     unsigned long *, LPSAFEARRAY * ); 

unsigned long             __RPC_USER  BSTR_UserSize64(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal64(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal64(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree64(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  LPSAFEARRAY_UserSize64(     unsigned long *, unsigned long            , LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserMarshal64(  unsigned long *, unsigned char *, LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserUnmarshal64(unsigned long *, unsigned char *, LPSAFEARRAY * ); 
void                      __RPC_USER  LPSAFEARRAY_UserFree64(     unsigned long *, LPSAFEARRAY * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


