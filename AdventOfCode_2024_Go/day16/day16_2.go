package day16

import (
	"container/heap"
	"math"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

var updateDistanceAndPath = func(pos Position, dir, distance int, paths [][]Position, graph *[][][]Cell) {
	currentNode := (*graph)[pos.Row][pos.Col][dir]
	newpaths := make([][]Position, 0)
	if distance < currentNode.Distance {
		newpaths = append(newpaths, paths...)
	} else if distance == currentNode.Distance {
		newpaths = append(newpaths, append(paths, currentNode.Paths...)...)
	}
	(*graph)[pos.Row][pos.Col][dir] = Cell{currentNode.Visited, distance, newpaths}
}

func consumepath2(queue *utilities.PriorityQueue[PositionDir],
	graph *[][][]Cell, maxrows, maxcols int, themap [][]string, endnode Position) int {
	if queue.Len() == 0 {
		lowest := math.MaxInt32
		for d := 0; d < 4; d++ {
			if (*graph)[endnode.Row][endnode.Col][d].Distance <= lowest {
				lowest = (*graph)[endnode.Row][endnode.Col][d].Distance
			}
		}
		uniquetepaths := make(map[Position]struct{})

		for d := 0; d < 4; d++ {
			if (*graph)[endnode.Row][endnode.Col][d].Distance == lowest {
				for _, p := range (*graph)[endnode.Row][endnode.Col][d].Paths {
					for _, sp := range p {
						uniquetepaths[sp] = struct{}{}
					}
				}
			}
		}

		return len(uniquetepaths)
	}

	current := heap.Pop(queue).(*utilities.Item[PositionDir]).Value
	if alreadyVisited(current.Pos, current.Dir, *graph) {
		return consumepath2(queue, graph, maxrows, maxcols, themap, endnode)
	}

	setVisited(current.Pos, current.Dir, graph)
	neighbours := Neighbours(current.Pos, current.Dir)
	for _, n := range neighbours {
		if isValidCoord(n.PDir.Pos, maxrows, maxcols, themap) &&
			!(alreadyVisited(n.PDir.Pos, n.PDir.Dir, *graph)) {
			currentDistance := (*graph)[current.Pos.Row][current.Pos.Col][current.Dir].Distance

			newpaths := make([][]Position, 0)
			for _, path := range (*graph)[current.Pos.Row][current.Pos.Col][current.Dir].Paths {
				newsubpaths := make([]Position, 0)
				newsubpaths = append(newsubpaths, n.PDir.Pos)
				newsubpaths = append(newsubpaths, path...)
				newpaths = append(newpaths, newsubpaths)
			}
			n.Distance = currentDistance + n.Distance

			if n.Distance <= (*graph)[n.PDir.Pos.Row][n.PDir.Pos.Col][n.PDir.Dir].Distance &&
				len(newpaths) > 0 {

				updateDistanceAndPath(n.PDir.Pos, n.PDir.Dir, n.Distance, newpaths, graph)
				heap.Push(queue, &utilities.Item[PositionDir]{Value: n.PDir, Priority: n.Distance, Index: -1})
			}
		}
	}

	return consumepath2(queue, graph, maxrows, maxcols, themap, endnode)
}

func djikstraExplore2(themap [][]string, startnode, endnode Position) int {
	maxrows, maxcols := len(themap), len(themap[0])
	graph := buildGraph(themap)
	queue := make(utilities.PriorityQueue[PositionDir], 0)
	heap.Init(&queue)

	initialparth := make([][]Position, 0)
	firstpos := make([]Position, 0)
	firstpos = append(firstpos, startnode)
	initialparth = append(initialparth, firstpos)
	updateDistanceAndPath(startnode, 0, 0, initialparth, &graph)

	sn := PositionDir{Position{startnode.Row, startnode.Col}, 0}
	heap.Push(&queue, &utilities.Item[PositionDir]{Value: sn, Priority: 0, Index: -1})

	return consumepath2(&queue, &graph, maxrows, maxcols, themap, endnode)
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day16/day16.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		themap, startnode, endnode := parseContent(fileContent)
		result = djikstraExplore2(themap, startnode, endnode)
	}

	return result
}
