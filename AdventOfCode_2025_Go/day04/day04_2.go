package day04

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int {
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

		counterMap := make([][]int, 0)
		var toDelete utilities.Queue[[]int] = utilities.Queue[[]int]{}
		for rowIdx, line := range fileContent {
			counterMap = append(counterMap, make([]int, len(line)))
			for colIdx, char := range line {
				if char == '@' {
					counter := countPapers(rowIdx, colIdx)
					if counter < 4 {
						toDelete.Enqueue([]int{rowIdx, colIdx})
					}
					counterMap[rowIdx][colIdx] = counter
				} else {
					counterMap[rowIdx][colIdx] = -1
				}
			}
		}
		for paper, ok := toDelete.Dequeue(); ok; paper, ok = toDelete.Dequeue() {
			result++
			for _, delta := range neighbors {
				nr := paper[0] + delta[0]
				nc := paper[1] + delta[1]
				if nr >= 0 && nr <= maxRow && nc >= 0 && nc <= maxCol {
					if counterMap[nr][nc] > 0 {
						if counterMap[nr][nc] == 4 {
							toDelete.Enqueue([]int{nr, nc})
						}
						counterMap[nr][nc]--
					}
				}
			}
		}
	}
	return result
}
