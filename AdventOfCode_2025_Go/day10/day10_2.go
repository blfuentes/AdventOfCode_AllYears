package day10

import (
	"regexp"
	"sync"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func gaussianElimination(matrix [][]int) ([]int, [][]int) {
	numRows := len(matrix)
	if numRows == 0 {
		return nil, nil
	}
	numCols := len(matrix[0]) - 1

	pivotCols := []int{}
	currentRow := 0

	reducedMatrix := make([][]int, numRows)
	for rowIdx := range matrix {
		reducedMatrix[rowIdx] = make([]int, numCols+1)
		copy(reducedMatrix[rowIdx], matrix[rowIdx])
	}

	for col := 0; col < numCols && currentRow < numRows; col++ {
		pivotRow := -1
		for row := currentRow; row < numRows; row++ {
			if reducedMatrix[row][col] != 0 {
				pivotRow = row
				break
			}
		}

		if pivotRow == -1 {
			continue
		}

		reducedMatrix[currentRow], reducedMatrix[pivotRow] = reducedMatrix[pivotRow], reducedMatrix[currentRow]
		pivotCols = append(pivotCols, col)

		for row := currentRow + 1; row < numRows; row++ {
			if reducedMatrix[row][col] != 0 {
				factor := reducedMatrix[row][col]
				pivotVal := reducedMatrix[currentRow][col]

				for colIdx := col; colIdx <= numCols; colIdx++ {
					reducedMatrix[row][colIdx] = reducedMatrix[row][colIdx]*pivotVal - reducedMatrix[currentRow][colIdx]*factor
				}
			}
		}

		currentRow++
	}

	return pivotCols, reducedMatrix
}

func solveSystem(machine Machine) []int {
	buttons := machine.Buttons
	joltages := machine.Joltages

	numButtons := len(buttons)
	numPositions := len(joltages)

	// Build coefficient matrix
	coefficientMatrix := make([][]int, numPositions)
	for posIdx := range coefficientMatrix {
		coefficientMatrix[posIdx] = make([]int, numButtons+1)
		for btnIdx := 0; btnIdx < numButtons; btnIdx++ {
			buttonAffectsPosition := false
			for _, affectedPos := range buttons[btnIdx] {
				if affectedPos == posIdx {
					buttonAffectsPosition = true
					break
				}
			}
			if buttonAffectsPosition {
				coefficientMatrix[posIdx][btnIdx] = 1
			}
		}
		coefficientMatrix[posIdx][numButtons] = joltages[posIdx]
	}

	pivotCols, reducedMatrix := gaussianElimination(coefficientMatrix)
	if reducedMatrix == nil {
		return nil
	}

	pivotSet := make(map[int]bool)
	for _, col := range pivotCols {
		pivotSet[col] = true
	}

	freeVariables := []int{}
	for btnIdx := 0; btnIdx < numButtons; btnIdx++ {
		if !pivotSet[btnIdx] {
			freeVariables = append(freeVariables, btnIdx)
		}
	}

	bestSolution := make([]int, numButtons)
	bestTotalPresses := -1

	var trySolution func(freeValues []int)
	trySolution = func(freeValues []int) {
		solution := make([]int, numButtons)

		for idx, varIdx := range freeVariables {
			if idx < len(freeValues) {
				solution[varIdx] = freeValues[idx]
			}
		}

		for pivotIdx := len(pivotCols) - 1; pivotIdx >= 0; pivotIdx-- {
			rowIdx := pivotIdx
			colIdx := pivotCols[pivotIdx]
			rightHandSide := reducedMatrix[rowIdx][numButtons]

			for substitutionCol := colIdx + 1; substitutionCol < numButtons; substitutionCol++ {
				rightHandSide -= reducedMatrix[rowIdx][substitutionCol] * solution[substitutionCol]
			}

			pivotCoefficient := reducedMatrix[rowIdx][colIdx]
			if pivotCoefficient == 0 {
				return
			}

			if rightHandSide%pivotCoefficient != 0 {
				return
			}

			variableValue := rightHandSide / pivotCoefficient
			if variableValue < 0 {
				return
			}

			solution[colIdx] = variableValue
		}

		for posIdx := 0; posIdx < numPositions; posIdx++ {
			computedValue := 0
			for btnIdx := 0; btnIdx < numButtons; btnIdx++ {
				if solution[btnIdx] > 0 {
					for _, affectedPos := range buttons[btnIdx] {
						if affectedPos == posIdx {
							computedValue += solution[btnIdx]
							break
						}
					}
				}
			}
			if computedValue != joltages[posIdx] {
				return
			}
		}

		totalPresses := 0
		for _, presses := range solution {
			totalPresses += presses
		}

		if bestTotalPresses == -1 || totalPresses < bestTotalPresses {
			copy(bestSolution, solution)
			bestTotalPresses = totalPresses
		}
	}

	if len(freeVariables) == 0 {
		trySolution([]int{})
	} else if len(freeVariables) == 1 {
		maxSearchValue := 0
		for _, joltage := range joltages {
			if joltage > maxSearchValue {
				maxSearchValue = joltage
			}
		}
		maxSearchValue *= 3
		for value := 0; value <= maxSearchValue; value++ {
			if bestTotalPresses != -1 && value > bestTotalPresses {
				break
			}
			trySolution([]int{value})
		}
	} else if len(freeVariables) == 2 {
		maxSearchValue := 0
		for _, joltage := range joltages {
			if joltage > maxSearchValue {
				maxSearchValue = joltage
			}
		}
		if maxSearchValue < 200 {
			maxSearchValue = 200
		}
		for value1 := 0; value1 <= maxSearchValue; value1++ {
			for value2 := 0; value2 <= maxSearchValue; value2++ {
				if bestTotalPresses != -1 && value1+value2 > bestTotalPresses {
					continue
				}
				trySolution([]int{value1, value2})
			}
		}
	} else if len(freeVariables) == 3 {
		for value1 := 0; value1 < 250; value1++ {
			for value2 := 0; value2 < 250; value2++ {
				for value3 := 0; value3 < 250; value3++ {
					if bestTotalPresses != -1 && value1+value2+value3 > bestTotalPresses {
						continue
					}
					trySolution([]int{value1, value2, value3})
				}
			}
		}
	} else if len(freeVariables) == 4 {
		for value1 := 0; value1 < 30; value1++ {
			for value2 := 0; value2 < 30; value2++ {
				for value3 := 0; value3 < 30; value3++ {
					for value4 := 0; value4 < 30; value4++ {
						if bestTotalPresses != -1 && value1+value2+value3+value4 > bestTotalPresses {
							continue
						}
						trySolution([]int{value1, value2, value3, value4})
					}
				}
			}
		}
	} else {
		trySolution(make([]int, len(freeVariables)))
	}

	if bestTotalPresses == -1 {
		return make([]int, numButtons)
	}

	return bestSolution
}

func minimumPresses(machine Machine) int {
	solution := solveSystem(machine)
	totalPresses := 0
	for _, presses := range solution {
		totalPresses += presses
	}
	return totalPresses
}

func Executepart2() int {
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
				presses := minimumPresses(m)
				resultChan <- presses
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
