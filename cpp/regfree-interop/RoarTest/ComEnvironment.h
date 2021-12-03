#pragma once

#include <atlbase.h>

namespace Bear::Test
{
    class ComEnvironment final
    {
    public:
        explicit ComEnvironment();
        ~ComEnvironment();
        ComEnvironment(const ComEnvironment&) = delete;
        ComEnvironment(ComEnvironment&&) noexcept = delete;
        ComEnvironment& operator=(const ComEnvironment&) = delete;
        ComEnvironment& operator=(ComEnvironment&&) noexcept = delete;

        static void WriteErrorToConsole(const HRESULT hr);
    };

}

