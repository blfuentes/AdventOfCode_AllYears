package utilities

import (
	"fmt"
	"os"
	"strings"
)

func ReadFileAsText(path string) (string, error) {
	var fileName string = path
	file, err := os.ReadFile(fileName)
	if err != nil {
		fmt.Printf("Cannot read file %s", fileName)

		return "", err
	}
	fileContent := string(file)

	return fileContent, err
}

func ReadFileAsLines(path string) ([]string, error) {
	var fileName string = path
	file, err := os.ReadFile(fileName)
	if err != nil {
		fmt.Printf("Cannot read file %s", fileName)

		return nil, err
	}
	fileContent := string(file)

	return strings.Split(fileContent, "\n"), err
}

type Int64Array []int64

func (a Int64Array) Len() int           { return len(a) }
func (a Int64Array) Swap(i, j int)      { a[i], a[j] = a[j], a[i] }
func (a Int64Array) Less(i, j int) bool { return a[i] < a[j] }
