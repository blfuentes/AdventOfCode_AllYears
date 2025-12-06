package day06

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int64 {
	var result int64 = 0

	// var fileName string = "./day06/test_input_06.txt"
	var fileName string = "./day06/day06.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		worksheetIdx := -1
		worksheets := make([]Worksheet, 0)

		processWorkSheet := func(worksheetIdx int) {
			var tempResult int64
			switch worksheets[worksheetIdx].Operation {
			case '*':
				tempResult = 1
			case '+':
				tempResult = 0
			}
			for _, number := range worksheets[worksheetIdx].Numbers {
				switch worksheets[worksheetIdx].Operation {
				case '*':
					tempResult *= number
				case '+':
					tempResult += number
				}
			}
			result += tempResult
		}

		for wIdx, char := range fileContent[len(fileContent)-1] {
			if char == '*' || char == '+' {
				if worksheetIdx >= 0 {
					processWorkSheet(worksheetIdx)
				}
				worksheetIdx++
				worksheets = append(worksheets, Worksheet{Operation: char, Numbers: make([]int64, 0)})

				// process the numbers for this worksheet row-wise
				for _, row := range fileContent[:len(fileContent)-1] {
					currentVal := int64(0)
					for _, char := range row[wIdx:] {
						if char == ' ' {
							if currentVal > 0 {
								break
							}
						} else {
							currentVal = (currentVal * 10) + int64(char-'0')
						}
					}
					worksheets[worksheetIdx].Numbers = append(worksheets[worksheetIdx].Numbers, currentVal)
				}
			}
		}
		// process the last worksheet
		processWorkSheet(worksheetIdx)
	}

	return result
}
