package day07

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day07/test_input_07.txt"
	var fileName string = "./day07/day07.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		colStar := 0
		for cIdx, char := range fileContent[0] {
			if char == 'S' {
				colStar = cIdx
				break
			}
		}
		dp := make([]int64, len(fileContent[0])+2)
		for i := range dp {
			dp[i] = int64(1)
		}
		for rIdx := len(fileContent) - 1; rIdx >= 0; rIdx-- {
			for cIdx := 1; cIdx <= len(fileContent[0]); cIdx++ {
				if fileContent[rIdx][cIdx-1] == '^' {
					dp[cIdx] = dp[cIdx-1] + dp[cIdx+1]
				}
			}
		}
		result = dp[colStar+1]
	}

	return result
}
