﻿module day03_part01

open System

open AdventOfCode_2017.Modules.LocalHelper

let calculateManhattanDistance (pointA: int[]) (pointB: int[])=
    Math.Abs(pointA.[0] - pointB.[0]) + Math.Abs(pointA.[1] - pointB.[1]);

let rec buildSpiral (mid: int) (input: int) (current: int) (currentPosition: int[]) (ring: int) (spiral: int[,]) =
    if current > input then
        (spiral, currentPosition)
    else
        let numberOfElements = ring * 8
        let maxValue = current + numberOfElements
        let mutable valueToAdd = maxValue + 1
        if ring = 0 then
            spiral[mid, mid] <- 1
            currentPosition[0] <- mid
            currentPosition[1] <- mid
        else
            // building in the corner right down
            let downRange = [-ring .. ring] |> Seq.rev
            for column in downRange do        
                spiral[mid + ring, mid + column] <- valueToAdd
                if valueToAdd = input then
                    currentPosition[0] <- mid + ring
                    currentPosition[1] <- mid + column                    
                else
                    ()
                valueToAdd <- valueToAdd - 1
            // building left side
            let leftRange = [(-ring + 1) .. (ring - 1)] |> Seq.rev
            for row in leftRange do
                spiral[mid + row, mid - ring] <- valueToAdd
                if valueToAdd = input then
                    currentPosition[0] <- mid + row
                    currentPosition[1] <- mid - ring
                else
                    ()
                valueToAdd <- valueToAdd - 1
            // building top side
            let topRange = [-ring .. ring]
            for column in topRange do
                spiral[mid - ring, mid + column] <- valueToAdd
                if valueToAdd = input then
                    currentPosition[0] <- mid - ring
                    currentPosition[1] <- mid + column
                else
                    ()
                valueToAdd <- valueToAdd - 1
            // building right side
            let rightRange = [(-ring + 1) .. (ring - 1)]
            for row in rightRange do
                spiral[mid + row, mid + ring] <- valueToAdd
                if valueToAdd = input then
                    currentPosition[0] <- mid + row
                    currentPosition[1] <- mid + ring
                else
                    ()
                valueToAdd <- valueToAdd - 1
        buildSpiral mid input maxValue currentPosition (ring + 1) spiral

let execute() =
    let path = "day03/day03_input.txt"
    let input = GetContentFromFile path |> int
    let length = 1000
    let spiral = Array2D.create length length 0
    let mid = length / 2
    let memorySpiral = buildSpiral mid input 0 [|0; 0|] 0 spiral
    calculateManhattanDistance [|mid; mid|] (snd memorySpiral)
