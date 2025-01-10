package day19

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func countIfValid(design string, patterns *map[string]bool, memo *map[string]int64, maxSize int, countSoFar int64) int64 {
	initialdesign := design
	if value, found := (*memo)[initialdesign]; found {
		return value
	}
	if initialdesign == "" {
		(*memo)[initialdesign] = countSoFar + 1
		return countSoFar + 1
	}

	minlength := min(maxSize, len(initialdesign))

	var subcount int64 = 0
	for idx := 1; idx <= minlength; idx++ {
		subdesign := initialdesign[:idx]
		if (*patterns)[subdesign] {
			newsubdesign := initialdesign[idx:]
			subcount += countIfValid(newsubdesign, patterns, memo, maxSize, countSoFar)
		}
	}

	(*memo)[initialdesign] = subcount
	return subcount
}

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day19/day19.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		patterns, designs, maxSize := parseContent(fileContent)

		memo := make(map[string]int64, 0)
		for _, design := range designs {
			result += countIfValid(design, &patterns, &memo, maxSize, 0)
		}
	}

	return result
}
