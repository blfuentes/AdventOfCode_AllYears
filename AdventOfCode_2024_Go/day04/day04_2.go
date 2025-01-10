package day04

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func hasCrossMas(wordmap [][]string) int {
	middle := wordmap[1][1] == "A"
	lefttop := (wordmap[0][0] == "M" && wordmap[2][2] == "S") || (wordmap[0][0] == "S" && wordmap[2][2] == "M")
	righttop := (wordmap[0][2] == "M" && wordmap[2][0] == "S") || (wordmap[0][2] == "S" && wordmap[2][0] == "M")

	if middle && lefttop && righttop {
		return 1
	}
	return 0
}

func buildSubMap(matrix [][]string, size int) [][][]string {
	rows := len(matrix)
	cols := len(matrix[0])
	var subArrays [][][]string

	for i := 0; i <= rows-size; i++ {
		for j := 0; j <= cols-size; j++ {
			subArray := make([][]string, size)
			for x := 0; x < size; x++ {
				subArray[x] = make([]string, size)
				for y := 0; y < size; y++ {
					subArray[x][y] = matrix[i+x][j+y]
				}
			}
			subArrays = append(subArrays, subArray)
		}
	}
	return subArrays
}

func countXMas(wordmap [][]string) int {
	count := 0
	for _, submap := range buildSubMap(wordmap, 3) {
		count += hasCrossMas(submap)
	}
	return count
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day04/day04.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {

		wordmap := make([][]string, len(fileContent[0]))
		for row := 0; row < len(fileContent); row++ {
			letters := strings.Split(fileContent[row], "")
			wordmap[row] = letters
		}
		result += countXMas(wordmap)
	}

	return result
}
