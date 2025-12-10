package day10

import (
	"strconv"
	"strings"
)

type Machine struct {
	Id           int
	LightDiagram []bool
	Buttons      [][]int
	Joltages     []int
}

func parseNumbers(s string) []int {
	var nums []int
	parts := strings.Split(s, ",")
	for _, part := range parts {
		part = strings.TrimSpace(part)
		if num, err := strconv.Atoi(part); err == nil {
			nums = append(nums, num)
		}
	}
	return nums
}

func boolsToUint64(bools []bool) uint64 {
	var result uint64
	for i, b := range bools {
		if b {
			result |= (1 << i)
		}
	}
	return result
}

func buttonToBitmask(buttonIndices []int, maxBits int) uint64 {
	var mask uint64
	for _, idx := range buttonIndices {
		if idx >= 0 && idx < maxBits {
			mask |= (1 << idx)
		}
	}
	return mask
}

// CombinationWithRepetition generates all combinations of length num from list, with repetition allowed
func CombinationWithRepetition[T any](num int, list []T) [][]T {
	if num == 0 {
		return [][]T{{}}
	}

	if len(list) == 0 {
		return [][]T{}
	}

	x := list[0]
	xs := list[1:]

	var result [][]T

	withX := CombinationWithRepetition(num-1, list)
	for _, combo := range withX {
		newCombo := append([]T{x}, combo...)
		result = append(result, newCombo)
	}

	withoutX := CombinationWithRepetition(num, xs)
	result = append(result, withoutX...)

	return result
}

func FindCombination(machine Machine) int {
	buttonMasks := make([]uint64, len(machine.Buttons))
	numBits := len(machine.LightDiagram)

	for i, button := range machine.Buttons {
		buttonMasks[i] = buttonToBitmask(button, numBits)
	}

	targetState := boolsToUint64(machine.LightDiagram)
	initialState := uint64(0)

	buttonIndices := make([]int, len(machine.Buttons))
	for i := range buttonIndices {
		buttonIndices[i] = i
	}

	for pressCount := 0; ; pressCount++ {
		combinations := CombinationWithRepetition(pressCount, buttonIndices)

		for _, buttonSequence := range combinations {
			finalState := initialState

			// apply XOR
			for _, btnIdx := range buttonSequence {
				finalState ^= buttonMasks[btnIdx]
			}

			if finalState == targetState {
				return len(buttonSequence)
			}
		}
	}
}
