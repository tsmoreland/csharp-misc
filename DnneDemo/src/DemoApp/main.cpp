#include <iostream>
#include <format>
#include "netframework_library.h"

constexpr int True = 0;
constexpr int False = 1;

int main()
{
    constexpr int x = 4;
    int y = get_2x(x);
    std::cout << std::format("y = 2x -> {0} * 2 = {1}", x, y) << "\n";

    DataTransferObject dto{};

    if (int const success = get_dto(4, &dto); success != True) {
        std::cout << "failed to get dto" << "\n";
        return False;
    }

    std::cout << std::format("dto ( length: {0}, isValid: {1} )", dto.length, dto.isValid) << "\n";
    return 0;
}

