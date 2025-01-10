package day10

type Coord struct {
	Row int
	Col int
}

type Node struct {
	Name      int
	Position  Coord
	Neighbors []*Node
}
