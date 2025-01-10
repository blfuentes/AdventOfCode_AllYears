package day19

import "strings"

func parseContent(lines string) (map[string]bool, []string, int) {
	patterns := make(map[string]bool, 0)
	designs := make([]string, 0)
	maxSize := 0
	for _, p := range strings.Split(strings.Split(lines, "\r\n\r\n")[0], ", ") {
		if len(p) > maxSize {
			maxSize = len(p)
		}
		patterns[p] = true
	}
	for _, d := range strings.Split(strings.Split(lines, "\r\n\r\n")[1], "\r\n") {
		designs = append(designs, d)
	}

	return patterns, designs, maxSize
}
