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
#include "process_monitor.h"
#include "windows_exception.h"
#include <stdexcept>

constexpr auto process_poll_period() 
{
    return std::chrono::seconds(10);
}

namespace host_server
{

process_monitor& process_monitor::get_instance() noexcept
{
    static process_monitor singleton{};
    return singleton;
}

void process_monitor::exit_when_process_exits(process&& owner)
{
    if (m_monitor_thread.joinable()) {
        throw std::runtime_error("process already registered");
    }

    using std::chrono::seconds;
    m_monitor_thread = std::thread(
        [](process&& owner) {
            int consecutive_poll_failures{0};
            bool done{false};
            while (!done) {
                std::this_thread::sleep_for(process_poll_period());
                try {
                    if (!owner.is_running()) {
                        ExitProcess(0U);
                    }
                    consecutive_poll_failures = 0;
                } catch (modern_win32::windows_exception const&) {
                    consecutive_poll_failures++;
                }
            }

        }, std::move(owner));
}

}
