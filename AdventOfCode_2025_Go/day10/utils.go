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

func applyButton(state []bool, buttonIndices []int) []bool {
	newState := make([]bool, len(state))
	copy(newState, state)

	for _, idx := range buttonIndices {
		if idx >= 0 && idx < len(newState) {
			newState[idx] = !newState[idx]
		}
	}

	return newState
}

func slicesEqual(a, b []bool) bool {
	if len(a) != len(b) {
		return false
	}
	for i := range a {
		if a[i] != b[i] {
			return false
		}
	}
	return true
}

func FindCombination(machine Machine) int {
	buttonIndices := make([]int, len(machine.Buttons))
	for i := range buttonIndices {
		buttonIndices[i] = i
	}

	initialState := make([]bool, len(machine.LightDiagram))

	for pressCount := 0; ; pressCount++ {
		combinations := CombinationWithRepetition(pressCount, buttonIndices)

		for _, buttonSequence := range combinations {
			finalState := make([]bool, len(initialState))
			copy(finalState, initialState)

			for _, btnIdx := range buttonSequence {
				finalState = applyButton(finalState, machine.Buttons[btnIdx])
			}

			if slicesEqual(finalState, machine.LightDiagram) {
				return len(buttonSequence)
			}
		}
	}
}
