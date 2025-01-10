package day19

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func checkIfValid(design string, patterns *map[string]bool, memo *map[string]bool, maxSize int) bool {
	initialdesign := design
	if value, found := (*memo)[initialdesign]; found {
		return value
	}
	if initialdesign == "" {
		(*memo)[initialdesign] = true
		return true
	}

	minlength := min(maxSize, len(initialdesign))

	for idx := 1; idx <= minlength; idx++ {
		subdesign := initialdesign[:idx]
		if (*patterns)[subdesign] {
			newsubdesign := initialdesign[idx:]
			if checkIfValid(newsubdesign, patterns, memo, maxSize) {
				(*memo)[initialdesign] = true
				return true
			}
		}
	}

	(*memo)[initialdesign] = false
	return false
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day19/day19.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		patterns, designs, maxSize := parseContent(fileContent)

		memo := make(map[string]bool, 0)
		for _, design := range designs {
			if checkIfValid(design, &patterns, &memo, maxSize) {
				result++
			}
		}
	}

	return result
}
