package day03

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day03/test_input_03.txt"
	// var fileName string = "./day03/day03.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
