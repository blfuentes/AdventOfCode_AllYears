package day01

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day01/test_input_01.txt"
	// var fileName string = "./day01/day01.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
