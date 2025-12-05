package utilities

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"net/http"
	"os"
	"runtime"
	"strconv"
	"strings"
	"time"

	"github.com/fatih/color"
)

func FormatDuration(d time.Duration) string {
	totalMicros := d.Microseconds()
	minutes := totalMicros / 60_000_000
	seconds := (totalMicros % 60_000_000) / 1_000_000
	millis := (totalMicros % 1_000_000) / 1_000
	micros := totalMicros % 1_000
	return fmt.Sprintf("%02d:%02d:%03d:%03d", minutes, seconds, millis, micros)
}

func RetrieveContent(year, day int) {
	path := fmt.Sprintf("./day%02d/day%02d.txt", day, day)

	if _, err := os.Stat(path); os.IsNotExist(err) {
		os.WriteFile(path, []byte(""), 0644)
	}

	file, err := os.Open(path)
	if err != nil {
		fmt.Printf("Error opening file: %v\n", err)
		return
	}

	scanner := bufio.NewScanner(file)
	var hasContent bool
	for scanner.Scan() {
		if strings.TrimSpace(scanner.Text()) != "" {
			hasContent = true
			break
		}
	}
	file.Close()

	if !hasContent {
		sessionFile, err := os.Open("./session.txt")
		if err != nil {
			fmt.Printf("Error reading session.txt: %v\n", err)
			return
		}
		defer sessionFile.Close()

		sessionScanner := bufio.NewScanner(sessionFile)
		sessionScanner.Scan()
		sessionKey := strings.TrimSpace(sessionScanner.Text())

		url := fmt.Sprintf("https://adventofcode.com/%d/day/%d/input", year, day)
		client := &http.Client{}
		req, err := http.NewRequest("GET", url, nil)
		if err != nil {
			fmt.Printf("Error creating request: %v\n", err)
			return
		}

		req.Header.Add("Cookie", fmt.Sprintf("session=%s", sessionKey))

		resp, err := client.Do(req)
		if err != nil {
			fmt.Printf("Error making request: %v\n", err)
			return
		}
		defer resp.Body.Close()

		body, err := io.ReadAll(resp.Body)
		if err != nil {
			fmt.Printf("Error reading response: %v\n", err)
			return
		}

		content := string(body)

		// only for windows...
		if runtime.GOOS == "windows" && !strings.Contains(content, "\r\n") {
			content = strings.ReplaceAll(content, "\n", "\r\n")
			content = strings.TrimRight(content, "\r\n")
		}

		os.WriteFile(path, []byte(content), 0644)
	}
}

func ReadFileAsText(path string) (string, error) {
	file, err := os.Open(path)
	if err != nil {
		fmt.Printf("Cannot read file %s", path)
		return "", err
	}
	defer file.Close()

	var builder strings.Builder
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		builder.WriteString(scanner.Text())
		builder.WriteString("\n")
	}

	if err := scanner.Err(); err != nil {
		return "", err
	}

	return strings.TrimSuffix(builder.String(), "\n"), nil
}

func ReadFileAsLines(path string) ([]string, error) {
	file, err := os.Open(path)
	if err != nil {
		fmt.Printf("Cannot read file %s", path)
		return nil, err
	}
	defer file.Close()

	var lines []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		lines = append(lines, scanner.Text())
	}

	if err := scanner.Err(); err != nil {
		return nil, err
	}

	return lines, nil
}

func ReverseString(s string) string {
	runes := []rune(s)
	size := len(runes)
	for i, j := 0, size-1; i < size>>1; i, j = i+1, j-1 {
		runes[i], runes[j] = runes[j], runes[i]
	}
	return string(runes)
}

// Function to check if an array contains an element
func Contains[T comparable](arr []T, target T) bool {
	for _, value := range arr {
		if value == target {
			return true
		}
	}
	return false
}

func RemoveElementAt[T any](index int, report []T) []T {
	if index < 0 || index >= len(report) {
		return []T{}
	}

	temp := make([]T, 0)
	for idx, value := range report {
		if idx != index {
			temp = append(temp, value)
		}
	}

	return temp
}

func ReverseArray[T any](arr []T) {
	for i, j := 0, len(arr)-1; i < j; i, j = i+1, j-1 {
		arr[i], arr[j] = arr[j], arr[i]
	}
}

func ReverseCopy[T any](arr []T) []T {
	reversed := make([]T, len(arr))
	copy(reversed, arr)
	ReverseArray(reversed)
	return reversed
}

func Clone2DArray[T any](original [][]T) [][]T {
	if len(original) == 0 {
		return [][]T{}
	}

	cloned := make([][]T, len(original))
	done := make(chan struct{}, len(original))

	for i, row := range original {
		go func(i int, row []T) {
			cloned[i] = make([]T, len(row))
			copy(cloned[i], row)
			done <- struct{}{}
		}(i, row)
	}

	for range original {
		<-done
	}

	return cloned
}

func PrintMatrix[T any](matrix *[][]T) {
	for rowIdx := 0; rowIdx < len(*matrix); rowIdx++ {
		for colIdx := 0; colIdx < len((*matrix)[rowIdx]); colIdx++ {
			fmt.Printf("%v", (*matrix)[rowIdx][colIdx])
		}
		fmt.Println()
	}
}

func PrintMatrixWithColors(matrix [][]int) {
	colors := []func(a ...interface{}) string{
		color.New(color.BgBlack).SprintFunc(),
		color.New(color.BgBlue).SprintFunc(),
		color.New(color.BgGreen).SprintFunc(),
		color.New(color.BgHiMagenta).SprintFunc(),
		color.New(color.BgHiRed).SprintFunc(),
		color.New(color.BgHiWhite).SprintFunc(),
		color.New(color.FgBlue).SprintFunc(),
		color.New(color.BgHiYellow).SprintFunc(),
		color.New(color.FgGreen).SprintFunc(),
		color.New(color.BgBlack).SprintFunc(),
	}

	for rowIdx, row := range matrix {
		for colIdx, value := range row {
			colorIdx := matrix[rowIdx][colIdx]
			coloredValue := colors[colorIdx](value)
			fmt.Print(coloredValue, " ")
		}
		fmt.Println()
	}
}

func StringToInt(value string) int {
	result, _ := strconv.Atoi(value)
	return result
}

func StringToInt64(value string) int64 {
	result, _ := strconv.ParseInt(value, 10, 64)
	return result
}

func IsInBoundaries(row, col, maxrows, maxcols int) bool {
	return row >= 0 && row < maxrows && col >= 0 && col < maxcols
}

func Combinations[T any](list []T, size int) [][]T {
	var combs [][]T
	var generate func([]T, int, []T)
	generate = func(current []T, start int, combination []T) {
		if len(combination) == size {
			comb := make([]T, size)
			copy(comb, combination)
			combs = append(combs, comb)
			return
		}
		for i := start; i < len(current); i++ {
			generate(current, i+1, append(combination, current[i]))
		}
	}
	generate(list, 0, []T{})
	return combs
}

func InsertElementAt[T any](pos int, element T, collection []T) []T {
	return append((collection)[:pos], append([]T{element}, (collection)[pos:]...)...)
}

func DeleteElementAt[T any](pos int, collection []T) []T {
	return append((collection)[:pos], (collection)[pos+1:]...)
}

func SplitNumberInTwo(n int) (int, int) {
	numDigits := func(n int) int {
		if n == 0 {
			return 1
		}
		return int(math.Log10(float64(n))) + 1
	}

	numDigitsCount := numDigits(n)
	middle := numDigitsCount / 2
	divisor := int(math.Pow(10, float64(middle)))
	leftPart := n / divisor
	rightPart := n % divisor

	return leftPart, rightPart
}

func SplitNumber64InTwo(n int64) (int64, int64) {
	numDigits := func(n int64) int {
		if n == 0 {
			return 1
		}
		return int(math.Log10(float64(n))) + 1
	}

	numDigitsCount := numDigits(n)
	middle := numDigitsCount / 2
	divisor := int64(math.Pow(10, float64(middle)))
	leftPart := n / divisor
	rightPart := n % divisor

	return leftPart, rightPart
}

type Int64Array []int64

func (a Int64Array) Len() int           { return len(a) }
func (a Int64Array) Swap(i, j int)      { a[i], a[j] = a[j], a[i] }
func (a Int64Array) Less(i, j int) bool { return a[i] < a[j] }

func SafeModulo(a, b int) int {
	result := a % b
	if result < 0 {
		result += b
	}
	return result
}

func FloorDiv(a, b int) int {
	return int(math.Floor(float64(a) / float64(b)))
}
