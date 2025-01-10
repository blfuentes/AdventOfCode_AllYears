package day03

import (
	"AdventOfCode_2015_Go/utilities"
)

type HouseKey struct {
	row, col int
}

func Executepart1() int {
	var fileName string = "./day03/day03.txt"
	// var fileName string = "./day03/test_input_03.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		chars := []rune(fileContent)
		// fmt.Printf("%c\n", chars)
		result = runInstructions1(chars)
	}
	return result
}

func runInstructions1(steps []rune) int {
	movements := make(map[rune]([]int))
	movements['v'] = []int{1, 0}
	movements['^'] = []int{-1, 0}
	movements['>'] = []int{0, 1}
	movements['<'] = []int{0, -1}

	houses := make(map[HouseKey](int))
	initHouse := HouseKey{0, 0}
	houses[initHouse] = 1

	var newPos HouseKey
	for _, v := range steps {
		newPos = HouseKey{initHouse.row + movements[v][0], initHouse.col + movements[v][1]}
		if presents, ok := houses[newPos]; ok {
			houses[newPos] = presents + 1
		} else {
			houses[newPos] = 1
		}
		initHouse = newPos
	}

	return len(houses)
}
