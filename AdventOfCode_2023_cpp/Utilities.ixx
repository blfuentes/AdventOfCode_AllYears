#include <fstream>
#include <vector>
#include <string>

using namespace std;

export module Utilities;

namespace Utilities {
	// function to read the content of text file
    export vector<string> readTextFile(const string& filePath) {
        vector<string> lines;
        ifstream inFile(filePath);
        if (inFile.is_open()) {
            string line;
            while (getline(inFile, line)) {
                lines.push_back(line);
            }
            inFile.close();
        }
        return lines;
    }

    export int rowDimension (string** array2D) {
		return sizeof(array2D);
	}

    export int colDimension(string** array2D) {
        return sizeof(array2D[0][0]);
    }

    export bool isNumber(const std::string& s) {
		return !s.empty() && std::find_if(s.begin(),
			s.end(), [](unsigned char c) { return !std::isdigit(c); }) == s.end();
    }
}