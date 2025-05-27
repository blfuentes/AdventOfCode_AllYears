package day20

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day20/day20.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		result = len(fileContent)
	}

	return result
}
