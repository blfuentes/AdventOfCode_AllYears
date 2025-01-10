package day06

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func patrol1(guardmap [][]string, patrolpos Coord) (visited int) {
	direction := Direction{Coord{-1, 0}, UP}
	maxRows := len(guardmap)
	maxCols := len(guardmap[0])

	guardmap[patrolpos.Row][patrolpos.Col] = "X"
	outOfRange := false
	visited = 1
	for !outOfRange {
		patrolpos.Row += direction.Mov.Row
		patrolpos.Col += direction.Mov.Col

		if utilities.IsInBoundaries(patrolpos.Row, patrolpos.Col, maxRows, maxCols) {
			value := guardmap[patrolpos.Row][patrolpos.Col]
			if value == "#" {
				patrolpos.Row -= direction.Mov.Row
				patrolpos.Col -= direction.Mov.Col
				direction = TurnRight(direction)
			} else {
				if value != "X" {
					visited++
					guardmap[patrolpos.Row][patrolpos.Col] = "X"
				}
			}
		} else {
			outOfRange = true
		}
	}

	return
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day06/day06.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		guardmap := make([][]string, len(fileContent))
		var guardposition Coord
		obstacles := make(map[Coord]bool)
		for lineIdx := 0; lineIdx < len(fileContent); lineIdx++ {
			guardmap[lineIdx] = make([]string, len(fileContent[lineIdx]))
			for cIdx, val := range strings.Split(fileContent[lineIdx], "") {
				if val == "^" {
					guardposition = Coord{lineIdx, cIdx}
				}
				if val == "#" {
					obstacles[Coord{lineIdx, cIdx}] = true
				}
				guardmap[lineIdx][cIdx] = val
			}
		}

		result = patrol1(guardmap, guardposition)
	}

	return result
}
