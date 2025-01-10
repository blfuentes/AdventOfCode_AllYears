module day19_part02

open AdventOfCode_2016.Modules

let findIndex (d: int) =
    let mutable factor = 1
    while (pown 3 factor) <= d do
        factor <- factor + 1
    let current = pown 3 (factor - 1) + 1
    let inc2 = pown 3 factor * 2 / 3
    max (2 * (d - inc2)) 0 + (min d inc2 - current) + 1

let execute =
    let path = "day19/day19_input.txt"
    let content = LocalHelper.GetContentFromFile path |> int
    findIndex content