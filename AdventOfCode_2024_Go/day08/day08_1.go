package day08

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func FindAntinodes1(antennas []Antenna, maxRows, maxCols int, antinodes *map[Coord]bool) {
	pairOfAntennas := utilities.Combinations(antennas, 2)
	for _, pair := range pairOfAntennas {
		mirrorA, mirrorB := Mirror(pair[0].Position, pair[1].Position, 1)
		if utilities.IsInBoundaries(mirrorA.Row, mirrorA.Col, maxRows, maxCols) {
			(*antinodes)[mirrorA] = true
		}
		if utilities.IsInBoundaries(mirrorB.Row, mirrorB.Col, maxRows, maxCols) {
			(*antinodes)[mirrorB] = true
		}
	}
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day08/day08.txt"
	// var fileName string = "./day08/test_input_08.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		antennas := make(map[string][]Antenna)
		maxRows := len(fileContent)
		maxCols := len(fileContent[0])
		for row := 0; row < maxRows; row++ {
			for col := 0; col < maxCols; col++ {
				value := strings.Split(fileContent[row], "")[col]
				if value != "." {
					antennas[value] = append(antennas[value], Antenna{value, Coord{row, col}})
				}
			}
		}
		antinodes := make(map[Coord]bool)
		for _, subantennas := range antennas {
			FindAntinodes1(subantennas, maxRows, maxCols, &antinodes)
		}
		result += len(antinodes)
	}

	return result
}
