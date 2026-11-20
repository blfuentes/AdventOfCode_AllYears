#include <vector>
#include <string>
#include <regex>
#include <iostream>
#include <sstream>
#include <unordered_map>

import Utilities;

using namespace std;

export module day02_part02;

namespace day02_part02 {
	class GameSet {
	public:
		GameSet(int id, vector<std::unordered_map<std::string, int>> cubes)
			: _idGame(id), _gameCubes(cubes)
		{
		}

		int idGame() const {
			return _idGame;
		}

		vector<std::unordered_map<std::string, int>> gameCubes() const {
			return _gameCubes;
		}
	private:
		int _idGame;
		vector<std::unordered_map<std::string, int>> _gameCubes;
	};

	static GameSet buildGameSet(string line) {
		int gameId = 0;
		vector<std::unordered_map<std::string, int>> gameCubes;

		// extract game part
		std::istringstream gameParts(line);
		std::vector<std::string> parts;
		std::string part;
		while (std::getline(gameParts, part, ' ')) {
			parts.push_back(part);
		}
		gameId = stoi(parts[1]);

		// extract cubes part
		regex rgx("(( \\d+ (blue|green|red),\\s*)+)?( \\d+ (blue|green|red)\\s*)");
		std::sregex_iterator iter(line.begin(), line.end(), rgx);
		std::sregex_iterator end;

		while (iter != end) {
			std::istringstream cubeSet(iter->str());
			std::string cube;
			std::unordered_map<std::string, int> cubesOfCubSet;
			while (std::getline(cubeSet, cube, ',')) {
				std::istringstream partDef(cube);
				string numOfCubes;
				string color;
				partDef >> numOfCubes >> color;
				cubesOfCubSet[color] = stoi(numOfCubes);
			}

			// Move to the next match
			++iter;
			gameCubes.push_back(cubesOfCubSet);
		}

		return GameSet(gameId, gameCubes);
	}

	export int execute() {
		vector<string> lines = Utilities::readTextFile("day02/day02_input.txt");
		//vector<string> lines = Utilities::readTextFile("day02/test_input_01.txt");
		int result = 0;
		vector<GameSet> gameSets;
		for (string line : lines)
			gameSets.push_back(buildGameSet(line));

		int sumOfCubes = 0;
		std::unordered_map<std::string, int> miniumCubes;
		for (GameSet gameSet : gameSets) {
			miniumCubes["red"] = 0;
			miniumCubes["green"] = 0;
			miniumCubes["blue"] = 0;

			for (std::unordered_map<std::string, int> cubes : gameSet.gameCubes()) {
				for (auto const& [key, val] : cubes) {
					int currentVal = miniumCubes[key];
					if (currentVal < val)
						miniumCubes[key] = val;
				}
			}
			sumOfCubes += miniumCubes["red"] * miniumCubes["green"] * miniumCubes["blue"];
		}

		return sumOfCubes;
	}
}