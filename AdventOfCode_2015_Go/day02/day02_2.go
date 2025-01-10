package day02

import (
	"AdventOfCode_2015_Go/utilities"
	"regexp"
	"sort"
	"strconv"
)

func Executepart2() int {
	var fileName string = "./day02/day02.txt"
	// var fileName string = "./day02/test_input_02.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			tmp := ExtractParts2(line)
			result += CalculatePresent2(tmp)
		}
	}

	return result
}

func ExtractParts2(input string) Present {
	r, _ := regexp.Compile(`\d+`)
	dim := make([]int64, 3)

	if parts := r.FindAllString(input, 3); parts != nil {
		dim[0], _ = strconv.ParseInt(parts[0], 10, 0)
		dim[1], _ = strconv.ParseInt(parts[1], 10, 0)
		dim[2], _ = strconv.ParseInt(parts[2], 10, 0)

		sort.Sort(utilities.Int64Array(dim))

		return Present{dim[0], dim[1], dim[2]}
	}

	return Present{}
}

func CalculatePresent2(present Present) int {
	values := make([]int64, 3)
	values[0], values[1], values[2] = present.long, present.width, present.height
	sort.Sort(utilities.Int64Array(values))

	return 2*int(values[0]) + 2*int(values[1]) +
		int(present.long*present.width*present.height)
}
