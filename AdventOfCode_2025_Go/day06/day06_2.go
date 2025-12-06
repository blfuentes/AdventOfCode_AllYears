package day06

import (
	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int64 {
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
			if char == '*' || char == '+' || char == '\n' {
				if worksheetIdx >= 0 {
					processWorkSheet(worksheetIdx)
				}
				worksheetIdx++
				worksheets = append(worksheets, Worksheet{Operation: char, Numbers: make([]int64, 0)})

				// process the numbers for this worksheet column-wise
				foundValue := true
				for cIdx := wIdx; foundValue && cIdx < len(fileContent[len(fileContent)-1]); cIdx++ {
					currentVal := int64(0)
					foundValue = false
					for _, row := range fileContent[:len(fileContent)-1] {
						tmpChar := rune(row[cIdx])
						if tmpChar == ' ' {
							if currentVal > 0 {
								foundValue = true
								break
							}
						} else {
							foundValue = true
							currentVal = (currentVal * 10) + int64(tmpChar-'0')
						}
					}
					if foundValue || currentVal > 0 {
						worksheets[worksheetIdx].Numbers = append(worksheets[worksheetIdx].Numbers, currentVal)
					} else {
						break
					}
				}
			}
		}
		// process the last worksheet
		processWorkSheet(worksheetIdx)
	}

	return result
}
