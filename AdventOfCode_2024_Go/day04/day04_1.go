package day04

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func isWord(wordmap [][]string, word string, row, rowDir, column, columnDir int) bool {
	for c := 0; c < len(word); c++ {
		if row < 0 || row >= len(wordmap) || column < 0 || column >= len(wordmap[row]) {
			return false
		}
		value := wordmap[row][column]
		if value != string(word[c]) {
			return false
		}
		row += rowDir
		column += columnDir
	}

	return true
}

func countWords(wordmap [][]string, word string, row, column int) (count int) {
	directions := []int{-1, 0, 1}
	for _, rowDir := range directions {
		for _, colDir := range directions {
			if isWord(wordmap, word, row, rowDir, column, colDir) {
				count++
			}
		}
	}
	return
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day04/day04.txt"
	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {

		wordmap := make([][]string, len(fileContent[0]))
		for row := 0; row < len(fileContent); row++ {
			letters := strings.Split(fileContent[row], "")
			wordmap[row] = letters
		}

		for row := 0; row < len(fileContent); row++ {
			for col := 0; col < len(fileContent[row]); col++ {
				result = result + countWords(wordmap, "XMAS", row, col)
			}
		}

	}

	return result
}
