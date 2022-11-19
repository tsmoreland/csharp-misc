#include <iostream>
#include "csharp_interop_aot.h"

using calculator = tsmoreland::samples::csharp_interop_aot::calculator;

int main() {

    try {

        calculator const calc{};

        int result = calc.add(1, 2);
        std::cout << result << "\n";
    } catch (std::exception const& ex) {
        std::cout << ex.what() << "\n";
    }

    return 0;
}

