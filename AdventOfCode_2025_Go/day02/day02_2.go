package day02

import (
	"strconv"
	"strings"

	"github.com/blfuentes/AdventOfCode_2025_Go/utilities"
)

func isNotValid2(num int64) bool {
	strNum := strconv.FormatInt(num, 10)
	length := len(strNum)

	for patternLen := 1; patternLen <= length/2; patternLen++ {
		if length%patternLen != 0 {
			continue
		}

		valid := true
		for offset := patternLen; offset < length; offset += patternLen {
			for i := 0; i < patternLen; i++ {
				if strNum[i] != strNum[offset+i] {
					valid = false
					break
				}
			}
			if !valid {
				break
			}
		}

		if valid {
			return true
		}
	}

	return false
}

func Executepart2() int64 {
	var result int64 = 0

	// var fileName string = "./day02/test_input_02.txt"
	var fileName string = "./day02/day02.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		for _, parts := range strings.Split(fileContent, ",") {
			init := int64(utilities.StringToInt(strings.TrimSpace(strings.Split(parts, "-")[0])))
			end := int64(utilities.StringToInt(strings.TrimSpace(strings.Split(parts, "-")[1])))
			for i := init; i <= end; i++ {
				if isNotValid2(i) {
					result += i
				}
			}
		}
	}

	return result
}
