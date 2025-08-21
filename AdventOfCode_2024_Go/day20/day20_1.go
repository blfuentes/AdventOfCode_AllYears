package day20

import (
	"sort"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func TryToCheat(
	graph [][]Coord, wallpoints []Coord, start Coord, end Coord) int {
	maxRows, maxCols := len(graph), len(graph[0])
	touchedWalls := make(map[Coord]int)
	initialLength, _, touchedWalls, visitedSorted, touchedWallsSorted := FindShortestPath(graph, nil, make(map[Coord]bool), touchedWalls, start, end)
	distances := make(map[Coord]int)
	idx := 0
	for _, coord := range visitedSorted {
		distances[coord] = *initialLength - idx
		idx++
	}
	cheatTimes := make([]int, 0)
	for _, cheatWall := range touchedWallsSorted {
		possibleExits := Neighbours(cheatWall)
		for _, exit := range possibleExits {
			nextRow, nextCol := exit[0], exit[1]
			if IsInBoundaries(nextRow, nextCol, maxRows, maxCols) &&
				(graph[nextRow][nextCol].Kind == Empty || distances[Coord{Row: nextRow, Col: nextCol, Kind: graph[nextRow][nextCol].Kind}] != 0) {

				neighbourCoord := Coord{Row: nextRow, Col: nextCol, Kind: graph[nextRow][nextCol].Kind}
				if val, ok := distances[neighbourCoord]; ok {
					wallValue := touchedWalls[cheatWall]
					cheatTimes = append(cheatTimes, 1+wallValue+val)
				}
			}
		}
	}

	groupOfSavings := make([]SavingGroup, 0)
	savingMap := make(map[int]int)

	// Map and filter
	for _, t := range cheatTimes {
		saving := *initialLength - t
		if saving > 0 {
			savingMap[saving]++
		}
	}

	// Sort keys
	keys := make([]int, 0, len(savingMap))
	for k := range savingMap {
		keys = append(keys, k)
	}
	sort.Ints(keys)

	// Build result
	for _, k := range keys {
		groupOfSavings = append(groupOfSavings, SavingGroup{Saving: k, Count: savingMap[k]})
	}

	sum := 0
	for _, group := range groupOfSavings {
		if group.Saving >= 100 {
			sum += group.Count
		}
	}

	return sum
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day20/day20.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		themap, wallpoints, startpoint, endpoint := ParseContent(fileContent)
		result = TryToCheat(themap, wallpoints, startpoint, endpoint)
	}

	return result
}
