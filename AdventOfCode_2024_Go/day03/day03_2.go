package day03

import (
	"regexp"
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart2() int {
	var result int = 0

	var fileName string = "./day03/day03.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		pattern := regexp.MustCompile(`mul\((\d+),(\d+)\)`)
		doparts := strings.Split(fileContent, "do()")
		for _, dopart := range doparts {
			removeddont := strings.Split(dopart, "don't()")[0]
			parts := pattern.FindAllStringSubmatch(removeddont, -1)
			for _, part := range parts {
				first, _ := strconv.Atoi(part[1])
				second, _ := strconv.Atoi(part[2])
				result += first * second
			}
		}
	}

	return result
}
