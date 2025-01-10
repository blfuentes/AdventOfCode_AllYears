package day13

import (
	"regexp"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Push struct {
	X int64
	Y int64
}

type Combination struct {
	ButtonA Push
	ButtonB Push
	ResultX int64
	ResultY int64
}

func ExtractNumbers(equation string) (int64, int64) {
	regexp := regexp.MustCompile(`(\d+)[^\d]+(\d+)`)
	matches := regexp.FindStringSubmatch(equation)
	return utilities.StringToInt64(matches[1]), utilities.StringToInt64(matches[2])
}

func SolveEquation(ecuation Combination) (int64, int64) {
	partA1 := (ecuation.ButtonB.Y*ecuation.ResultX - ecuation.ButtonB.X*ecuation.ResultY)
	partA2 := (ecuation.ButtonA.X*ecuation.ButtonB.Y - ecuation.ButtonA.Y*ecuation.ButtonB.X)
	mulA := partA1 / partA2
	partB1 := (ecuation.ButtonA.Y*ecuation.ResultX - ecuation.ButtonA.X*ecuation.ResultY)
	partB2 := (ecuation.ButtonA.Y*ecuation.ButtonB.X - ecuation.ButtonA.X*ecuation.ButtonB.Y)
	mulB := partB1 / partB2

	newPointA := Push{ecuation.ButtonA.X * mulA, ecuation.ButtonA.Y * mulA}
	newPointB := Push{ecuation.ButtonB.X * mulB, ecuation.ButtonB.Y * mulB}
	newPoint := Push{newPointA.X + newPointB.X, newPointA.Y + newPointB.Y}

	if newPoint.X == ecuation.ResultX && newPoint.Y == ecuation.ResultY {
		return mulA, mulB
	}
	return 0, 0
}
