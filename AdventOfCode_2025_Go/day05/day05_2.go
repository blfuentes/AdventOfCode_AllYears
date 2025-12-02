package day05

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day05/test_input_05.txt"
	// var fileName string = "./day05/day05.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
