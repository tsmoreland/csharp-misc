#include "csharp_interop_aot.h"

#if defined(_WIN32)
#define PATH_TO_LIBRARY "TSMoreland.Samples.CSharpInteropAot.dll"
#elif defined(__APPLE__)
#define PATH_TO_LIBRARY "TSMoreland.Samples.CSharpInteropAot.dylib"
#else
#define PATH_TO_LIBRARY "TSMoreland.Samples.CSharpInteropAot.so"
#endif

#ifdef _WIN32
#include "windows.h"
#define SYM_LOAD GetProcAddress
#include <cstdlib>
#include <cstdio>
#else
#include "dlfcn.h"
#include <unistd.h>
#define SYM_LOAD dlsym
#include <stdlib.h>
#include <stdio.h>
#endif


#ifndef F_OK
#define F_OK    0
#endif

#include <memory>
#include <exception>

namespace tsmoreland::samples::csharp_interop_aot {

    int call_sum_func(char* path, char const* const func_name, int a, int b);


    void initialize_csharp_interop_aot() {
        
    }

    class calculator_impl final {
        using cs_add = int (*)(int, int);

        cs_add add_{};
#ifdef _WIN32
        HINSTANCE handle_{};
#else
        void* handle_{};
#endif
    public:
        explicit calculator_impl(char const * const path) {
#ifdef _WIN32
            handle_ = LoadLibraryA(path);
#else
            handle_ = dlopen(path, RTLD_LAZY)
#endif
            // CoreRT libraries do not support unloading
            // See https://github.com/dotnet/corert/issues/7887

            auto* proc_address = SYM_LOAD(handle_, "add");

            add_ = reinterpret_cast<cs_add>(proc_address);
            if (add_ == nullptr) {
                throw std::exception("Unable to load add");
            }
        }
        
        [[nodiscard]]
        int add(int const x, int const y) const {
            return add_(x, y);
        }
    };


    calculator::calculator() : impl_{new calculator_impl(PATH_TO_LIBRARY)} {}
    calculator::~calculator() {
        delete impl_;
    }
    calculator::calculator(calculator&& other) noexcept : impl_{other.impl_} {
        other.impl_ = nullptr;
    }
    calculator& calculator::operator=(calculator&& other) noexcept {
        if (&other == this) {
            return *this;
        }

        calculator_impl* previous = nullptr;
        std::swap(previous, impl_);
        std::swap(impl_, other.impl_);

        delete previous;

        return *this;
    }
    int calculator::add(int const x, int const y) const {
        if (impl_ != nullptr) {
            return impl_->add(x, y);
        }

        throw std::exception("object has been released.");
        
    }

} // namespace tsmoreland::samples::csharp_interop_aot
