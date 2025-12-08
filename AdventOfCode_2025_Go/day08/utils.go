package day08

type Box struct {
	Id      int
	X, Y, Z int64
}

type DistancePair struct {
	BoxAId   int
	BoxBId   int
	Distance int64
}

func EuclideanDistanceSquared(from, to Box) int64 {
	dx := to.X - from.X
	dy := to.Y - from.Y
	dz := to.Z - from.Z
	return (dx*dx + dy*dy + dz*dz)
}
