package day11

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

type Key struct {
	k string
	p string
}

func buildP(path utilities.Set[string]) string {
	return strings.Join(path.Values(), "_")
}

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day11/test_input_11b.txt"
	var fileName string = "./day11/day11.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		devices := make(map[string]Device, 0)
		for _, line := range fileContent {
			from, outputs, _ := strings.Cut(line, ":")
			devices[from] = Device{Name: from, Outputs: strings.Split(strings.TrimSpace(outputs), " ")}
		}

		//
		start, end := "svr", "out"
		required := utilities.NewSetWithValues("dac", "fft")
		cache := make(map[Key]int64)
		var dfs func(visited, requiredVisited utilities.Set[string], current string) int64
		dfs = func(visited, requiredVisited utilities.Set[string], current string) int64 {
			if current == end {
				if required.Difference(&requiredVisited).IsEmpty() {
					return 1
				}
				return 0
			}

			key := Key{k: current, p: buildP(requiredVisited)}
			if v, ok := cache[key]; ok {
				return v
			}

			totalCount := int64(0)
			if device, ok := devices[current]; ok {
				for _, neighbor := range device.Outputs {
					if !visited.Contains(neighbor) {
						newRequired := utilities.NewSet[string]()
						for _, v := range requiredVisited.Values() {
							newRequired.Add(v)
						}
						if required.Contains(neighbor) {
							newRequired.Add(neighbor)
						}
						newVisited := utilities.NewSet[string]()
						for _, v := range visited.Values() {
							newVisited.Add(v)
						}
						newVisited.Add(neighbor)
						totalCount += dfs(*newVisited, *newRequired, neighbor)
					}
				}
			}
			cache[key] = totalCount
			return totalCount
		}
		result = dfs(*utilities.NewSetWithValues(start), *utilities.NewSet[string](), start)
	}

	return result
}
