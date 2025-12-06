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
			tempResult := int64(0)
			if operation == '*' {
				tempResult = 1
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

		var currentNumbers []int64
		var currentOp rune
		for wIdx, char := range fileContent[len(fileContent)-1] {
			if char == '*' || char == '+' {
				if currentNumbers != nil {
					processWorkSheet(currentOp, currentNumbers)
				}
				currentOp = char
				currentNumbers = make([]int64, 0)

				// process the numbers for this worksheet column-wise
				for cIdx := wIdx; cIdx < len(fileContent[len(fileContent)-1]); cIdx++ {
					currentVal := int64(0)
					hasDigit := false
					for _, row := range fileContent[:len(fileContent)-1] {
						tmpChar := rune(row[cIdx])
						if tmpChar != ' ' {
							hasDigit = true
							currentVal = (currentVal * 10) + int64(tmpChar-'0')
						} else if hasDigit {
							break
						}
					}
					if !hasDigit {
						break
					}
					currentNumbers = append(currentNumbers, currentVal)
				}
			}
		}
		// process the last worksheet
		if currentNumbers != nil {
			processWorkSheet(currentOp, currentNumbers)
		}
	}

	return result
}
