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

#include "pch.h"
#include "process.h"
#include "windows_exception.h"
#include <stdexcept>

namespace host_server
{

process::process(DWORD const process_id)
    : m_process_handle(OpenProcess(PROCESS_QUERY_INFORMATION, false, process_id))
{
    if (m_process_handle == nullptr)
        throw std::exception("Unable to open process");
}

process::process(native_handle_type process_handle /* = invalid_handle() */) noexcept
    : m_process_handle(process_handle)
{
}

process::process(process&& other) noexcept
    : m_process_handle(other.release())
{
}
process& process::operator=(process&& other) noexcept
{
    if (this != &other)
        static_cast<void>(reset(other.release()));
    return *this;
}


process::~process()
{
    close();
}

bool process::is_running() const
{
    if (!static_cast<bool>(this))
        return false;
    auto const process_handle = m_process_handle;

    DWORD exit_code{};
    if (GetExitCodeProcess(process_handle, &exit_code) == 0)
        throw modern_win32::windows_exception("unable to determine process exit code");

    return exit_code == STILL_ACTIVE;
}

bool process::reset(native_handle_type process_handle /* = invalid_handle() */) noexcept
{
    if (m_process_handle != process_handle) {
        close();
        m_process_handle = process_handle;
    }
    return static_cast<bool>(this);
}

process::native_handle_type process::release() noexcept
{
    auto process_handle = m_process_handle;
    m_process_handle = invalid_handle();
    return process_handle;
}

process::operator bool() const noexcept
{
    return m_process_handle != invalid_handle();
}

void process::close()
{
    CloseHandle(m_process_handle);
}

}
