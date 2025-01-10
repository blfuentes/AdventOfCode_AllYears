package day01

import (
	"AdventOfCode_2015_Go/utilities"
)

func Executepart2() int {
	var fileName string = "./day01/day01.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		chars := []rune(fileContent)
		for i := 0; i < len(chars); i++ {
			if string(chars[i]) == "(" {
				result = result + 1
			} else {
				result = result - 1
			}
			if result == -1 {
				result = i + 1
				break
			}
		}
	}

	return result
}
