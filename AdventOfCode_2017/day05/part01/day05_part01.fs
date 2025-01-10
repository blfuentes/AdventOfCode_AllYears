module day05_part01


open AdventOfCode_2017.Modules.LocalHelper

let rec calculateJumps (input: int[]) (index: int) (steps: int) =
    let length = input.Length
    if index < 0 || index >= length then
        steps
    else
        let jump = input.[index]
        let newIndex = index + jump
        input.[index] <- input.[index] + 1
        calculateJumps input newIndex (steps + 1)

let execute() =
    let path = "day05/day05_input.txt"
    let input = GetLinesFromFile path |> Array.map int
    calculateJumps input 0 0