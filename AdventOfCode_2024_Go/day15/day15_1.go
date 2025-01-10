package day15

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type DefineTile int

const (
	Wall DefineTile = iota
	Empty
	Robot
	Box
	None
)

type Tile struct {
	Kind DefineTile
	Row  int
	Col  int
}

func findBoxes(from Tile, themap [][]Tile, mov DefinedDir) []Tile {
	result := make([]Tile, 0)
	r, c := directionOfMov(mov)
	cantake := true
	emptyfound := false
	current := themap[from.Row][from.Col]
	for cantake {
		current = themap[current.Row+r][current.Col+c]
		switch current.Kind {
		case Empty:
			result = append(result, current)
			emptyfound = true
			cantake = false
		case Box:
			result = append(result, current)
		case Wall:
			cantake = false
		}
	}
	if emptyfound {
		utilities.ReverseArray(result)
		return result
	} else {
		return []Tile{}
	}
}

func move(robot Tile, themap *[][]Tile, movs []DefinedDir) {
	if len(movs) > 0 {
		mov := movs[0]
		r, c := directionOfMov(mov)
		nextPos := (*themap)[robot.Row+r][robot.Col+c]
		switch nextPos.Kind {
		case Wall:
			move(robot, themap, movs[1:])
		case Empty:
			(*themap)[robot.Row][robot.Col].Kind = Empty
			(*themap)[nextPos.Row][nextPos.Col].Kind = Robot
			robot.Row, robot.Col = nextPos.Row, nextPos.Col
			move(robot, themap, movs[1:])
		default:
			tilesToMov := findBoxes(robot, *themap, mov)
			if len(tilesToMov) > 0 {
				for tIdx := 0; tIdx < len(tilesToMov)-1; tIdx++ {
					t := tilesToMov[tIdx]
					(*themap)[t.Row][t.Col].Kind = Box
				}
				(*themap)[robot.Row][robot.Col].Kind = Empty
				(*themap)[nextPos.Row][nextPos.Col].Kind = Robot
				robot.Row, robot.Col = nextPos.Row, nextPos.Col
				move(robot, themap, movs[1:])
			} else {
				move(robot, themap, movs[1:])
			}
		}
	}
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day15/day15.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		robotinit, themap, movements := parseContent(fileContent)
		move(robotinit, &themap, movements)

		for _, v := range themap {
			for _, t := range v {
				result += calculateGPS(t)
			}
		}
	}

	return result
}
