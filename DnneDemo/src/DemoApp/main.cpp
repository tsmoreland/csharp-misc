#include <iostream>
#include <format>
#include "netframework_library.h"

int main()
{
    int const x = 4;
    int y = get_2x(x);
    std::cout << std::format("y = 2x -> {0} * 2 = {1}", x, y) << "\n";

    return 0;
}

