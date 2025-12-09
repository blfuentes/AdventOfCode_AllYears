package day09

import (
	"math"
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func rectangleArea(x1, y1, x2, y2 int64) int64 {
	return int64((math.Abs(float64(x2-x1)) + 1) * (math.Abs(float64(y2-y1)) + 1))
}

func Executepart1() int64 {
	var result int64 = 0

	// var fileName string = "./day09/test_input_09.txt"
	var fileName string = "./day09/day09.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {

		for fromIdx := 0; fromIdx < len(fileContent)-1; fromIdx++ {
			for toIdx := fromIdx + 1; toIdx < len(fileContent); toIdx++ {
				fX, fY, _ := strings.Cut(fileContent[fromIdx], ",")
				tX, tY, _ := strings.Cut(fileContent[toIdx], ",")
				area := rectangleArea(
					utilities.StringToInt64(fX),
					utilities.StringToInt64(fY),
					utilities.StringToInt64(tX),
					utilities.StringToInt64(tY))
				if area > result {
					result = area
				}
			}
		}
	}

	return result
}
