
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

#include <Windows.h>

namespace host_server
{
    class process final 
    {
    public:
        using native_handle_type = HANDLE;
        static constexpr native_handle_type invalid_handle() { return nullptr; }

        explicit process(DWORD const process_id); 
        explicit process(native_handle_type process_handle = invalid_handle()) noexcept;
        process(process const&) = delete;
        process(process&& other) noexcept;
        ~process();

        [[nodiscard]] bool is_running() const;
        [[nodiscard]] bool reset(native_handle_type process_handle = invalid_handle()) noexcept;
        [[nodiscard]] native_handle_type release() noexcept;

        [[nodiscard]] explicit operator bool() const noexcept;

        process& operator=(process const&) = delete;
        process& operator=(process&& other) noexcept;
    private:
        native_handle_type m_process_handle;

        void close();
    };
}
