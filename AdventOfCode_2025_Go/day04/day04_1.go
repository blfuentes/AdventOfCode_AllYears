package day04

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day04/test_input_04.txt"
	var fileName string = "./day04/day04.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		maxRow := len(fileContent) - 1
		maxCol := len(fileContent[0]) - 1
		neighbors := [8][2]int{
			{-1, -1}, {-1, 0}, {-1, 1},
			{0, -1} /*{0,0},*/, {0, 1},
			{1, -1}, {1, 0}, {1, 1},
		}
		var countPapers = func(r, c int) (counter int) {
			for _, neighbor := range neighbors {
				nr := r + neighbor[0]
				nc := c + neighbor[1]
				if nr >= 0 && nr <= maxRow && nc >= 0 && nc <= maxCol {
					if fileContent[nr][nc] == '@' {
						counter++
					}
				}
			}
			return
		}

		for rowIdx, line := range fileContent {
			for colIdx, char := range line {
				if char == '@' {
					if countPapers(rowIdx, colIdx) < 4 {
						result++
					}
				}
			}
		}
	}
	return result
}
