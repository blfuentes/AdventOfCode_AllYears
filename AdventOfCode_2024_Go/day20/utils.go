package day20

import (
	"fmt"
	"strings"
)

type KindDef int

const (
	Empty KindDef = iota
	Wall
	Cheat
	Start
	End
)

type Coord struct {
	Row  int
	Col  int
	Kind KindDef
}

func ParseContent(lines []string) ([][]Coord, []Coord, Coord, Coord) {
	maxrows, maxcols := len(lines), len(lines[0])
	themap := make([][]Coord, 0)
	wallpoints := make([]Coord, 0)
	start, end := Coord{-1, -1, Start}, Coord{-1, -1, End}

	for row := range maxrows {
		line := strings.Split(lines[row], "")
		mapline := make([]Coord, 0)
		for col := range maxcols {
			value := line[col]
			tmpCoord := Coord{row, col, Empty}
			if value == "S" {
				start.Row, start.Col = row, col
				tmpCoord.Kind = Start
			} else if value == "E" {
				end.Row, end.Col = row, col
				tmpCoord.Kind = End
			} else if value == "#" {
				wallpoints = append(wallpoints, Coord{row, col, Wall})
				tmpCoord.Kind = Wall
			}
			mapline = append(mapline, tmpCoord)
		}
		themap = append(themap, mapline)
	}

	return themap, wallpoints, start, end

}

func PrintMatrix(matrix *[][]Coord) {
	for rowIdx := range *matrix {
		for colIdx := range (*matrix)[rowIdx] {
			kind := (*matrix)[rowIdx][colIdx].Kind
			switch kind {
			case Empty:
				fmt.Print(".")
			case Wall:
				fmt.Print("#")
			case Start:
				fmt.Print("S")
			case End:
				fmt.Print("E")
			}
		}
		fmt.Println()
	}
}

func Neighbours(position Coord) [][]int {
	directions := [][]int{
		{-1, 0},
		{1, 0},
		{0, 1},
		{0, -1},
	}
	neighbours := make([][]int, 0)
	for _, dir := range directions {
		neighbours = append(neighbours, []int{dir[0], dir[1]})
	}

	return neighbours
}

func IsInBoundaries(row, col, maxRows, maxCols int) bool {
	return row >= 0 && col >= 0 && row < maxRows && col < maxCols
}
