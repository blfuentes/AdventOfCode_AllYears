package day17

import (
	"math"
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func performOp(pIdx int, program *Program) (int, int) {
	op, opOp := program.Ops[pIdx], program.Ops[pIdx+1]
	pointerIdx := pIdx + 2
	output := -1
	switch op {
	case int(Adv):
		numerator := (float64)(program.RegA)
		denominator := math.Pow(2, float64(combOperand(opOp, *program)))
		program.RegA = (int64)(numerator / denominator)
	case int(Bxl):
		program.RegB = program.RegB ^ int64(opOp)
	case int(Bst):
		numerator := combOperand(opOp, *program)
		program.RegB = numerator % 8
	case int(Jnz):
		if program.RegA != 0 {
			pointerIdx = opOp
		}
	case int(Bxc):
		program.RegB = program.RegB ^ program.RegC
	case int(Out):
		output = int(combOperand(opOp, *program)) % 8
	case int(Bdv):
		numerator := (float64(program.RegA))
		denominator := math.Pow(2, float64(combOperand(opOp, *program)))
		program.RegB = (int64)(numerator / denominator)
	case int(Cdv):
		numerator := (float64(program.RegA))
		denominator := math.Pow(2, float64(combOperand(opOp, *program)))
		program.RegC = (int64)(numerator / denominator)
	}
	return pointerIdx, output
}

func Executepart1() string {
	var result string = ""

	var fileName string = "./day17/day17.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		program := parseContent(fileContent)
		outputValues := runProgram(program)
		converted := make([]string, 0)
		for _, o := range outputValues {
			converted = append(converted, strconv.Itoa(o))
		}

		result = strings.Join(converted, ",")
	}

	return result
}
