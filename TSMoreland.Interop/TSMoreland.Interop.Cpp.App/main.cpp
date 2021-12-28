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
#include <iostream>
#include <string_view>
    
#import "libid:580185ad-317a-4eb7-a6ab-48ebd08c8407" lcid("0")
#import "libid:4faab4cd-f38e-4709-a0e3-b15763ec7452" lcid("0")

GUID uuid_from_string(char const* source);


int main() {

    GUID const in_process_id{uuid_from_string("e3d3572d-9e25-4cf3-82f5-45b6f0035a82")};
    GUID const events_id{uuid_from_string("71A4D526-4FAD-4D4B-8A6E-78AFCABD7F63")};

    SimpleInProcessCOMLib::_ISimpleObjectEventsPtr events_ptr;
    SimpleInProcessCOMLib::ISimpleObject2Ptr simple_object{};
    if (simple_object.CreateInstance(in_process_id, nullptr) != S_OK) {
        return -1;
    }

    _bstr_t name = simple_object->Name;
    std::wstring_view const name_view{name.GetBSTR(), name.length()};
    std::wcout << L"Name: " << name_view << std::endl;

    simple_object->Numeric = 42L;
    std::wcout << L"Numeric: " << simple_object->Numeric << std::endl;

    simple_object.Release();

    return 0;
}

GUID uuid_from_string(char const* source) {

	GUID guid;
	sscanf_s(source,
        "{%8x-%4hx-%4hx-%2hhx%2hhx-%2hhx%2hhx%2hhx%2hhx%2hhx%2hhx}",
        &guid.Data1, &guid.Data2, &guid.Data3, &guid.Data4[0],
        &guid.Data4[1], &guid.Data4[2], &guid.Data4[3],
        &guid.Data4[4], &guid.Data4[5], &guid.Data4[6], &guid.Data4[7] );
    return guid;
}
