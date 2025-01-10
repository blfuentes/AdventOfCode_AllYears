#include <vector>
#include <string>
#include <regex>
#include <algorithm>

import Utilities;

using namespace std;

export module day01_part02;

namespace day01_part02 {
	int static convertRevNumber(string input) {
		if (input == "eno" or input == "one")
			return 1;
		else if (input == "owt" or input == "two")
			return 2;
		else if (input == "eerht" or input == "three")
			return 3;
		else if (input == "ruof" or input == "four")
			return 4;
		else if (input == "evif" or input == "five")
			return 5;
		else if (input == "xis" or input == "six")
			return 6;
		else if (input == "neves" or input == "seven")
			return 7;
		else if (input == "thgie" or input == "eight")
			return 8;
		else if (input == "enin" or input == "nine")
			return 9;
		else
			return stoi(input);
	}
	vector<int> static findNumbers(string line) {
		vector<int> numbers;

		regex firstDigit("(\\d|one|two|three|four|five|six|seven|eight|nine)"); // matches digits from left to right
		regex lastDigit("(\\d|eno|owt|eerht|ruof|evif|xis|neves|thgie|enin)"); // matches digits from right to left

		string reverseLine = line;
		reverse(reverseLine.begin(), reverseLine.end());

		// iterator
		regex_iterator<string::iterator> firstit(line.begin(), line.end(), firstDigit);
		regex_iterator<string::iterator> lastit(reverseLine.begin(), reverseLine.end(), lastDigit);

		// add first and last digit
		numbers.push_back(convertRevNumber(firstit->str()));
		numbers.push_back(convertRevNumber(lastit->str()));

		return numbers;
	}

	int static extractNumber(vector<int> numbers) {
		if (numbers.size() == 0)
			return 0;
		else
			return numbers[0] * 10 + numbers[numbers.size() - 1];
	}

	export int execute() {
		vector<string> lines = Utilities::readTextFile("day01/day01_input.txt");
		int result = 0;
		for (string line : lines) {
			vector<int> numbers = findNumbers(line);
			result += extractNumber(numbers);
		}

		return result;
	}
}
