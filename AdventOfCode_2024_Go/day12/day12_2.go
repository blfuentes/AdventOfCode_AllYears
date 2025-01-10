package day12

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Direction int

const (
	Up    Direction = 1
	Down  Direction = 2
	Right Direction = 3
	Left  Direction = 4
)

func nextPosition(movDir Direction, position Coord) Coord {
	switch movDir {
	case Up:
		return Coord{position.Row - 1, position.Col}
	case Down:
		return Coord{position.Row + 1, position.Col}
	case Right:
		return Coord{position.Row, position.Col + 1}
	case Left:
		return Coord{position.Row, position.Col - 1}
	}
	return Coord{}
}

func getPerpendicularDir(movDir Direction) Direction {
	if movDir == Up || movDir == Down {
		return Right
	}
	return Down
}

func growDimension(region map[Coord]struct{}, visitedSoFar *map[Coord]struct{}, movDir Direction, point Coord) {
	nextPoint := nextPosition(getPerpendicularDir(movDir), point)

	if _, ok := region[nextPoint]; ok {
		secondStep := nextPosition(movDir, nextPoint)
		if _, seen := region[secondStep]; !seen {
			(*visitedSoFar)[point] = struct{}{}
			growDimension(region, visitedSoFar, movDir, nextPoint)
		}
	}
	(*visitedSoFar)[point] = struct{}{}
}

func consumeRegion(name string, movDir Direction, region, visited map[Coord]struct{}, regionPoints []Coord, numOfSides int) int {
	if len(regionPoints) == 0 {
		return numOfSides
	}

	currentPoint := regionPoints[0]
	restPoints := regionPoints[1:]

	if _, ok := visited[currentPoint]; ok {
		return consumeRegion(name, movDir, region, visited, restPoints, numOfSides)
	}

	nextPoint := nextPosition(movDir, currentPoint)

	if _, ok := region[nextPoint]; ok {
		visited[currentPoint] = struct{}{}
		return consumeRegion(name, movDir, region, visited, restPoints, numOfSides)
	}
	growDimension(region, &visited, movDir, currentPoint)
	return consumeRegion(name, movDir, region, visited, restPoints, numOfSides+1)
}

func exploreRegion(region Region) (sides int) {
	directions := []Direction{Up, Right, Down, Left}

	for _, dir := range directions {
		regionMap := make(map[Coord]struct{})
		for _, p := range region.Points {
			regionMap[p] = struct{}{}
		}
		sides += consumeRegion(region.Name, dir, regionMap, make(map[Coord]struct{}), orderPointsByRowCol(region.Points), 0)
	}

	return
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day12/day12.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		garden := make([][]string, 0)
		for _, line := range fileContent {
			garden = append(garden, strings.Split(line, ""))
		}
		maxRows, maxCols := len(garden), len(garden[0])
		regions := buildRegions(garden, maxRows, maxCols)
		for _, reg := range regions {
			sides := exploreRegion(reg)
			result += reg.Size * sides
		}
	}

	return result
}
