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
#pragma once

// ReSharper disable CppClangTidyClangDiagnosticLanguageExtensionToken
// ReSharper disable CppInconsistentNaming
// ReSharper disable CppPolymorphicClassWithNonVirtualPublicDestructor

using namespace ATL;

template <class T>
class CProxy_ISimpleOOPObjectEvents // NOLINT(clang-diagnostic-non-virtual-dtor)
    : public IConnectionPointImpl<T, &__uuidof(_ISimpleOOPObjectEvents), CComDynamicUnkArray> {

    using base = IConnectionPointImpl<T, &__uuidof(_ISimpleOOPObjectEvents), CComDynamicUnkArray>;
public:
    HRESULT Fire_OnPropertyChanaged(BSTR propertyName) {
        T* p_this      = static_cast<T*>(this);

        for (int i = 0, size = base::m_vec.GetSize(); i < size; i++) {

            p_this->Lock();
            ATL::CComPtr<IUnknown> const unknown = base::m_vec.GetAt(i);
            p_this->Unlock();

            if (auto const dispatch = reinterpret_cast<IDispatch*>(unknown.p);
                dispatch != nullptr) {

                constexpr DISPID disp_id = 1; // see IDL file for id value
                ATL::CComVariant parameters[1] = {ATL::CComVariant(propertyName)};
                ATL::CComVariant result{};
                DISPPARAMS params = {parameters, nullptr, 1, 0};
                dispatch->Invoke(
                    disp_id,
                    IID_NULL,
                    LOCALE_USER_DEFAULT,
                    DISPATCH_METHOD,
                    &params,
                    &result,
                    nullptr, nullptr);
            }
        }
        return S_OK;
    }
};
