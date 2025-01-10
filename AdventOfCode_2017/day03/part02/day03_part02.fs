module day03_part02

open AdventOfCode_2017.Modules.LocalHelper

let getValueForPosition (position: int[]) (spiral: int[,]) =
    let rowRange = [-1..1]
    let columnRange = [-1..1]
    let value = [|0|]
    for row in rowRange do
        for column in columnRange do
            value.[0] <- value.[0] + spiral[position.[0] + row, position.[1] + column]
    value.[0]

let rec buildSpiral (mid: int) (input: int) (result: int[]) (ring: int) (spiral: int[,]) =
    if result.[0] > input then
        result.[0]
    else
        if ring = 0 then
            spiral[mid, mid] <- 1
        else
            // building right side
            let rightRange = [(-ring + 1) .. (ring - 1)] |> Seq.rev
            for row in rightRange do
                let valueToAdd = getValueForPosition [|mid + row; mid + ring|] spiral
                spiral[mid + row, mid + ring] <- valueToAdd
                if valueToAdd > input && result.[0] = 0 then
                    result.[0] <- valueToAdd
                else
                    ()
            // building top side
            let topRange = [-ring .. ring] |> Seq.rev
            for column in topRange do
                let valueToAdd = getValueForPosition [|mid - ring; mid + column|] spiral
                spiral[mid - ring, mid + column] <- valueToAdd
                if valueToAdd > input && result.[0] = 0 then
                    result.[0] <- valueToAdd
                else
                    ()
            // building left side
            let leftRange = [(-ring + 1) .. (ring - 1)]
            for row in leftRange do
                let valueToAdd = getValueForPosition [|mid + row; mid - ring|] spiral
                spiral[mid + row, mid - ring] <- valueToAdd
                if valueToAdd > input && result.[0] = 0 then
                    result.[0] <- valueToAdd
                else
                    ()
            // building bottom side
            let downRange = [-ring .. ring]
            for column in downRange do
                let valueToAdd = getValueForPosition [|mid + ring; mid + column|] spiral
                spiral[mid + ring, mid + column] <- valueToAdd
                if valueToAdd > input && result.[0] = 0 then
                    result.[0] <- valueToAdd                   
                else
                    ()
        buildSpiral mid input result (ring + 1) spiral

let execute() =
    let path = "day03/day03_input.txt"
    let input = GetContentFromFile path |> int
    let length = 1000
    let spiral = Array2D.create length length 0
    let mid = length / 2
    buildSpiral mid input [|0|] 0 spiral