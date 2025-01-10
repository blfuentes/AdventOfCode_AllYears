package day18

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day18/day18.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		size, bytes := 70, 1024
		themap, corrupted := parseContent(fileContent, size)

		start, goal := Coord{0, 0, Empty}, Coord{size, size, Empty}
		searchmap := buildCorruptedMap(themap, corrupted, bytes)
		result = findShortestPath(searchmap, start, goal)
	}

	return result
}
