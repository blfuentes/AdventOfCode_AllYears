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
			switch value {
			case "S":
				start.Row, start.Col = row, col
			case "E":
				end.Row, end.Col = row, col
			case "#":
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
		neighbours = append(neighbours, []int{position.Row + dir[0], position.Col + dir[1]})
	}

	return neighbours
}

func IsInBoundaries(row, col, maxRows, maxCols int) bool {
	return row >= 0 && col >= 0 && row < maxRows && col < maxCols
}

func FindShortestPath(
	graph [][]Coord, wallcheat *Coord, visited map[Coord]bool, touchedWalls map[Coord]int, start, end Coord) (*int, map[Coord]bool, map[Coord]int, []Coord, []Coord) {
	maxRows, maxCols := len(graph), len(graph[0])

	queue := make([]QueueItem, 0)
	startingPoint := graph[start.Row][start.Col]
	queue = append(queue, QueueItem{Coord: startingPoint, Path: new(int)})

	touchedWallsSorted := make([]Coord, 0)
	visitedSorted := make([]Coord, 0)

	var bfs func(counter int) (*int, map[Coord]bool, map[Coord]int, []Coord, []Coord)
	bfs = func(counter int) (*int, map[Coord]bool, map[Coord]int, []Coord, []Coord) {
		if len(queue) == 0 {
			return nil, visited, touchedWalls, visitedSorted, touchedWallsSorted
		}

		current := queue[0]
		queue = queue[1:]

		if current.Coord.Row == end.Row && current.Coord.Col == end.Col {
			visited[current.Coord] = true
			visitedSorted = append(visitedSorted, current.Coord)
			return current.Path, visited, touchedWalls, visitedSorted, touchedWallsSorted
		}

		if !visited[current.Coord] &&
			(current.Coord.Kind == Empty ||
				(wallcheat != nil &&
					current.Coord.Row == wallcheat.Row &&
					current.Coord.Col == wallcheat.Col)) {
			visited[current.Coord] = true
			visitedSorted = append(visitedSorted, current.Coord)
			neighbours := Neighbours(current.Coord)
			for _, n := range neighbours {
				if IsInBoundaries(n[0], n[1], maxRows, maxCols) {
					neighbour := graph[n[0]][n[1]]
					if !visited[neighbour] {
						var newPath = new(int)
						*newPath = *current.Path + 1
						queue = append(queue, QueueItem{Coord: neighbour, Path: newPath})
					}
					if neighbour.Kind == Wall {
						if _, exists := touchedWalls[neighbour]; exists {
							if touchedWalls[neighbour] > len(visited) {
								touchedWalls[neighbour] = len(visited)
							}
						} else {
							touchedWalls[neighbour] = len(visited)
							touchedWallsSorted = append(touchedWallsSorted, neighbour)
						}
					}
				}
			}
		}
		return bfs(counter + 1)
	}

	return bfs(0)
}

func TryToCheat(
	graph [][]Coord, wallpoints []Coord, start Coord, end Coord) int {
	maxRows, maxCols := len(graph), len(graph[0])
	touchedWalls := make(map[Coord]int)
	initialLength, _, touchedWalls, visitedSorted, touchedWallsSorted := FindShortestPath(graph, nil, make(map[Coord]bool), touchedWalls, start, end)
	distances := make(map[Coord]int)
	idx := 0
	for _, coord := range visitedSorted {
		distances[coord] = *initialLength - idx
		idx++
	}
	cheatTimes := make([]int, 0)
	for _, cheatWall := range touchedWallsSorted {
		possibleExits := Neighbours(cheatWall)
		for _, exit := range possibleExits {
			nextRow, nextCol := exit[0], exit[1]
			if IsInBoundaries(nextRow, nextCol, maxRows, maxCols) &&
				(graph[nextRow][nextCol].Kind == Empty || distances[Coord{Row: nextRow, Col: nextCol, Kind: graph[nextRow][nextCol].Kind}] != 0) {

				neighbourCoord := Coord{Row: nextRow, Col: nextCol, Kind: graph[nextRow][nextCol].Kind}
				if val, ok := distances[neighbourCoord]; ok {
					wallValue := touchedWalls[cheatWall]
					cheatTimes = append(cheatTimes, 1+wallValue+val)
				}
			}
		}
	}

	groupOfSavings := make([]SavingGroup, 0)
	savingMap := make(map[int]int)

	// Map and filter
	for _, t := range cheatTimes {
		saving := *initialLength - t
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
