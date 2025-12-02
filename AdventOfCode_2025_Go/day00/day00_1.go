package day00

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day00/day00.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
