package day18

import (
	"fmt"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type KindDef int

const (
	Empty KindDef = iota
	Corrupted
	Visited
)

type Coord struct {
	X    int
	Y    int
	Kind KindDef
}

func parseContent(lines []string, size int) ([][]Coord, []Coord) {
	themap := make([][]Coord, 0)
	corrupted := make([]Coord, 0)
	for rowIdx := 0; rowIdx < size+1; rowIdx++ {
		mapline := make([]Coord, 0)
		for colIdx := 0; colIdx < size+1; colIdx++ {
			mapline = append(mapline, Coord{colIdx, rowIdx, Empty})
		}
		themap = append(themap, mapline)
	}
	for _, line := range lines {
		X := utilities.StringToInt(strings.Split(line, ",")[0])
		Y := utilities.StringToInt(strings.Split(line, ",")[1])
		corrupted = append(corrupted, Coord{X, Y, Corrupted})
	}
	return themap, corrupted
}

type CoordPath struct {
	TheCoord Coord
	PathSize int
}

func findShortestPath(graph [][]Coord, start, goal Coord) int {
	maxRows, maxCols := len(graph), len(graph[0])

	var isInBoundaries = func(row, col int) bool {
		return row >= 0 && col >= 0 && row < maxRows && col < maxCols
	}

	directions := [4][2]int{{-1, 0}, {1, 0}, {0, 1}, {0, -1}}

	queue := utilities.Queue[CoordPath]{}
	visited := make(map[Coord]bool)

	queue.Enqueue(CoordPath{start, 0})
	path := -1

	for !queue.IsEmpty() {
		current, _ := queue.Dequeue()
		if current.TheCoord == goal {
			path = current.PathSize
			break
		} else {
			if !visited[current.TheCoord] && current.TheCoord.Kind == Empty {
				visited[current.TheCoord] = true

				for _, dir := range directions {
					nextRow := current.TheCoord.Y + dir[0]
					nextCol := current.TheCoord.X + dir[1]

					if isInBoundaries(nextRow, nextCol) {
						neighbour := graph[nextRow][nextCol]
						if !visited[neighbour] {
							queue.Enqueue(CoordPath{neighbour, current.PathSize + 1})
						}
					}
				}
			}
		}
	}

	return path
}

func PrintMatrix(matrix *[][]Coord) {
	for rowIdx := 0; rowIdx < len(*matrix); rowIdx++ {
		for colIdx := 0; colIdx < len((*matrix)[rowIdx]); colIdx++ {
			kind := (*matrix)[rowIdx][colIdx].Kind
			switch kind {
			case Empty:
				fmt.Print(".")
			case Corrupted:
				fmt.Print("#")
			case Visited:
				fmt.Print("0")
			}
		}
		fmt.Println()
	}
}

func buildCorruptedMap(graph [][]Coord, corrupted []Coord, numOfBytes int) [][]Coord {
	clonedgraph := utilities.Clone2DArray(graph)
	for bIdx := 0; bIdx < numOfBytes; bIdx++ {
		c := corrupted[bIdx]
		clonedgraph[c.Y][c.X].Kind = Corrupted
	}
	return clonedgraph
}
