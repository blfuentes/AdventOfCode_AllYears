#include <vector>
#include <string>
#include <regex>

import Utilities;

using namespace std;

export module day01_part01;

namespace day01_part01 {
	vector<int> static findNumbers(string line) {
		vector<int> numbers;
		regex e("\\d"); // matches digits

		// iterator
		regex_iterator<string::iterator> rit(line.begin(), line.end(), e);
		regex_iterator<string::iterator> rend;

		while (rit != rend) {
			numbers.push_back(stoi(rit->str()));
			++rit;
		}

		return numbers;
	}

	int static extractNumber (vector<int> numbers) {
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
