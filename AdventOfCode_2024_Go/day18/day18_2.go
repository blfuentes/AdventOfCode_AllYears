package day18

import (
	"fmt"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func findCorrupted(themap [][]Coord, corrupted []Coord, start, goal Coord) Coord {
	result := Coord{-1, -1, Empty}
	cIdx := 0
	for true {
		corrupted := corrupted[cIdx]
		themap[corrupted.Y][corrupted.X].Kind = Corrupted
		pathresult := findShortestPath(themap, start, goal)
		if pathresult < 0 {
			return corrupted
		}
		cIdx++
	}
	return result
}

func Executepart2() string {
	var result string = ""

	var fileName string = "./day18/day18.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		size := 70
		themap, corrupted := parseContent(fileContent, size)

		start, goal := Coord{0, 0, Empty}, Coord{size, size, Empty}
		found := findCorrupted(themap, corrupted, start, goal)
		result = fmt.Sprintf("%v,%v", found.X, found.Y)
	}

	return result
}
