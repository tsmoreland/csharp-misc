#pragma once

namespace tsmoreland::samples::csharp_interop_aot {

    void initialize_csharp_interop_aot();

    class calculator_impl;

    class calculator final {
        calculator_impl* impl_{};

    public:
        explicit calculator();
        ~calculator();
        calculator(calculator const&) = delete;
        calculator& operator=(calculator const&) = delete;
        calculator(calculator&&) noexcept;
        calculator& operator=(calculator&&) noexcept;
        
        [[nodiscard]]
        int add(int const x, int const y) const;
    };
}

