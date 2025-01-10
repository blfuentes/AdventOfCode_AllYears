package day07

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func calculate2(expected int64, eqparams []int64, index int) bool {
	if index < 0 {
		return false
	}
	lastparam := eqparams[index]
	if index == 0 {
		return expected == lastparam
	}
	if expected%lastparam == 0 && calculate2(expected/lastparam, eqparams, index-1) {
		return true
	}
	if expected > lastparam && calculate2(expected-lastparam, eqparams, index-1) {
		return true
	}

	expectedAsString := strconv.FormatInt(expected, 10)
	lastparamAsString := strconv.FormatInt(lastparam, 10)

	if len(expectedAsString) > len(lastparamAsString) &&
		strings.HasSuffix(expectedAsString, lastparamAsString) {

		newTarget := utilities.StringToInt64(expectedAsString[:len(expectedAsString)-len(lastparamAsString)])
		return calculate2(newTarget, eqparams, index-1)
	}

	return false
}

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day07/day07.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for index := 0; index < len(fileContent); index++ {
			expected := utilities.StringToInt64(strings.Split(fileContent[index], ":")[0])
			values := make([]int64, 0)
			for _, val := range strings.Split(strings.TrimSpace(strings.Split(fileContent[index], ":")[1]), " ") {
				values = append(values, utilities.StringToInt64(val))
			}
			if calculate2(expected, values, len(values)-1) {
				result += expected
			}
		}
	}

	return result
}
