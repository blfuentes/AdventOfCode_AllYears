package day01

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func countTimes(leftside []string, rightside []string) (result int) {
	rightsidetarget := strings.Join(rightside, ",")
	var times int = 0
	for idx := 0; idx < len(leftside); idx++ {
		value := leftside[idx]
		times = strings.Count(rightsidetarget, value)

		if val, err := strconv.Atoi(leftside[idx]); err == nil && times > 0 {
			result += val * times
		}
	}
	return
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day01/day01.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		left := make([]string, len(fileContent))
		right := make([]string, len(fileContent))
		for lineIdx := 0; lineIdx < len(fileContent); lineIdx++ {
			parts := strings.Split(fileContent[lineIdx], " ")
			left[lineIdx] = parts[0]
			right[lineIdx] = parts[(len(parts) - 1)]
		}
		result = countTimes(left, right)
	}

	return result
}
