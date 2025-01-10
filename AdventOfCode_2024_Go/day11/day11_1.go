package day11

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func MutateStone(stone int) []int {
	if stone == 0 {
		return []int{1}
	}
	if len(strconv.Itoa(stone))%2 == 0 {
		left, right := utilities.SplitNumberInTwo(stone)
		return []int{left, right}
	}

	return []int{stone * 2024}
}

func Blink1(numoOfBlinks int, stones *[]int) int {
	doBlink := func(blink int) {
		for blink > 0 {
			newstones := make([]int, 0)
			for _, s := range *stones {
				newstones = append(newstones, MutateStone(s)...)
			}
			stones = &newstones
			blink--
		}
	}

	doBlink(numoOfBlinks)

	return len(*stones)
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day11/day11.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		parts := strings.Split(fileContent, " ")
		numStones := len(parts)
		stones := make([]int, numStones)
		for idx := 0; idx < numStones; idx++ {
			stones[idx] = utilities.StringToInt(parts[idx])
		}

		result = Blink1(25, &stones)
	}

	return result
}
