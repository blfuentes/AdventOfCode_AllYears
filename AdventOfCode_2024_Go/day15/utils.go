package day15

import (
	"fmt"
	"strings"
)

type DefinedDir int

const (
	UP DefinedDir = iota
	DOWN
	RIGHT
	LEFT
	NONE
)

func symbolOf(tile Tile) string {
	switch tile.Kind {
	case Wall:
		return "#"
	case Empty:
		return " "
	case Robot:
		return "@"
	case Box:
		return "O"
	}
	return ""
}

func symbolOf2(tile TileEx) string {
	switch tile.Kind {
	case WallEx:
		return "#"
	case EmptyEx:
		return " "
	case RobotEx:
		return "@"
	case BoxLeft:
		return "["
	case BoxRight:
		return "]"
	}
	return ""
}

func directionOfMov(mov DefinedDir) (int, int) {
	switch mov {
	case UP:
		return -1, 0
	case DOWN:
		return 1, 0
	case RIGHT:
		return 0, 1
	case LEFT:
		return 0, -1
	}
	return 0, 0
}

func calculateGPS(tile Tile) int {
	if tile.Kind == Box {
		return 100*tile.Row + tile.Col
	}

	return 0
}

func calculateGPS2(tile TileEx) int {
	if tile.Kind == BoxLeft {
		return 100*tile.Row + tile.Col
	}

	return 0
}

func printMap(matrix *[][]Tile) {
	for rowIdx := 0; rowIdx < len(*matrix); rowIdx++ {
		for colIdx := 0; colIdx < len((*matrix)[rowIdx]); colIdx++ {
			fmt.Printf("%v", symbolOf((*matrix)[rowIdx][colIdx]))
		}
		fmt.Println()
	}
}

func printMap2(matrix *[][]TileEx) {
	for rowIdx := 0; rowIdx < len(*matrix); rowIdx++ {
		for colIdx := 0; colIdx < len((*matrix)[rowIdx]); colIdx++ {
			fmt.Printf("%v", symbolOf2((*matrix)[rowIdx][colIdx]))
		}
		fmt.Println()
	}
}

func parseContent(lines string) (Tile, [][]Tile, []DefinedDir) {
	themap := make([][]Tile, 0)
	var initposition Tile = Tile{}
	movements := make([]DefinedDir, 0)

	mappart, movpart := strings.Split(lines, "\r\n\r\n")[0], strings.Split(lines, "\r\n\r\n")[1]
	for row, line := range strings.Split(mappart, "\r\n") {
		mapline := make([]Tile, 0)
		for col, char := range strings.Split(line, "") {
			tile := Tile{Empty, row, col}
			if char == "#" {
				tile.Kind = Wall
			} else if char == "." {
				tile.Kind = Empty
			} else if char == "@" {
				tile.Kind = Robot
				initposition = tile
			} else {
				tile.Kind = Box
			}

			mapline = append(mapline, tile)
		}
		themap = append(themap, mapline)
	}

	movpart = strings.ReplaceAll(movpart, "\r\n", "")
	for _, m := range strings.Split(movpart, "") {
		if m == "^" {
			movements = append(movements, UP)
		} else if m == "v" {
			movements = append(movements, DOWN)
		} else if m == ">" {
			movements = append(movements, RIGHT)
		} else {
			movements = append(movements, LEFT)
		}
	}

	return initposition, themap, movements
}

func parseContent2(lines string) (TileEx, [][]TileEx, []DefinedDir) {
	themap := make([][]TileEx, 0)
	var initposition TileEx = TileEx{}
	movements := make([]DefinedDir, 0)

	mappart, movpart := strings.Split(lines, "\r\n\r\n")[0], strings.Split(lines, "\r\n\r\n")[1]
	for row, line := range strings.Split(mappart, "\r\n") {
		mapline := make([]TileEx, 0)
		index := 0
		for _, char := range strings.Split(line, "") {
			if char == "#" {
				mapline = append(mapline, TileEx{WallEx, row, index})
				mapline = append(mapline, TileEx{WallEx, row, index + 1})
			} else if char == "." {
				mapline = append(mapline, TileEx{EmptyEx, row, index})
				mapline = append(mapline, TileEx{EmptyEx, row, index + 1})
			} else if char == "@" {
				mapline = append(mapline, TileEx{RobotEx, row, index})
				mapline = append(mapline, TileEx{EmptyEx, row, index + 1})
				initposition = TileEx{RobotEx, row, index}
			} else {
				mapline = append(mapline, TileEx{BoxLeft, row, index})
				mapline = append(mapline, TileEx{BoxRight, row, index + 1})
			}
			index += 2
		}
		themap = append(themap, mapline)
	}

	movpart = strings.ReplaceAll(movpart, "\r\n", "")
	for _, m := range strings.Split(movpart, "") {
		if m == "^" {
			movements = append(movements, UP)
		} else if m == "v" {
			movements = append(movements, DOWN)
		} else if m == ">" {
			movements = append(movements, RIGHT)
		} else {
			movements = append(movements, LEFT)
		}
	}

	return initposition, themap, movements
}
