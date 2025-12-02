package day01

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func ZeroCrossings(start, end int) int {
	if start <= end {
		return utilities.FloorDiv(end, 100) - utilities.FloorDiv(start, 100)
	}
	return ZeroCrossings(end-1, start-1)
}

func Executepart2() int {
	var result int = 0

	// var fileName string = "./day01/test_input_01.txt"
	var fileName string = "./day01/day01.txt"
	var sum int = 50
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			init := line[0]
			end := utilities.StringToInt(string(line[1:]))
			if init == 'L' {
				end *= -1
			}
			result += ZeroCrossings(sum, (sum + end))
			sum += end
		}
	}

	return result
}
