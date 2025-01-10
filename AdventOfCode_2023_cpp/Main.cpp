// AdventOfCode_2023_cpp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

import day01_part01;
import day01_part02;
import day02_part01;
import day02_part02;
import day03_part01;
import day03_part02;

int main() {
    std::cout << "Advent of Code 2023!" << std::endl;
    int resultDay01Part01 = day01_part01::execute();
    std::cout << "Day 01 Part 01: " << resultDay01Part01 << std::endl;
    int resultDay01Part02 = day01_part02::execute();
    std::cout << "Day 01 Part 02: " << resultDay01Part02 << std::endl;

    int resultDay02Part01 = day02_part01::execute();
    std::cout << "Day 02 Part 01: " << resultDay02Part01 << std::endl;
    int resultDay02Part02 = day02_part02::execute();
    std::cout << "Day 02 Part 02: " << resultDay02Part02 << std::endl;

    int resultDay03Part01 = day03_part01::execute();
    std::cout << "Day 03 Part 01: " << resultDay03Part01 << std::endl;
    int resultDay03Part02 = day03_part02::execute();
    std::cout << "Day 03 Part 02: " << resultDay03Part02 << std::endl;
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
