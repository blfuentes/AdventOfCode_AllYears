package day01

import (
	"math"
	"sort"
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func calculateDistance(leftside, rightside []int) (result int) {
	for idx := 0; idx < len(leftside); idx++ {
		result += int(math.Abs(float64(leftside[idx]) - float64(rightside[idx])))
	}

	return
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day01/day01.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		left := make([]int, len(fileContent))
		right := make([]int, len(fileContent))
		for lineIdx := 0; lineIdx < len(fileContent); lineIdx++ {
			parts := strings.Split(fileContent[lineIdx], " ")
			left[lineIdx], _ = strconv.Atoi(parts[0])
			right[lineIdx], _ = strconv.Atoi(parts[(len(parts) - 1)])
		}
		sort.Ints(left)
		sort.Ints(right)
		result = calculateDistance(left, right)
	}

	return result
}
