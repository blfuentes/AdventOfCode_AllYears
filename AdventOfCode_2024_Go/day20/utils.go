package day20

import (
	"fmt"
	"sort"
	"strings"
)

type KindDef int

const (
	Empty KindDef = iota
	Wall
	Cheat
	Start
	End
)

type Coord struct {
	Row  int
	Col  int
	Kind KindDef
}

type QueueItem struct {
	Coord Coord
	Path  *int
}

type SavingGroup struct {
	Saving int
	Count  int
}

func ParseContent(lines []string) ([][]Coord, []Coord, Coord, Coord) {
	maxrows, maxcols := len(lines), len(lines[0])
	themap := make([][]Coord, 0)
	wallpoints := make([]Coord, 0)
	start, end := Coord{-1, -1, Start}, Coord{-1, -1, End}

	for row := range maxrows {
		line := strings.Split(lines[row], "")
		mapline := make([]Coord, 0)
		for col := range maxcols {
			value := line[col]
			tmpCoord := Coord{row, col, Empty}
			if value == "S" {
				start.Row, start.Col = row, col
				tmpCoord.Kind = Start
			} else if value == "E" {
				end.Row, end.Col = row, col
				tmpCoord.Kind = End
			} else if value == "#" {
				wallpoints = append(wallpoints, Coord{row, col, Wall})
				tmpCoord.Kind = Wall
			}
			mapline = append(mapline, tmpCoord)
		}
		themap = append(themap, mapline)
	}

	return themap, wallpoints, start, end

}

func PrintMatrix(matrix *[][]Coord) {
	for rowIdx := range *matrix {
		for colIdx := range (*matrix)[rowIdx] {
			kind := (*matrix)[rowIdx][colIdx].Kind
			switch kind {
			case Empty:
				fmt.Print(".")
			case Wall:
				fmt.Print("#")
			case Start:
				fmt.Print("S")
			case End:
				fmt.Print("E")
			}
		}
		fmt.Println()
	}
}

func Neighbours(position Coord) [][]int {
	directions := [][]int{
		{-1, 0},
		{1, 0},
		{0, 1},
		{0, -1},
	}
	neighbours := make([][]int, 0)
	for _, dir := range directions {
		neighbours = append(neighbours, []int{dir[0], dir[1]})
	}

	return neighbours
}

func IsInBoundaries(row, col, maxRows, maxCols int) bool {
	return row >= 0 && col >= 0 && row < maxRows && col < maxCols
}

func FindShortestPath(
	graph [][]Coord, wallcheat *Coord, visited map[Coord]bool, touchedWalls map[Coord]int, start, end Coord) (int, map[Coord]bool, map[Coord]int) {
	maxRows, maxCols := len(graph), len(graph[0])

	queue := make([]QueueItem, 0)
	startingPoint := graph[start.Row][start.Col]
	queue = append(queue, QueueItem{Coord: startingPoint, Path: nil})

	var bfs func(counter int) (*int, map[Coord]bool, map[Coord]int)
	bfs = func(counter int) (*int, map[Coord]bool, map[Coord]int) {
		if len(queue) == 0 {
			return nil, visited, touchedWalls
		} else {
			current := queue[0]
			queue = queue[1:]

			if current.Coord.Row == end.Row && current.Coord.Col == end.Col {
				visited[current.Coord] = true
				return current.Path, visited, touchedWalls
			} else if !visited[current.Coord] &&
				(current.Coord.Kind == Empty ||
					(wallcheat != nil &&
						current.Coord.Row == wallcheat.Row &&
						current.Coord.Col == wallcheat.Col)) {
				visited[current.Coord] = true
				neighbours := Neighbours(current.Coord)
				for _, n := range neighbours {
					if IsInBoundaries(n[0], n[1], maxRows, maxCols) {
						neighbour := graph[n[0]][n[1]]
						if !visited[neighbour] {
							var val int
							if current.Path == nil {
								val = 1
							} else {
								val = *current.Path + 1
							}
							nextPath := &val
							queue = append(queue, QueueItem{Coord: neighbour, Path: nextPath})
						}
						if neighbour.Kind == Wall {
							if touchedWalls[neighbour] > len(visited) {
								touchedWalls[neighbour] = len(visited)
							} else {
								touchedWalls[neighbour] = len(visited)
							}

						}
					}
				}
				return nil, visited, touchedWalls
			}

			return bfs(counter + 1)
		}
	}

	pathPtr, visitedResult, touchedWallsResult := bfs(0)
	var path int
	if pathPtr != nil {
		path = *pathPtr
	} else {
		path = -1 // or any default value indicating no path found
	}
	return path, visitedResult, touchedWallsResult
}

func TryToCheat(
	graph [][]Coord, wallpoints []Coord, start Coord, end Coord) int {
	maxRows, maxCols := len(graph), len(graph[0])
	touchedWalls := make(map[Coord]int)
	initialLength, visitedLengths, touchedWalls := FindShortestPath(graph, nil, make(map[Coord]bool), touchedWalls, start, end)
	distances := make(map[Coord]int)
	idx := 0
	for coord, _ := range visitedLengths {
		distances[coord] = initialLength - idx
		idx++
	}
	cheatTimes := make([]int, 0)
	for cheatWall, wallValue := range touchedWalls {
		possibleExits := Neighbours(cheatWall)
		for _, exit := range possibleExits {
			nextRow, nextCol := cheatWall.Row+exit[0], cheatWall.Col+exit[1]
			if IsInBoundaries(nextRow, nextCol, maxRows, maxCols) &&
				(graph[nextRow][nextCol].Kind == Empty || distances[Coord{Row: nextRow, Col: nextCol, Kind: graph[nextRow][nextCol].Kind}] != 0) {

				neighbourCoord := Coord{Row: nextRow, Col: nextCol, Kind: graph[nextRow][nextCol].Kind}
				if val, ok := distances[neighbourCoord]; ok {
					cheatTimes = append(cheatTimes, 1+wallValue+val)
				}
			}
		}
	}

	groupOfSavings := make([]SavingGroup, 0)
	savingMap := make(map[int]int)

	// Map and filter
	for _, t := range cheatTimes {
		saving := initialLength - t
		if saving > 0 {
			savingMap[saving]++
		}
	}

	// Sort keys
	keys := make([]int, 0, len(savingMap))
	for k := range savingMap {
		keys = append(keys, k)
	}
	sort.Ints(keys)

	// Build result
	for _, k := range keys {
		groupOfSavings = append(groupOfSavings, SavingGroup{Saving: k, Count: savingMap[k]})
	}

	sum := 0
	for _, group := range groupOfSavings {
		if group.Saving >= 100 {
			sum += group.Count
		}
	}

	return sum
}
