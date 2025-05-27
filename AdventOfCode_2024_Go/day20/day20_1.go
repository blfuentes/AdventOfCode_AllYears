package day20

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day20/test_input_20.txt" //"./day20/day20.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {

		themap, _, _, _ := ParseContent(fileContent)
		PrintMatrix(&themap)
	}

	return result
}
