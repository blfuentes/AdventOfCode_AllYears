package day02

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day02/day02.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		reports := make([][]int, len(fileContent))
		for lineIdx := 0; lineIdx < len(fileContent); lineIdx++ {
			parts := strings.Split(fileContent[lineIdx], " ")
			report := make([]int, len(parts))
			for cc := 0; cc < len(parts); cc++ {
				report[cc], _ = strconv.Atoi(parts[cc])
			}
			reports[lineIdx] = report
		}
		for idx := 0; idx < len(reports); idx++ {
			if IsSafeReport(reports[idx]) {
				result++
			}
		}
	}

	return result
}
