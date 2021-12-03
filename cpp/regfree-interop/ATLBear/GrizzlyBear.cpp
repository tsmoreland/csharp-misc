#include "pch.h"
#include "GrizzlyBear.h"
#include <memory>
#include <iostream>

namespace Bears::Native
{
    using std::wstring;
    using std::move;
    using std::wcout;
    using std::endl;

    const wstring& GrizzlyBear::GetName() const
    {
        return m_name;
    }

    void GrizzlyBear::SetName(wstring value)
    {
        m_name = move(value);
    }

    void GrizzlyBear::Roar() const
    {
        wcout << m_name << L" Roars!" << endl;
    }

}
