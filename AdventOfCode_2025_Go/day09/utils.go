package day09

import "math"

func rectangleArea(x1, y1, x2, y2 int64) int64 {
	return int64((math.Abs(float64(x2-x1)) + 1) * (math.Abs(float64(y2-y1)) + 1))
}
