package day07

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day07/test_input_07.txt"
	var fileName string = "./day07/day07.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		rowStar := 0
		colStar := 0
		for cIdx, char := range fileContent[0] {
			if char == 'S' {
				colStar = cIdx
				break
			}
		}
		positionsToTry := utilities.Queue[[2]int]{}
		positionsToTry.Enqueue([2]int{rowStar, colStar})
		splitters := make(map[[2]int]bool)
		for position, ok := positionsToTry.Dequeue(); ok; position, ok = positionsToTry.Dequeue() {
			newRow, newCol := position[0]+1, position[1]
			if newRow >= len(fileContent) {
				break
			}
			if fileContent[newRow][newCol] == '^' {
				if _, found := splitters[[2]int{newRow, newCol}]; found {
					continue
				} else {
					result++
					splitters[[2]int{newRow, newCol}] = true
				}
				positionsToTry.Enqueue([2]int{newRow, newCol + 1})
				positionsToTry.Enqueue([2]int{newRow, newCol - 1})
			} else {
				positionsToTry.Enqueue([2]int{newRow, newCol})
			}
		}
	}

	return result
}
