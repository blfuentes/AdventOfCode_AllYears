package day02

import (
	"AdventOfCode_2015_Go/utilities"
	"regexp"
	"sort"
	"strconv"
)

type Present struct {
	long, width, height int64
}

func Executepart1() int {
	var fileName string = "./day02/day02.txt"
	// var fileName string = "./day02/test_input_02.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			tmp := ExtractParts(line)
			result += CalculatePresent(tmp)
		}
	}

	return result
}

func ExtractParts(input string) Present {
	r, _ := regexp.Compile(`\d+`)
	dim := make([]int64, 3)

	if parts := r.FindAllString(input, 3); parts != nil {
		dim[0], _ = strconv.ParseInt(parts[0], 10, 0)
		dim[1], _ = strconv.ParseInt(parts[1], 10, 0)
		dim[2], _ = strconv.ParseInt(parts[2], 10, 0)

		return Present{dim[0], dim[1], dim[2]}
	}

	return Present{}
}

func CalculatePresent(present Present) int {
	values := make([]int64, 3)
	values[0] = present.long
	values[1] = present.width
	values[2] = present.height
	sort.Sort(utilities.Int64Array(values))

	return int(values[0]) +
		2*int(present.long*present.width) +
		2*int(present.width*present.height) +
		2*int(present.long*present.height)
}
