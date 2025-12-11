package day11

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day11/test_input_11.txt"
	var fileName string = "./day11/day11.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		devices := make(map[string]Device, 0)
		for _, line := range fileContent {
			from, outputs, _ := strings.Cut(line, ":")
			devices[from] = Device{Name: from, Outputs: strings.Split(strings.TrimSpace(outputs), " ")}
		}

		//
		start, end := "you", "out"
		var dfs func(current Device)
		dfs = func(current Device) {
			if utilities.Contains(current.Outputs, end) {
				result++
			} else {
				for _, neighbor := range current.Outputs {
					dfs(devices[neighbor])
				}
			}
		}
		dfs(devices[start])
	}

	return result
}
