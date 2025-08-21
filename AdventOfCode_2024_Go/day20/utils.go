package day20

import (
	"fmt"
	"math"
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

type BucketKey struct {
	X, Y int
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

func CheatLength(cStart, cEnd Coord) int {
	return int(math.Abs(float64(cStart.Row)-float64(cEnd.Row)) + math.Abs(float64(cStart.Col)-float64(cEnd.Col)))
}

func GetBucketKey(coord Coord, size int) []int {
	return []int{coord.Row / size, coord.Col / size}
}

func BuildSpatialRange(coords []Coord) [][2]Coord {
	bucketSize := 20

	getBucketKey := func(coord Coord) BucketKey {
		return BucketKey{coord.Row / bucketSize, coord.Col / bucketSize}
	}

	spatialHash := make(map[BucketKey][]Coord)

	for _, coord := range coords {
		key := getBucketKey(coord)
		spatialHash[key] = append(spatialHash[key], coord)
	}

	var result [][2]Coord

	for bucketKey, points := range spatialHash {
		bucketX, bucketY := bucketKey.X, bucketKey.Y

		for dx := -1; dx <= 1; dx++ {
			for dy := -1; dy <= 1; dy++ {
				neighborKey := BucketKey{bucketX + dx, bucketY + dy}

				if neighbors, exists := spatialHash[neighborKey]; exists {
					for _, p1 := range points {
						for _, p2 := range neighbors {
							if p1 != p2 && CheatLength(p1, p2) <= 20 {
								result = append(result, [2]Coord{p1, p2})
							}
						}
					}
				}
			}
		}
	}

	return result
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
