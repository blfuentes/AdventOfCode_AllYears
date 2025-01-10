package day14

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func belongsToQ(robot Robot, qfromX, qtoX, qfromY, qtoY int) bool {
	return (robot.Pos.X >= qfromX && robot.Pos.X <= qtoX &&
		robot.Pos.Y >= qfromY && robot.Pos.Y <= qtoY)
}

func getQuadrant(robot Robot, maxRows, maxCols int) (quadrant int) {
	Q1fromX, Q1toX, Q1fromY, Q1toY := 0, (maxRows/2)-1, 0, (maxCols/2)-1
	Q2fromX, Q2toX, Q2fromY, Q2toY := 0, (maxRows/2)-1, (maxCols/2)+1, maxCols-1
	Q3fromX, Q3toX, Q3fromY, Q3toY := (maxRows/2)+1, maxRows-1, 0, (maxCols/2)-1
	Q4fromX, Q4toX, Q4fromY, Q4toY := (maxRows/2)+1, maxRows-1, (maxCols/2)+1, maxCols-1

	if belongsToQ(robot, Q1fromX, Q1toX, Q1fromY, Q1toY) {
		quadrant = 1
		return
	}
	if belongsToQ(robot, Q2fromX, Q2toX, Q2fromY, Q2toY) {
		quadrant = 2
		return
	}
	if belongsToQ(robot, Q3fromX, Q3toX, Q3fromY, Q3toY) {
		quadrant = 3
		return
	}
	if belongsToQ(robot, Q4fromX, Q4toX, Q4fromY, Q4toY) {
		quadrant = 4
		return
	}
	return
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day14/day14.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		robots := make([]Robot, 0)
		for _, line := range fileContent {
			robots = append(robots, parseLine(line))
		}
		maxrows, maxcols, seconds := 103, 101, 100
		quadrants := [4]int{0, 0, 0, 0}
		for _, r := range robots {
			move(&r, seconds, maxrows, maxcols)
			q := getQuadrant(r, maxrows, maxcols)
			if q > 0 {
				quadrants[q-1]++
			}
		}
		result = quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3]
	}

	return result
}
