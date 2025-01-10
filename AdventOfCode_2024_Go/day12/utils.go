package day12

import (
	"sort"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Coord struct {
	Row int
	Col int
}
type Region struct {
	Name   string
	Size   int
	Points []Coord
}

func copyMap(original map[Coord]struct{}) map[Coord]struct{} {
	copy := make(map[Coord]struct{}, len(original))
	for k, v := range original {
		copy[k] = v
	}
	return copy
}

func orderPointsByRowCol(points []Coord) []Coord {
	sort.Slice(points, func(row, col int) bool {
		if points[row].Row != points[col].Row {
			return points[row].Row < points[col].Row
		}
		return points[row].Col < points[col].Col
	})
	return points
}

func neighbours(point Coord) []Coord {
	return []Coord{
		{point.Row - 1, point.Col},
		{point.Row + 1, point.Col},
		{point.Row, point.Col - 1},
		{point.Row, point.Col + 1}}
}

func floodFillArea(point Coord, element string,
	currentArea *[]Coord, garden [][]string, visited *[][]bool,
	maxRows, maxCols int) []Coord {
	if !utilities.IsInBoundaries(point.Row, point.Col, maxRows, maxCols) {
		return *currentArea
	}
	if (*visited)[point.Row][point.Col] || garden[point.Row][point.Col] != element {
		return *currentArea
	}
	(*visited)[point.Row][point.Col] = true
	*currentArea = append(*currentArea, Coord{point.Row, point.Col})
	for _, n := range neighbours(point) {
		*currentArea = floodFillArea(n, element, currentArea, garden, visited, maxRows, maxCols)
	}
	return *currentArea
}

func buildRegions(garden [][]string, maxRows, maxCols int) []Region {
	visited := make([][]bool, maxRows)
	for idx := range visited {
		visited[idx] = make([]bool, maxCols)
	}
	regions := make([]Region, 0)

	for row := 0; row < maxRows; row++ {
		for col := 0; col < maxCols; col++ {
			if !visited[row][col] {
				currentArea := make([]Coord, 0)
				points := floodFillArea(Coord{row, col}, garden[row][col],
					&currentArea, garden, &visited, maxRows, maxCols)
				if len(points) > 0 {
					regions = append(regions, Region{garden[row][col], len(points), points})
				}
			}
		}
	}

	return regions
}
