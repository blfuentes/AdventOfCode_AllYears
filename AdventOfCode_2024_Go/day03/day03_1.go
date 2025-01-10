package day03

import (
	"regexp"
	"strconv"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day03/day03.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		pattern := regexp.MustCompile(`mul\((\d+),(\d+)\)`)
		parts := pattern.FindAllStringSubmatch(fileContent, -1)
		for _, part := range parts {
			first, _ := strconv.Atoi(part[1])
			second, _ := strconv.Atoi(part[2])
			result += first * second
		}
	}

	return result
}
