package day07

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day07/test_input_07.txt"
	// var fileName string = "./day07/day07.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
