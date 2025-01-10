package day13

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day13/day13.txt"
	var extra int64 = 10000000000000
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		equations := strings.Split(fileContent, "\r\n\r\n")
		toSolve := make([]Combination, 0)
		for _, eq := range equations {
			aX, aY := ExtractNumbers(strings.Split(eq, "\n")[0])
			bX, bY := ExtractNumbers(strings.Split(eq, "\n")[1])
			rX, rY := ExtractNumbers(strings.Split(eq, "\n")[2])

			ecuation := Combination{Push{aX, aY}, Push{bX, bY}, rX + extra, rY + extra}
			toSolve = append(toSolve, ecuation)

			resultA, resultB := SolveEquation(ecuation)
			result += resultA*3 + resultB
		}
	}

	return result
}
