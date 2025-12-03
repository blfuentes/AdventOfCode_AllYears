package day03

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day03/test_input_03.txt"
	var fileName string = "./day03/day03.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			var first byte = '0'
			var second byte = '0'
			found := 0
			for idx := 0; idx < len(line)-1; idx++ {
				char := line[idx]
				if char > first {
					first = char
					found = idx
				}
			}
			for idx := found + 1; idx < len(line); idx++ {
				char := line[idx]
				if char > second {
					second = char
				}
			}
			// fmt.Printf("%c%c\n", first, second)
			result += int(first-'0')*10 + int(second-'0')
		}
	}

	return result
}
