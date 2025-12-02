package day02

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day02/test_input_02.txt"
	// var fileName string = "./day02/day02.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
