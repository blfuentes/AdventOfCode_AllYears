package day09

import (
	"strings"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type DiskFile struct {
	FileId int
	Space  int
}

func buildChecksum(files *[]DiskFile) int64 {
	values := make([]int, 0)
	for fIdx := 0; fIdx < len(*files); fIdx++ {
		size := (*files)[fIdx].Space
		if size > 0 {
			for sIdx := 0; sIdx < size; sIdx++ {
				values = append(values, (*files)[fIdx].FileId)
			}
		}
	}
	return checksum(&values)
}

func compactFiles2(files *[]DiskFile) []DiskFile {
	fileList := *files
	fileIdx := len(fileList) - 1
	gapIdx := 0
	firstGap := 0

	for fileIdx > gapIdx {
		if fileList[fileIdx].FileId != -1 {
			for gapIdx < fileIdx && (fileList[gapIdx].FileId != -1 || fileList[gapIdx].Space < fileList[fileIdx].Space) {
				gapIdx++
			}

			if gapIdx < fileIdx {
				insertedFile := DiskFile{FileId: fileList[fileIdx].FileId, Space: fileList[fileIdx].Space}
				fileList[gapIdx].Space -= fileList[fileIdx].Space

				fileList = append(fileList[:gapIdx+1], fileList[gapIdx:]...)
				fileList[gapIdx] = insertedFile

				fileList[fileIdx+1].FileId = -1
				delPos := gapIdx + 1
				if fileList[delPos].Space == 0 {
					fileList = append(fileList[:delPos], fileList[delPos+1:]...)
				}
			}
		}

		fileIdx--

		if gapIdx != 0 {
			for gapIdx = firstGap; gapIdx < fileIdx; gapIdx++ {
				if fileList[gapIdx].Space > 0 && fileList[gapIdx].FileId == -1 {
					firstGap = gapIdx
					break
				}
			}
		}
	}

	return fileList
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day09/day09.txt"

	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		fileIdx := 0
		var tmpIdx int
		disk := make([]DiskFile, 0)
		for cIdx, size := range strings.Split(fileContent, "") {
			if cIdx%2 == 0 {
				tmpIdx = fileIdx
				fileIdx++
			} else {
				tmpIdx = -1
			}
			diskfile := DiskFile{tmpIdx, utilities.StringToInt(size)}
			disk = append(disk, diskfile)
		}
		compacteddisk := compactFiles2(&disk)
		result = int(buildChecksum(&compacteddisk))
	}

	return result
}
