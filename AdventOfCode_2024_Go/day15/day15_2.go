package day15

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type TileEx struct {
	Kind ExtendedDefineTile
	Row  int
	Col  int
}

type ExtendedDefineTile int

const (
	WallEx ExtendedDefineTile = iota
	EmptyEx
	RobotEx
	BoxLeft
	BoxRight
	NoneEx
)

func canMove(currentparents, parentsToCheck []TileEx, themap [][]TileEx, r, c int, trylevel bool) []TileEx {
	if !trylevel {
		return []TileEx{}
	}
	newparentsToCheck := make([]TileEx, 0)
	for _, t := range parentsToCheck {
		tmp := themap[t.Row+r][t.Col+c]
		if tmp.Kind == BoxRight {
			newparentsToCheck = append(newparentsToCheck, themap[tmp.Row][tmp.Col-1])
			newparentsToCheck = append(newparentsToCheck, tmp)
		} else if tmp.Kind == BoxLeft {
			newparentsToCheck = append(newparentsToCheck, tmp)
			newparentsToCheck = append(newparentsToCheck, themap[tmp.Row][tmp.Col+1])
		} else {
			newparentsToCheck = append(newparentsToCheck, tmp)
		}
	}
	allareempty := true
	notwallfound := true
	filteredparents := make([]TileEx, 0)
	for _, p := range newparentsToCheck {
		if p.Kind != EmptyEx {
			allareempty = false
			filteredparents = append(filteredparents, p)
		}
		if p.Kind == WallEx {
			notwallfound = false
		}
	}
	if allareempty {
		return append(append(currentparents, parentsToCheck...), newparentsToCheck...)
	}

	return canMove(append(currentparents, parentsToCheck...), filteredparents, themap, r, c, notwallfound)

}

func take(from []TileEx, themap [][]TileEx, mov DefinedDir) []TileEx {
	row, col := directionOfMov(mov)
	return canMove(make([]TileEx, 0), from, themap, row, col, true)
}

func findBoxes2(from TileEx, themap [][]TileEx, mov DefinedDir) []TileEx {
	result := make([]TileEx, 0)
	r, c := directionOfMov(mov)
	cantake := true
	emptyfound := false
	current := themap[from.Row][from.Col]
	for cantake {
		current = themap[current.Row+r][current.Col+c]
		switch current.Kind {
		case BoxRight:
			if mov == UP || mov == DOWN {
				partnerleft := themap[current.Row][current.Col-1]
				tosend := make([]TileEx, 0)
				tosend = append(tosend, partnerleft, current)
				availableboxes := take(tosend, themap, mov)
				result = append(result, availableboxes...)
				cantake = false
				emptyfound = len(availableboxes) > 0
			} else {
				if mov == LEFT || mov == RIGHT {
					result = append(result, current)
				}
			}
		case BoxLeft:
			if mov == UP || mov == DOWN {
				partnerright := themap[current.Row][current.Col+1]
				tosend := make([]TileEx, 0)
				tosend = append(tosend, current, partnerright)
				availableboxes := take(tosend, themap, mov)
				result = append(result, availableboxes...)
				cantake = false
				emptyfound = len(availableboxes) > 0
			} else {
				if mov == LEFT || mov == RIGHT {
					result = append(result, current)
				}
			}
		case EmptyEx:
			if mov == LEFT || mov == RIGHT {
				result = append(result, current)
				emptyfound = true
				cantake = false
			}
			result = append(result, current)
		case WallEx:
			cantake = false
		}
	}
	if emptyfound {
		utilities.ReverseArray(result)
		return result
	} else {
		return []TileEx{}
	}
}

func move2(robot TileEx, themap *[][]TileEx, movs []DefinedDir) {
	if len(movs) > 0 {
		mov := movs[0]
		r, c := directionOfMov(mov)
		nextPos := (*themap)[robot.Row+r][robot.Col+c]
		switch nextPos.Kind {
		case WallEx:
			move2(robot, themap, movs[1:])
		case EmptyEx:
			(*themap)[robot.Row][robot.Col].Kind = EmptyEx
			(*themap)[nextPos.Row][nextPos.Col].Kind = RobotEx
			robot.Row, robot.Col = nextPos.Row, nextPos.Col
			move2(robot, themap, movs[1:])
		default:
			tilesToMov := findBoxes2(robot, *themap, mov)
			if len(tilesToMov) > 0 {
				(*themap)[robot.Row][robot.Col].Kind = EmptyEx
				for _, t := range tilesToMov {
					possiblereplacement := (*themap)[t.Row-r][t.Col-c]
					isinpossible := false
					for _, tt := range tilesToMov {
						if tt.Row == possiblereplacement.Row &&
							tt.Col == possiblereplacement.Col {
							isinpossible = true
							break
						}
					}
					replacementkind := EmptyEx
					if isinpossible {
						replacementkind = possiblereplacement.Kind
					}

					if t.Kind != EmptyEx {
						(*themap)[t.Row+r][t.Col+c].Kind = t.Kind
					}
					(*themap)[t.Row][t.Col].Kind = replacementkind
				}
				(*themap)[nextPos.Row][nextPos.Col].Kind = RobotEx
				robot.Row, robot.Col = nextPos.Row, nextPos.Col
				move2(robot, themap, movs[1:])
			} else {
				move2(robot, themap, movs[1:])
			}
		}
	}
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day15/day15.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		robotinit, themap, movements := parseContent2(fileContent)

		move2(robotinit, &themap, movements)
		for _, v := range themap {
			for _, t := range v {
				result += calculateGPS2(t)
			}
		}
	}

	return result
}
