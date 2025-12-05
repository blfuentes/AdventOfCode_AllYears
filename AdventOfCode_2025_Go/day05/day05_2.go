package day05

import (
	"sort"
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day05/test_input_05.txt"
	var fileName string = "./day05/day05.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		var ranges []Range = make([]Range, 0)
		for _, line := range fileContent {
			if line == "" {
				break
			}
			ranges = append(ranges,
				Range{
					start: utilities.StringToInt64(strings.Split(line, "-")[0]),
					end:   utilities.StringToInt64(strings.Split(line, "-")[1])})
		}

		sort.Slice(ranges, func(i, j int) bool {
			if ranges[i].start != ranges[j].start {
				return ranges[i].start < ranges[j].start
			}
			return ranges[i].end < ranges[j].end
		})
		var mergedRanges []Range = make([]Range, 0)
		for _, r := range ranges {
			if len(mergedRanges) == 0 || mergedRanges[len(mergedRanges)-1].end < r.start {
				mergedRanges = append(mergedRanges, r)
			} else {
				if mergedRanges[len(mergedRanges)-1].end < r.end {
					mergedRanges[len(mergedRanges)-1].end = r.end
				}
			}
		}

		for _, r := range mergedRanges {
			result += (r.end - r.start) + 1
		}
	}
	return result
}
