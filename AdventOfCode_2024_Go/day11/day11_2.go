package day11

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Blink2(numoOfBlinks int, stonedata *map[int64]int64) int64 {
	insertOrUpdate := func(m *map[int64]int64, key int64, value int64) {
		if currentValue, ok := (*m)[key]; ok {
			(*m)[key] = currentValue + value
		} else {
			(*m)[key] = value
		}
	}
	for numoOfBlinks > 0 {
		newStoneData := make(map[int64]int64)
		for key, value := range *stonedata {
			length := len(strconv.FormatInt(key, 10))
			if key == 0 {
				insertOrUpdate(&newStoneData, 1, value)
			} else if length%2 == 0 {
				left, right := utilities.SplitNumber64InTwo(key)
				insertOrUpdate(&newStoneData, left, value)
				insertOrUpdate(&newStoneData, right, value)
			} else {
				insertOrUpdate(&newStoneData, key*2024, value)
			}
		}
		stonedata = &newStoneData
		numoOfBlinks--
	}
	var result int64
	for _, v := range *stonedata {
		result += v
	}

	return result
}

func Executepart2() int64 {
	var result int64 = 0

	var fileName string = "./day11/day11.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		parts := strings.Split(fileContent, " ")
		numStones := len(parts)
		stonecount := make(map[int64]int64)
		for idx := 0; idx < numStones; idx++ {
			stonecount[utilities.StringToInt64(parts[idx])] = 1
		}

		result = Blink2(75, &stonecount)
	}

	return result
}
