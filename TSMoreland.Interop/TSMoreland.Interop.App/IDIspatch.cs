//
// Copyright © 2021 Terry Moreland
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

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace TSMoreland.Interop.App;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("00020400-0000-0000-C000-000000000046")]
public interface IDispatch
{
    // Gets the number of Types that the object provides (0 or 1).
    [PreserveSig]
    int GetTypeInfoCount(out int typeInfoCount);

    // Gets the Type information for an object if GetTypeInfoCount returned 1.
    void GetTypeInfo(int typeInfoIndex, int lcid,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.TypeToTypeInfoMarshaler")]
        out Type typeInfo);

    // Gets the DISPID of the specified member name.
    [PreserveSig]
    int GetDispId(ref Guid riid, ref string name, int nameCount, int lcid, 
        out int dispId);

    [PreserveSig]
    int Invoke(
        int dispIdMember,
        ref Guid riid,
        uint lcid,
        ushort wFlags,
        ref System.Runtime.InteropServices.ComTypes.DISPPARAMS pDispParams,
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(GuidMarshaler))]
        out object pVarResult,
        ref System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo,
        out uint pArgErr);

    /*
 HRESULT Invoke(
  [in]      DISPID     dispIdMember,
  [in]      REFIID     riid,
  [in]      LCID       lcid,
  [in]      WORD       wFlags,
  [in, out] DISPPARAMS *pDispParams,
  [out]     VARIANT    *pVarResult,
  [out]     EXCEPINFO  *pExcepInfo,
  [out]     UINT       *puArgErr
);   
    */


    // NOTE: The real IDispatch also has an Invoke method next, but we don't need it.
}
