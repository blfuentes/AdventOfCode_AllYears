package day08

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func FindAntinodes2(antennas []Antenna, maxRows, maxCols int, antinodes *map[Coord]bool) {
	pairOfAntennas := utilities.Combinations(antennas, 2)
	for _, pair := range pairOfAntennas {
		radius := 1
		oneOutOfRange := false
		twoOutOfRange := false
		initialA, initialB := pair[0].Position, pair[1].Position
		(*antinodes)[initialA] = true
		(*antinodes)[initialB] = true
		for !oneOutOfRange || !twoOutOfRange {
			mirrorA, mirrorB := Mirror(initialA, initialB, radius)
			if utilities.IsInBoundaries(mirrorA.Row, mirrorA.Col, maxRows, maxCols) {
				(*antinodes)[mirrorA] = true
			} else {
				oneOutOfRange = true
			}
			if utilities.IsInBoundaries(mirrorB.Row, mirrorB.Col, maxRows, maxCols) {
				(*antinodes)[mirrorB] = true
			} else {
				twoOutOfRange = true
			}
			radius++
		}
	}
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day08/day08.txt"

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
			FindAntinodes2(subantennas, maxRows, maxCols, &antinodes)
		}

		result += len(antinodes)
	}

	return result
}
