package day02

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func isNotValid1(num int64) bool {
	strNum := strconv.FormatInt(num, 10)
	if len(strNum)%2 != 0 {
		return false
	}
	mid := len(strNum) / 2
	firstHalf := strNum[:mid]
	secondHalf := strNum[mid:]
	return firstHalf == secondHalf
}

func Executepart1() int64 {
	var result int64 = 0

	// var fileName string = "./day02/test_input_02.txt"
	var fileName string = "./day02/day02.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		for _, parts := range strings.Split(fileContent, ",") {
			init := int64(utilities.StringToInt(strings.TrimSpace(strings.Split(parts, "-")[0])))
			end := int64(utilities.StringToInt(strings.TrimSpace(strings.Split(parts, "-")[1])))
			for i := init; i <= end; i++ {
				if isNotValid1(i) {
					result += i
				}
			}
		}
	}

	return result
}
