package day03

import (
	"math"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day03/test_input_03.txt"
	var fileName string = "./day03/day03.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			values := make([]int64, 12)
			init := 0
			for c := 11; c >= 0; c-- {
				current := byte('0')
				maxIdx := len(line) - c
				for idx := init; idx < maxIdx; idx++ {
					char := line[idx]
					if char > current {
						current = char
						init = idx + 1
					}
				}
				values[11-c] = int64(current - '0')
			}
			for idx, v := range values {
				result += v * int64(math.Pow(10, float64(11-idx)))
			}
		}
	}

	return result
}
