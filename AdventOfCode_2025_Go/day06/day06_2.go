package day06

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day06/test_input_06.txt"
	var fileName string = "./day06/day06.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		processWorkSheet := func(operation rune, numbers []int64) {
			tempResult := int64(1)
			if operation == '+' {
				tempResult = 0
			}
			for _, number := range numbers {
				if operation == '*' {
					tempResult *= number
				} else {
					tempResult += number
				}
			}
			result += tempResult
		}

		currentNumbers := make([]int64, 0)
		for cIdx := len(fileContent[0]) - 1; cIdx >= 0; cIdx-- {
			currentVal := int64(0)
			hasDigit := false
			for _, row := range fileContent {
				tmpChar := rune(row[cIdx])
				if tmpChar == '+' || tmpChar == '*' {
					currentNumbers = append(currentNumbers, currentVal)
					processWorkSheet(tmpChar, currentNumbers)
					currentNumbers = make([]int64, 0)
					hasDigit = false
					break
				}
				if tmpChar != ' ' {
					currentVal = currentVal*10 + int64(tmpChar-'0')
					hasDigit = true
				}
			}
			if hasDigit {
				currentNumbers = append(currentNumbers, currentVal)
			}
		}
	}

	return result
}
