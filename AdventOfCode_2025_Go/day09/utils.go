package day09

func abs64(x int64) int64 {
	if x < 0 {
		return -x
	}
	return x
}

func rectangleArea(x1, y1, x2, y2 int64) int64 {
	return (abs64(x2-x1) + 1) * (abs64(y2-y1) + 1)
}
