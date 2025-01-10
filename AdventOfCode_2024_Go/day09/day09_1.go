package day09

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func compactFiles(files *[]int) []int {
	fileIdx := len((*files)) - 1
	gapIdx := 0
	for gapIdx < fileIdx {
		val := (*files)[gapIdx]
		if val == -1 {
			for fileIdx > gapIdx && (*files)[fileIdx] == -1 {
				fileIdx--
			}
			if fileIdx > gapIdx {
				fileVal := (*files)[fileIdx]
				(*files)[gapIdx] = fileVal
				(*files)[fileIdx] = -1
			}
		} else {
			gapIdx++
		}
	}
	return (*files)[:gapIdx]
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day09/day09.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		fileIdx := 0
		var tmpIdx int
		disk := make([]int, 0)
		for cIdx, r := range strings.Split(fileContent, "") {
			if cIdx%2 == 0 {
				tmpIdx = fileIdx
				fileIdx++
			} else {
				tmpIdx = -1
			}
			for sIdx := 0; sIdx < utilities.StringToInt(r); sIdx++ {
				disk = append(disk, tmpIdx)
			}
		}
		compacteddisk := compactFiles(&disk)
		result = int(checksum(&compacteddisk))
	}

	return result
}
