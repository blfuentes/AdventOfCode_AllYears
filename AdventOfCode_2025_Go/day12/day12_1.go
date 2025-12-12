package day12

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

type Region struct {
	X, Y  int
	ToFit [6]int
}

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day12/test_input_12.txt"
	var fileName string = "./day12/day12.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		regionFits := func(region Region) bool {
			requiredArea := 0
			for _, v := range region.ToFit {
				requiredArea += v * 9
			}
			availableArea := region.X * region.Y
			return availableArea >= requiredArea
		}
		for _, line := range fileContent {
			if (len(line) > 4) && (line[2] == 'x') {
				dimensions, description, _ := strings.Cut(line, ": ")
				x := strings.Split(dimensions, "x")[0]
				y := strings.Split(dimensions, "x")[1]
				d := [6]int{0}
				for i, v := range strings.Split(description, " ") {
					d[i] = utilities.StringToInt(v)
				}
				region := Region{X: utilities.StringToInt(x), Y: utilities.StringToInt(y), ToFit: d}
				if regionFits(region) {
					result++
				}
			}
		}
	}

	return result
}
