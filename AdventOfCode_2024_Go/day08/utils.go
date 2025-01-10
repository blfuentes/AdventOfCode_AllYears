package day08

import "fmt"

type Coord struct {
	Row int
	Col int
}

type Antenna struct {
	Name     string
	Position Coord
}

func Mirror(coordA, coordB Coord, radius int) (cloneA, cloneB Coord) {
	dx := coordB.Row - coordA.Row
	dy := coordB.Col - coordA.Col

	cloneA = Coord{coordB.Row + radius*dx, coordB.Col + radius*dy}
	cloneB = Coord{coordA.Row - radius*dx, coordA.Col - radius*dy}

	return
}

func PrintMatrix[T any](antinodes *map[Coord]bool, maxRows, maxCols int) {
	for rowIdx := 0; rowIdx < maxRows; rowIdx++ {
		for colIdx := 0; colIdx < maxCols; colIdx++ {
			if _, ok := (*antinodes)[Coord{rowIdx, colIdx}]; ok {
				fmt.Printf("#")
			} else {
				fmt.Print(".")
			}
		}
		fmt.Println()
	}
}
