package day06

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type PatrolResult struct {
	IsLoop  bool
	Visited []Coord
}

func patrol2(guardmap [][]string, patrolpos Coord, extraObstable Coord) (visited PatrolResult) {
	direction := Direction{Coord{-1, 0}, UP}
	maxRows := len(guardmap)
	maxCols := len(guardmap[0])

	guardmap[patrolpos.Row][patrolpos.Col] = "X"
	outOfRange := false
	visited.Visited = append(visited.Visited, Coord{patrolpos.Row, patrolpos.Col})

	seen := make(map[Direction]bool)
	seen[Direction{Coord{patrolpos.Row, patrolpos.Col}, UP}] = true

	for !outOfRange && !visited.IsLoop {
		patrolpos.Row += direction.Mov.Row
		patrolpos.Col += direction.Mov.Col

		if utilities.IsInBoundaries(patrolpos.Row, patrolpos.Col, maxRows, maxCols) {
			value := guardmap[patrolpos.Row][patrolpos.Col]
			if value == "#" || (patrolpos.Row == extraObstable.Row && patrolpos.Col == extraObstable.Col) {
				patrolpos.Row -= direction.Mov.Row
				patrolpos.Col -= direction.Mov.Col
				direction = TurnRight(direction)
			} else {
				if value != "X" {
					visited.Visited = append(visited.Visited, Coord{patrolpos.Row, patrolpos.Col})
					guardmap[patrolpos.Row][patrolpos.Col] = "X"
				}
				if _, exists := seen[Direction{Coord{patrolpos.Row, patrolpos.Col}, direction.Orientation}]; exists {
					visited.IsLoop = true
				} else {
					seen[Direction{Coord{patrolpos.Row, patrolpos.Col}, direction.Orientation}] = true
				}
			}
		} else {
			outOfRange = true
			visited.IsLoop = false
		}
	}

	return
}

func Executepart2() int {
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

		firstround := patrol2(guardmap, guardposition, Coord{-1, -1})

		for _, round := range firstround.Visited {
			patrolround := patrol2(guardmap, guardposition, round)
			if patrolround.IsLoop {
				result++
			}
		}
	}

	return result
}
