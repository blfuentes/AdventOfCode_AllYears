package day09

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day09/test_input_09.txt"
	// var fileName string = "./day09/day09.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
