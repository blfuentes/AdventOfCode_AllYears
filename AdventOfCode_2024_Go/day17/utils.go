package day17

import (
	"regexp"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Program struct {
	RegA int64
	RegB int64
	RegC int64
	Ops  []int
}

type OpDef int

const (
	Adv OpDef = iota
	Bxl
	Bst
	Jnz
	Bxc
	Out
	Bdv
	Cdv
)

func parseContent(lines string) Program {
	if re, err := regexp.Compile("\\d+"); err == nil {
		founds := re.FindAllString(lines, -1)
		regA, regB, regC := founds[0], founds[1], founds[2]
		program := Program{}
		program.RegA = utilities.StringToInt64(regA)
		program.RegB = utilities.StringToInt64(regB)
		program.RegC = utilities.StringToInt64(regC)

		for pIdx := 3; pIdx < len(founds); pIdx++ {
			program.Ops = append(program.Ops, utilities.StringToInt(founds[pIdx]))
		}

		return program
	}

	return Program{}
}

func combOperand(op int, program Program) int64 {
	switch op {
	case int(Adv):
		return 0
	case int(Bxl):
		return 1
	case int(Bst):
		return 2
	case int(Jnz):
		return 3
	case int(Bxc):
		return program.RegA
	case int(Out):
		return program.RegB
	case int(Bdv):
		return program.RegC
	default:
		return -1
	}
}

func runProgram(program Program) []int {
	opIdx, outputValues := 0, make([]int, 0)
	for opIdx < len(program.Ops) {
		newIdx, output := performOp(opIdx, &program)
		opIdx = newIdx
		if output != -1 {
			outputValues = append(outputValues, output)
		}
	}
	return outputValues
}
