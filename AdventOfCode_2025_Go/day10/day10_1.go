package day10

import (
	"regexp"
	"sync"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	// var fileName string = "./day10/test_input_10.txt"
	var fileName string = "./day10/day10.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		machines := make([]Machine, len(fileContent))

		mainRegex := regexp.MustCompile(`\[([^\]]+)\](.+)\{([^\}]+)\}`)
		parenRegex := regexp.MustCompile(`\(([^\)]+)\)`)

		for idx, line := range fileContent {
			match := mainRegex.FindStringSubmatch(line)

			tmpMachine := Machine{Id: idx}
			tmpMachine.LightDiagram = make([]bool, len(match[1]))
			for i, char := range match[1] {
				tmpMachine.LightDiagram[i] = (char == '#')
			}

			parenMatches := parenRegex.FindAllStringSubmatch(match[2], -1)
			tmpMachine.Buttons = make([][]int, 0, len(parenMatches))
			for _, m := range parenMatches {
				if len(m) > 1 {
					nums := parseNumbers(m[1])
					tmpMachine.Buttons = append(tmpMachine.Buttons, nums)
				}
			}

			tmpMachine.Joltages = parseNumbers(match[3])
			machines[idx] = tmpMachine
		}

		var wg sync.WaitGroup
		resultChan := make(chan int, len(machines))

		for _, machine := range machines {
			wg.Add(1)
			go func(m Machine) {
				defer wg.Done()
				resultChan <- FindCombination(m)
			}(machine)
		}

		go func() {
			wg.Wait()
			close(resultChan)
		}()

		for count := range resultChan {
			result += count
		}
	}

	return result
}
