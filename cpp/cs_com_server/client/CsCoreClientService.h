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

#pragma once
#pragma pack(push, 8)

#include <comdef.h>

namespace core::client_service
{
    struct __declspec(uuid("999B9B4F-F983-49F2-889B-087C3C4FB57C")) ICoreClientService;
    struct CoreClientService;

    using core_service_ptr = _com_ptr_t<_com_IIID<ICoreClientService, &__uuidof(ICoreClientService)> >;

    struct __declspec(uuid("999B9B4F-F983-49F2-889B-087C3C4FB57C"))
    ICoreClientService : IUnknown
    {
        _bstr_t ToLower(_bstr_t input)
        {
            BSTR result = 0;
            if (auto const hr = raw_ToLower(input, &result); FAILED(hr))
                _com_issue_errorex(hr, this, __uuidof(this));
            return _bstr_t(result, false);
        }

        virtual HRESULT __stdcall raw_ToLower(BSTR input, BSTR* output) = 0;
    };

    struct __declspec(uuid("8C1466E8-87CC-4CBC-B4E6-124024CFEDF3"))
    CoreClientService;
}
