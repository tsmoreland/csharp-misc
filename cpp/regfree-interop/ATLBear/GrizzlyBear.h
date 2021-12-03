#pragma once

#include <string>

namespace Bears::Native
{
    class GrizzlyBear final
    {
    public:
        explicit GrizzlyBear() = default;

        __declspec(property(get = GetName, put = SetName)) std::wstring Name;
        [[nodiscard]] const std::wstring& GetName() const;
        void SetName(std::wstring value);

        void Roar() const;
    private:
        std::wstring m_name{L""};
    };
}

