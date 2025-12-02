package day08

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day08/test_input_08.txt"
	// var fileName string = "./day08/day08.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
