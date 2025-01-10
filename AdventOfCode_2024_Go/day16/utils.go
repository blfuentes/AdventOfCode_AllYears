package day16

import (
	"math"
	"strings"
)

type Position struct {
	Row int
	Col int
}

type PositionDir struct {
	Pos Position
	Dir int
}

type PositionDirDistance struct {
	PDir     PositionDir
	Distance int
}

type Cell struct {
	Visited  bool
	Distance int
	Paths    [][]Position
}

func parseContent(lines []string) ([][]string, Position, Position) {
	maxrows, maxcols := len(lines), len(lines[0])
	themap := make([][]string, 0)
	start, end := Position{-1, -1}, Position{-1, -1}

	for row := 0; row < maxrows; row++ {
		line := strings.Split(lines[row], "")
		mapline := make([]string, 0)
		for col := 0; col < maxcols; col++ {
			value := line[col]
			mapline = append(mapline, value)
			if value == "S" {
				start.Row, start.Col = row, col
			} else if value == "E" {
				end.Row, end.Col = row, col
			}
		}
		themap = append(themap, mapline)
	}

	return themap, start, end
}

func Neighbours(position Position, dir int) [3]PositionDirDistance {
	clockwisecell := PositionDirDistance{PositionDir{position, -1}, 1000}
	counterwisecell := PositionDirDistance{PositionDir{position, -1}, 1000}
	neigbour := PositionDirDistance{PositionDir{Position{-1, -1}, dir}, 1}
	switch dir {
	case 0: // east
		neigbour.PDir.Pos.Row = position.Row
		neigbour.PDir.Pos.Col = position.Col + 1
	case 1: // south
		neigbour.PDir.Pos.Row = position.Row + 1
		neigbour.PDir.Pos.Col = position.Col
	case 2: // west
		neigbour.PDir.Pos.Row = position.Row
		neigbour.PDir.Pos.Col = position.Col - 1
	case 3: // north
		neigbour.PDir.Pos.Row = position.Row - 1
		neigbour.PDir.Pos.Col = position.Col
	}
	clockwisecell.PDir.Dir = (dir + 1) % 4
	counterwisecell.PDir.Dir = (dir + 3) % 4

	return [3]PositionDirDistance{neigbour, clockwisecell, counterwisecell}
}

func buildGraph(themap [][]string) [][][]Cell {
	maxrows, maxcols := len(themap), len(themap[0])
	graph := make([][][]Cell, 0)
	for row := 0; row < maxrows; row++ {
		rowdimension := make([][]Cell, 0)
		for col := 0; col < maxcols; col++ {
			dirdimension := make([]Cell, 0)
			for dir := 0; dir < 4; dir++ {
				pos := Cell{Visited: false, Distance: math.MaxInt32}
				dirdimension = append(dirdimension, pos)
			}
			rowdimension = append(rowdimension, dirdimension)
		}
		graph = append(graph, rowdimension)
	}

	return graph
}

var isValidCoord = func(position Position, maxrows, maxcols int, themap [][]string) bool {
	return position.Row >= 0 && position.Col >= 0 &&
		position.Row < maxrows && position.Col < maxcols &&
		themap[position.Row][position.Col] != "#"
}
var alreadyVisited = func(pos Position, dir int, graph [][][]Cell) bool {
	return graph[pos.Row][pos.Col][dir].Visited
}
var setDistance = func(pos Position, dir, distance int, graph *[][][]Cell) {
	(*graph)[pos.Row][pos.Col][dir].Distance = distance
}
var setVisited = func(pos Position, dir int, graph *[][][]Cell) {
	(*graph)[pos.Row][pos.Col][dir].Visited = true
}
