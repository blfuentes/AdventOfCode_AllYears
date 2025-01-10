package day04

import (
	"crypto/md5"
	"fmt"
	"io"
)

func Executepart2() int {
	var input string = "yzbqklnj"
	var result int = 0
	var isValid bool = false
	hasher := md5.New()
	for !isValid {
		io.WriteString(hasher, input+fmt.Sprint(result))
		isValid = isValidHash6(fmt.Sprintf("%x", hasher.Sum(nil)))
		hasher.Reset()
		result++
	}
	return result - 1
}

func isValidHash6(hash string) bool {
	return hash[0] == '0' && hash[1] == '0' && hash[2] == '0' && hash[3] == '0' && hash[4] == '0' && hash[5] == '0'
}
