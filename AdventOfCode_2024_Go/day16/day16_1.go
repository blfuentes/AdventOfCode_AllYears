package day16

import (
	"container/heap"
	"math"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func consumepath(queue *utilities.PriorityQueue[PositionDir],
	graph *[][][]Cell, maxrows, maxcols int, themap [][]string, endnode Position) int {
	if queue.Len() == 0 {
		lowest := math.MaxInt32
		for d := 0; d < 4; d++ {
			if (*graph)[endnode.Row][endnode.Col][d].Distance <= lowest {
				lowest = (*graph)[endnode.Row][endnode.Col][d].Distance
			}
		}
		return lowest
	}

	current := heap.Pop(queue).(*utilities.Item[PositionDir]).Value
	if alreadyVisited(current.Pos, current.Dir, *graph) {
		return consumepath(queue, graph, maxrows, maxcols, themap, endnode)
	}

	setVisited(current.Pos, current.Dir, graph)
	neighbours := Neighbours(current.Pos, current.Dir)
	for _, n := range neighbours {
		if isValidCoord(n.PDir.Pos, maxrows, maxcols, themap) &&
			!(alreadyVisited(n.PDir.Pos, n.PDir.Dir, *graph)) {
			currentDistance := (*graph)[current.Pos.Row][current.Pos.Col][current.Dir].Distance
			n.Distance = currentDistance + n.Distance

			if n.Distance <= (*graph)[n.PDir.Pos.Row][n.PDir.Pos.Col][n.PDir.Dir].Distance {
				setDistance(n.PDir.Pos, n.PDir.Dir, n.Distance, graph)
				heap.Push(queue, &utilities.Item[PositionDir]{Value: n.PDir, Priority: n.Distance, Index: -1})
			}
		}
	}

	return consumepath(queue, graph, maxrows, maxcols, themap, endnode)
}

func djikstraExplore(themap [][]string, startnode, endnode Position) int {
	maxrows, maxcols := len(themap), len(themap[0])
	graph := buildGraph(themap)
	queue := make(utilities.PriorityQueue[PositionDir], 0)
	heap.Init(&queue)

	setDistance(startnode, 0, 0, &graph)
	sn := PositionDir{Position{startnode.Row, startnode.Col}, 0}
	heap.Push(&queue, &utilities.Item[PositionDir]{Value: sn, Priority: 0, Index: -1})

	return consumepath(&queue, &graph, maxrows, maxcols, themap, endnode)
}

func Executepart1() int {
	var result int = 0

	var fileName string = "./day16/day16.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		themap, startnode, endnode := parseContent(fileContent)
		result = djikstraExplore(themap, startnode, endnode)

	}

	return result
}
