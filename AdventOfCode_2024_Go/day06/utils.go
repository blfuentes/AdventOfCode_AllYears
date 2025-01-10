package day06

type DefinedDir int

const (
	UP DefinedDir = iota
	DOWN
	RIGHT
	LEFT
	NONE
)

type Coord struct {
	Row, Col int
}

type Direction struct {
	Mov         Coord
	Orientation DefinedDir
}

func TurnRight(dir Direction) Direction {
	switch dir.Orientation {
	case UP:
		return Direction{Coord{0, 1}, RIGHT}
	case DOWN:
		return Direction{Coord{0, -1}, LEFT}
	case RIGHT:
		return Direction{Coord{1, 0}, DOWN}
	case LEFT:
		return Direction{Coord{-1, 0}, UP}
	}

	return Direction{Coord{0, 0}, NONE}
}
