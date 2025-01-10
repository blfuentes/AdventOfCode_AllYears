#include <vector>
#include <string>
#include <regex>
#include <iostream>
#include <sstream>
#include <unordered_map>

import Utilities;

using namespace std;

export module day03_part01;

struct COORD {
	string element;
	int row;
	int col;
};

struct REGION {
	string id;
	int row;
	int col;
	int width;
	vector<COORD> neighbours;
};

static bool isSymbol(string element) {
	if (Utilities::isNumber(element))
		return false;
	else
		return element != ".";
}

static bool containsCoord(vector<COORD> coords, COORD coord) {
	for (COORD c : coords) {
		if (c.row == coord.row && c.col == coord.col)
			return true;
	}
	return false;
}

static REGION buildRegion(string** schematic, int row, int col, string number, int maxRow, int maxCol) {
	REGION region;
	region.id = number;
	region.row = row;
	region.col = col;
	region.width = number.length();

	for (int j = col; j < (col + region.width); ++j) {
		for (int dr = -1; dr <= 1; ++dr) {
			for (int dc = -1; dc <= 1; ++dc) {
				if (dr == 0 && dc == 0)
					continue;

				int checkRow = row + dr;
				int checkCol = j + dc;
				if (checkRow < 0 || checkRow >= maxRow || checkCol < 0 || checkCol >= maxCol) {
					continue;
				}

				string element = schematic[checkRow][checkCol];
				if (isSymbol(element)) {
					COORD coord;
					coord.element = element;
					coord.row = checkRow;
					coord.col = checkCol;
					if (!containsCoord(region.neighbours, coord))
						region.neighbours.push_back(coord);
				}
			}

		}
	}
	return region;

}

namespace day03_part01 {
	export int execute() {
		vector<string> lines = Utilities::readTextFile("day03/day03_input.txt");

		// get numbers for regions
		std::regex rgx("\\d+");
		std::sregex_iterator end;

		// defineSchematics
		int rows = lines.size();
		int cols = lines[0].size();
		string** engineSchematic = new string * [rows];
		for (int i = 0; i < rows; ++i) {
			engineSchematic[i] = new string[cols];
		}

		// build schematic
		int row = 0;
		for (string line : lines) {
			int col = 0;
			for (char c : line) {
				engineSchematic[row][col] = std::string(1, c);
				col++;
			}

			++row;
		}

		// build regions
		row = 0;
		vector<REGION> regions;
		for (string line : lines) {
			std::istringstream lineStream(line);
			std::string part;
			// Create regex iterator for the entire string
			std::sregex_iterator iter(line.begin(), line.end(), rgx);
			while (iter != end) {
				std::smatch match = *iter;
				int colOfNumerRegion = match.position();
				regions.push_back(buildRegion(engineSchematic, row, colOfNumerRegion, match.str(), rows, cols));
				++iter;
			}
			++row;
		}
		int linkedRegions = 0;
		for (REGION region : regions) {
			if (region.neighbours.size() > 0)
				linkedRegions += stoi(region.id);
		}
		return linkedRegions;
	}
}