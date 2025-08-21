package day20

import (
	"sort"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func TryToCheatTwenty(
	graph [][]Coord, wallpoints []Coord, start Coord, end Coord) int {
	touchedWalls := make(map[Coord]int)
	initialLength, _, _, visitedSorted, _ := FindShortestPath(graph, nil, make(map[Coord]bool), touchedWalls, start, end)
	distances := make(map[Coord]int)
	idx := 0
	for _, coord := range visitedSorted {
		distances[coord] = *initialLength - idx
		idx++
	}
	cheatTimes := make([]int, 0)
	combinations := BuildSpatialRange(visitedSorted)

	for _, combination := range combinations {
		startcheat := combination[0]
		endcheat := combination[1]

		cheatdistance := CheatLength(startcheat, endcheat)
		if cheatdistance <= 20 {
			startcheatdistance := distances[startcheat]
			endcheatdistance := distances[endcheat]

			cheattime := *initialLength - startcheatdistance + cheatdistance + endcheatdistance
			cheatTimes = append(cheatTimes, cheattime)
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

func Executepart2() int {
	var result int = 0

	var fileName string = "./day20/day20.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		themap, wallpoints, startpoint, endpoint := ParseContent(fileContent)
		result = TryToCheatTwenty(themap, wallpoints, startpoint, endpoint)
	}

	return result
}
