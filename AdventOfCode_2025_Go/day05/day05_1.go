package day05

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day05/test_input_05.txt"
	var fileName string = "./day05/day05.txt"
	var rangesSection = true
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		var ranges []Range = make([]Range, 0)
		var containsSection = func(number int64) bool {
			for _, r := range ranges {
				if number >= r.start && number <= r.end {
					return true
				}
			}
			return false
		}
		for _, line := range fileContent {
			if line == "" {
				rangesSection = false
				continue
			}
			if rangesSection {
				ranges = append(ranges,
					Range{
						start: utilities.StringToInt64(strings.Split(line, "-")[0]),
						end:   utilities.StringToInt64(strings.Split(line, "-")[1])})
			} else {
				if number := utilities.StringToInt64(line); containsSection(number) {
					result++
				}
			}
		}
	}
	return result
}
