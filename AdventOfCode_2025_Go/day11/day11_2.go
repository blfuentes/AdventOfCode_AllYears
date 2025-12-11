package day11

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

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

		var dfs func(cache map[string]int64, current, end string) int64
		dfs = func(cache map[string]int64, current, end string) int64 {
			if current == end {
				return 1
			}
			if v, ok := cache[current]; ok {
				return v
			}

			totalCount := int64(0)
			if device, ok := devices[current]; ok {
				for _, neighbor := range device.Outputs {
					totalCount += dfs(cache, neighbor, end)
				}
			}
			cache[current] = totalCount
			return totalCount
		}

		// svr -> fft
		start, end := "svr", "fft"
		svr_fft := dfs(make(map[string]int64), start, end)

		// svr -> dac
		start, end = "svr", "dac"
		svr_dac := dfs(make(map[string]int64), start, end)

		// fft -> dac
		start, end = "fft", "dac"
		fft_dac := dfs(make(map[string]int64), start, end)

		// dac -> fft
		start, end = "dac", "fft"
		dac_fft := dfs(make(map[string]int64), start, end)

		// dac -> out
		start, end = "dac", "out"
		dac_out := dfs(make(map[string]int64), start, end)

		// fft -> out
		start, end = "fft", "out"
		fft_out := dfs(make(map[string]int64), start, end)

		result = svr_fft*fft_dac*dac_out + svr_dac*dac_fft*fft_out
	}

	return result
}
