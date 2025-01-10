package day14

import (
	"regexp"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

type Vector struct {
	X int
	Y int
}

type Robot struct {
	Pos      Vector
	Velocity Vector
}

func parseLine(line string) Robot {
	regexp := regexp.MustCompile(`p=(-?\d+),(-?\d+)\s+v=(-?\d+),(-?\d+)`)
	matches := regexp.FindStringSubmatch(line)
	posX, posY := utilities.StringToInt(matches[2]), utilities.StringToInt(matches[1])
	velX, velY := utilities.StringToInt(matches[4]), utilities.StringToInt(matches[3])
	return Robot{Vector{posX, posY}, Vector{velX, velY}}
}

func move(robot *Robot, times, maxrows, maxcols int) {
	newX := ((*robot).Pos.X + (*robot).Velocity.X*times) % maxrows
	if newX < 0 {
		newX = maxrows + newX
	}
	newY := ((*robot).Pos.Y + (*robot).Velocity.Y*times) % maxcols
	if newY < 0 {
		newY = maxcols + newY
	}
	(*robot).Pos = Vector{newX, newY}
}
