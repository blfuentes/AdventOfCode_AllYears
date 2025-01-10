package day12

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func calculatePerimeter(points []Coord, maxRows, maxCols int) (perimeter int) {
	processed := make(map[Coord]struct{})
	for _, p := range points {
		if _, ok := processed[p]; !ok {
			processed[p] = struct{}{}
		}
	}

	for _, point := range points {
		for _, n := range neighbours(point) {
			if !utilities.IsInBoundaries(n.Row, n.Col, maxRows, maxCols) {
				perimeter++
			} else if _, ok := processed[n]; !ok {
				perimeter++
			}
		}
	}

	return
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day12/day12.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		garden := make([][]string, 0)
		for _, line := range fileContent {
			garden = append(garden, strings.Split(line, ""))
		}
		maxRows, maxCols := len(garden), len(garden[0])
		regions := buildRegions(garden, maxRows, maxCols)
		for _, reg := range regions {
			result += reg.Size * calculatePerimeter(reg.Points, maxRows, maxCols)
		}
	}

	return result
}
