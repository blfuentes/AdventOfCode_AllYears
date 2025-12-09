package day09

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

type Edge struct {
	x1, y1, x2, y2 int64
}

func sort(a, b int64) (int64, int64) {
	if a < b {
		return a, b
	} else {
		return b, a
	}
}

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day09/test_input_09.txt"
	var fileName string = "./day09/day09.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {

		redTiles := make([][]int64, 0)
		edges := make([]Edge, 0)

		var initX, initY = utilities.StringToInt64(strings.Split(fileContent[0], ",")[0]),
			utilities.StringToInt64(strings.Split(fileContent[0], ",")[1])
		var lastX, lastY = utilities.StringToInt64(strings.Split(fileContent[len(fileContent)-1], ",")[0]),
			utilities.StringToInt64(strings.Split(fileContent[len(fileContent)-1], ",")[1])

		for fromIdx := 0; fromIdx < len(fileContent)-1; fromIdx++ {
			fX := utilities.StringToInt64(strings.Split(fileContent[fromIdx], ",")[0])
			fY := utilities.StringToInt64(strings.Split(fileContent[fromIdx], ",")[1])
			tX := utilities.StringToInt64(strings.Split(fileContent[fromIdx+1], ",")[0])
			tY := utilities.StringToInt64(strings.Split(fileContent[fromIdx+1], ",")[1])

			edges = append(edges, Edge{fX, fY, tX, tY})
			redTiles = append(redTiles, []int64{fX, fY}, []int64{tX, tY})
		}

		// close the polygon
		edges = append(edges, Edge{initX, initY, lastX, lastY})

		// does it intersect?
		var intersections = func(minX, minY, maxX, maxY int64) bool {
			for _, inter := range edges {
				iMinX, iMaxX := sort(inter.x1, inter.x2)
				iMinY, iMaxY := sort(inter.y1, inter.y2)
				if minX < iMaxX && maxX > iMinX && minY < iMaxY && maxY > iMinY {
					return true
				}
			}
			return false
		}

		for fTIdx := 0; fTIdx < len(redTiles)-1; fTIdx++ {
			for tTIdx := fTIdx; tTIdx < len(redTiles); tTIdx++ {
				fromTile := redTiles[fTIdx]
				toTile := redTiles[tTIdx]
				minX, maxX := sort(fromTile[0], toTile[0])
				minY, maxY := sort(fromTile[1], toTile[1])
				if !intersections(minX, minY, maxX, maxY) {
					area := rectangleArea(fromTile[0], fromTile[1], toTile[0], toTile[1])
					if area > result {
						result = area
					}
				}
			}
		}
	}

	return result
}
