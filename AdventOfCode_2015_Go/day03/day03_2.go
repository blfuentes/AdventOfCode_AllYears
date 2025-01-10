package day03

import (
	"AdventOfCode_2015_Go/utilities"
)

func Executepart2() int {
	var fileName string = "./day03/day03.txt"
	// var fileName string = "./day03/test_input_03.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		chars := []rune(fileContent)
		// fmt.Printf("%c\n", chars)
		result = runInstructions2(chars)
	}
	return result
}

func runInstructions2(steps []rune) int {
	movements := make(map[rune]([]int))
	movements['v'] = []int{1, 0}
	movements['^'] = []int{-1, 0}
	movements['>'] = []int{0, 1}
	movements['<'] = []int{0, -1}

	houses := make(map[HouseKey](int))
	santaPos := HouseKey{0, 0}
	roboSantaPost := HouseKey{0, 0}
	houses[santaPos] = 1
	houses[roboSantaPost] = houses[roboSantaPost] + 1

	var newSantaPos HouseKey
	var newRoboSantaPost HouseKey
	for idx := 0; idx < len(steps)-2; idx = idx + 2 {
		v := steps[idx]
		v2 := steps[idx+1]
		newSantaPos = HouseKey{santaPos.row + movements[v][0], santaPos.col + movements[v][1]}
		newRoboSantaPost = HouseKey{roboSantaPost.row + movements[v2][0], roboSantaPost.col + movements[v2][1]}
		if presents, ok := houses[newSantaPos]; ok {
			houses[newSantaPos] = presents + 1
		} else {
			houses[newSantaPos] = 1
		}
		if presents, ok := houses[newRoboSantaPost]; ok {
			houses[newRoboSantaPost] = presents + 1
		} else {
			houses[newRoboSantaPost] = 1
		}
		santaPos = newSantaPos
		roboSantaPost = newRoboSantaPost
	}

	return len(houses)
}
