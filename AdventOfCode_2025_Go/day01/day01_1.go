package day01

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day01/test_input_01.txt"
	var fileName string = "./day01/day01.txt"
	var position int = 50
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			init := line[0]
			end := utilities.StringToInt(string(line[1:]))
			if init == 'L' {
				end *= -1
			}
			position = utilities.SafeModulo(position+end, 100)

			if position == 0 {
				result++
			}
		}
	}

	return result
}
