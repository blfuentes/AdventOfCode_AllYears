package day17

import (
	"math"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type DoubleIdx struct {
	PIdx   int64
	Mindex int
}

func findNewRegister(program Program) int64 {
	reversed := utilities.ReverseCopy(program.Ops)
	checkStack := utilities.Stack[DoubleIdx]{}
	currentSolution := math.MaxInt64

	checkStack.Push(DoubleIdx{0, 0})
	for !checkStack.IsEmpty() {
		tocheck, _ := checkStack.Pop()
		for bitIdx := 0; bitIdx <= 8; bitIdx++ {
			cloneProgram := Program{int64(bitIdx) + tocheck.PIdx, program.RegB, program.RegC, program.Ops}
			output := runProgram(cloneProgram)
			if output[0] == reversed[tocheck.Mindex] {
				if tocheck.Mindex+1 >= len(program.Ops) {
					if bitIdx+int(tocheck.PIdx) < currentSolution {
						currentSolution = bitIdx + int(tocheck.PIdx)
					}
				} else {
					checkStack.Push(DoubleIdx{8 * (tocheck.PIdx + int64(bitIdx)), tocheck.Mindex + 1})
				}
			}
		}
	}

	return int64(currentSolution)
}

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day17/day17.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		program := parseContent(fileContent)
		result = findNewRegister(program)
	}

	return result
}
