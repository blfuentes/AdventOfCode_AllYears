package day10

import (
	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func FindPath2(initnodes []*Node) int {
	found := make([]Coord, 0) //make(map[Coord]bool)

	var FindPathRecursive func(node Node)
	FindPathRecursive = func(node Node) {
		if node.Name == 9 {
			found = append(found, node.Position)
		} else {
			for _, neighbor := range node.Neighbors {
				if neighbor.Name-node.Name == 1 {
					FindPathRecursive(*neighbor)
				}
			}
		}
	}

	for _, node := range initnodes {
		FindPathRecursive(*node)
	}

	return len(found)
}

func Executepart2() int {
	var result int = 0

	var fileName string = "./day10/day10.txt"

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		nodes := make(map[Coord]*Node)
		initnodes := make([]*Node, 0)
		for i, line := range fileContent {
			for j, char := range line {
				coord := Coord{i, j}
				nodes[coord] = &Node{int(char - '0'), coord, make([]*Node, 0)}
			}
		}
		movs := []Coord{{-1, 0}, {1, 0}, {0, -1}, {0, 1}}
		for _, node := range nodes {
			for _, mov := range movs {
				neighbor := Coord{node.Position.Row + mov.Row, node.Position.Col + mov.Col}
				if neighborNode, ok := nodes[neighbor]; ok {
					node.Neighbors = append(node.Neighbors, neighborNode)
				}
			}
			if node.Name == 0 {
				initnodes = append(initnodes, node)
			}
		}
		result = FindPath2(initnodes)
	}

	return result
}
