package day14

import (
	"fmt"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func findChristmassTree(robots []Robot, maxrows, maxcols, seconds int) (map[Vector]struct{}, bool) {
	newRobots := make([]Robot, 0)
	for _, r := range robots {
		copyRobot := Robot{r.Pos, r.Velocity}
		move(&copyRobot, seconds, maxrows, maxcols)
		newRobots = append(newRobots, copyRobot)
	}
	robotsPositions := make(map[Vector]struct{})
	for _, r := range newRobots {
		robotsPositions[r.Pos] = struct{}{}
	}

	return robotsPositions, len(newRobots) == len(robotsPositions)
}

func printTree(robots map[Vector]struct{}, maxrows, maxcols int) {
	for rowIdx := 0; rowIdx < maxrows; rowIdx++ {
		for colIdx := 0; colIdx < maxcols; colIdx++ {
			if _, ok := robots[Vector{rowIdx, colIdx}]; ok {
				fmt.Printf("#")
			} else {
				fmt.Printf(" ")
			}
		}
		fmt.Println()
	}
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day14/day14.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		robots := make([]Robot, 0)
		for _, line := range fileContent {
			robots = append(robots, parseLine(line))
		}
		maxrows, maxcols, seconds := 103, 101, 1
		for seconds < maxrows*maxcols {
			if possibletree, found := findChristmassTree(robots, maxrows, maxcols, seconds); found {
				result = seconds
				printTree(possibletree, maxrows, maxcols)
				break
			}
			seconds++
		}
	}

	return result
}
