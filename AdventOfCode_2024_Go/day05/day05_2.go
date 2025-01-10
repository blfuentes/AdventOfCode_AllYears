package day05

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func compare(left, right int, rules [][]int) bool {
	return containsRule(left, right, rules)
}

func SortElements(elements []int, rules [][]int) {
	for init := 0; init < len(elements)-1; init++ {
		for inner := 0; inner < len(elements)-init-1; inner++ {
			if !compare(elements[inner], elements[inner+1], rules) {
				elements[inner], elements[inner+1] = elements[inner+1], elements[inner]
			}
		}
	}
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day05/day05.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		parts := strings.Split(fileContent, "\r\n\r\n")
		rules := make([][]int, 0)
		tobechecked := make([][]int, 0)
		for _, value := range strings.Split(parts[0], "\r\n") {
			prev := utilities.StringToInt(strings.Split(value, "|")[0])
			next := utilities.StringToInt(strings.Split(value, "|")[1])
			rules = append(rules, []int{prev, next})
		}
		for _, value := range strings.Split(parts[1], "\r\n") {
			serie := make([]int, 0)
			for _, value2 := range strings.Split(value, ",") {
				serie = append(serie, utilities.StringToInt(value2))
			}
			tobechecked = append(tobechecked, serie)
		}

		for cIdx := 0; cIdx < len(tobechecked); cIdx++ {
			if !IsInOrder(tobechecked[cIdx], rules) {
				SortElements(tobechecked[cIdx], rules)
				mid := len(tobechecked[cIdx]) / 2
				result += tobechecked[cIdx][mid]
			}
		}
	}

	return result
}
