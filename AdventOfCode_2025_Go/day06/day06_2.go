package day06

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day06/test_input_06.txt"
	// var fileName string = "./day06/day06.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
